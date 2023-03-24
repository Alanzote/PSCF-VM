// Usings...
using System;
using VM.Interfaces;

// Namespace...
namespace VM.Implementations.Memory.Cache;

// Create Direct Cache Memory.
public class DirectCache : IMemory {

	// Our Parent Memory.
	private readonly IMemory _parent;

	// The Size of the Full Memory, this will go all the way up when using
	// multiple Caches.
	public uint Size => _parent.Size;

	// Size of only the Current Cache.
	public uint ActualSize => (uint) (CacheMatrix.GetLength(0) * CacheMatrix.GetLength(1));

	// If each line is initialized.
	private readonly bool[] CacheInit;

	// Dirty Flags.
	private readonly bool[] CacheDirty;

	// The Cache Matrix.
	private readonly int[,] CacheMatrix;

	// The Cache Tags.
	private readonly uint[] CacheTags;

	// Our Values for Calculating Indexes.
	private readonly uint W;
	private readonly uint R;
	private readonly int R_Offset;
	private readonly uint T;
	private readonly int T_Offset;

	// Constructor.
	public DirectCache(uint Size, uint Lines, IMemory Parent) {
		// Set Data.
		_parent = Parent;

		// Make sure Size and Lines are Multiples of two
		// and that we can divide our full size based on
		// the amount of lines.
		if (Size % 2 != 0 || Size % Lines != 0)
			throw new ArgumentException("Size needs to be multiple of 2 and Size needs to be fully divisable by the Number of Lines.");

		// The Length of a Line.
		uint LineLength = Size / Lines;

		// Calculate Bits.
		int W_Bits = (int) Math.Ceiling(Math.Log2(LineLength));
		int R_Bits = (int) Math.Ceiling(Math.Log2(Lines));
		int T_Bits = (int) Math.Ceiling(Math.Log2(_parent.Size / Size));

		// Get Values for Calculating Indexes.
		W = (uint) (1 << W_Bits) - 1;
		R = (uint) (1 << (R_Bits + W_Bits)) - 1;
		T = (uint) (1 << (T_Bits + R_Bits + W_Bits)) - 1;

		// Calculate Offsets.
		R_Offset = W_Bits;
		T_Offset = W_Bits + R_Bits;

		// Create Cache.
		CacheInit = new bool[Lines];
		CacheDirty = new bool[Lines];
		CacheMatrix = new int[Lines, LineLength];
		CacheTags = new uint[Lines];
	}

	// If this Address is Valid.
	public bool IsAddressValid(uint Address) {
		return Address >= 0 && Address < Size; // We check FULL SIZE.
	}

	// Copies to the Cache if MISS.
	private void CopyToCache(uint r, uint t) {
		// If HIT, we just exit our function.
		if (CacheInit[r] && CacheTags[r] == t)
			return;

		// Calculate Start Address.
		uint NewStartAddress = (t << T_Offset) | (r << R_Offset);
		uint OldStartAddress = (CacheTags[t] << T_Offset) | (r << R_Offset);

		// Tis a miss, let's loop our Cache line.
		for (uint i = 0; i < CacheMatrix.GetLength(1); i++) {
			// If this line is initialized and it is dirty
			// we will be writing back to the old start address.
			if (CacheInit[r] && CacheDirty[r])
				_parent.Write(OldStartAddress + i, CacheMatrix[r, i]);

			// Calculate new Address.
			uint Address = NewStartAddress + i;

			// Copy Data Over.
			CacheMatrix[r, i] = IsAddressValid(Address) ? _parent.Read(Address) : 0;
		}

		// Set Tag.
		CacheTags[r] = t;

		// Mark Cache Line as Initialized.
		CacheInit[r] = true;

		// At this point, our cache line isn't Dirty anymore.
		CacheDirty[r] = false;
	}

	// Calculates W, T and R.
	private Tuple<uint, uint, uint> CalculateCachePosition(uint Address) {
		return Tuple.Create(
			Address & W,
			(Address & R & ~W) >> R_Offset,
			(Address & T & ~R & ~W) >> T_Offset
		);
	}

	// Read the valye at an Address.
	public int Read(uint Address) {
		// Check for Address Validity.
		if (!IsAddressValid(Address))
			throw new ArgumentOutOfRangeException(nameof(Address));

		// Calculate Cache Position.
		var Pos = CalculateCachePosition(Address);

		// Call Copy to Cache, that will check for MISS and Copy data if needed.
		CopyToCache(Pos.Item2, Pos.Item3);

		// We can just return the data since copy to cache fills in the blanks for us.
		return CacheMatrix[Pos.Item2, Pos.Item1];
	}

	// Writes the value to an Address.
	public void Write(uint Address, int Value) {
		// Check for Address Validity.
		if (!IsAddressValid(Address))
			throw new ArgumentOutOfRangeException(nameof(Address));

		// Calculate Cache Position.
		var Pos = CalculateCachePosition(Address);

		// Call Copy to Cache, that will check for MISS and Copy data if needed.
		CopyToCache(Pos.Item2, Pos.Item3);

		// Write to the Position.
		CacheMatrix[Pos.Item2, Pos.Item1] = Value;

		// Notify this Cache Line is Dirty.
		CacheDirty[Pos.Item2] = true;
	}

	// Gets the Parent of this Cache.
	public IMemory? GetParent() {
		return _parent;
	}
}

