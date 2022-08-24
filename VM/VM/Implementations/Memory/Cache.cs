// Usings...
using System;
using VM.Interfaces;

// Namespace...
namespace VM.Implementations.Memory;

// Create Cache Memory.
public class Cache : IMemory {

	// Our Parent Memory.
	private readonly IMemory _parent;

	// If cache is Initialized.
	private bool Initialized = false;

	// This a Dirty Flag, which indicates that the Cache was written to.
	private bool IsDirty = false;

	// The Address the Cache begins.
	private uint CacheStartAddress;

	// Memory associated to this cache.
	private readonly int[] CacheMemory;

	// The Size of the Full Memory, this will go all the way up when using
	// multiple Caches.
	public uint Size => _parent.Size;

	// Size of only the Current Cache.
	public uint ActualSize => (uint) CacheMemory.Length;

	// Constructor.
	public Cache(uint Size, IMemory Parent) {
		// Set Data.
		_parent = Parent;

		// Create Cache Memory.
		CacheMemory = new int[Size];
	}

	// If this Address is Valid.
	public bool IsAddressValid(uint Address) {
		return Address >= 0 && Address < Size; // We check FULL SIZE.
	}

	// Copies data to the Cache.
	private void CopyToCache(uint NewAddress) {
		// Check for Cache Hit.
		if (Initialized && NewAddress >= CacheStartAddress && NewAddress < (CacheStartAddress + ActualSize))
			return; // We don't copy anything, it is a Hit!

		// Cache Miss, let's loop for our Cache Size.
		for (uint i = 0; i < ActualSize; i++) {
			// If we are initialized, this means we are switching cache position
			// therefore, we must copy stuff over to Parent Memory that we have on the Cache, but
			// only if we have been written to.
			if (Initialized && IsDirty)
				_parent.Write(CacheStartAddress + i, CacheMemory[i]);

			// Calculate Address.
			uint Address = NewAddress + i;

			// Copy From Parent Memory.
			CacheMemory[i] = IsAddressValid(Address) ? _parent.Read(Address) : 0;
		}

		// Set Cache Start Address.
		CacheStartAddress = NewAddress;

		// Mark if we are Initialized.
		Initialized = true;

		// At this point, everything was written back to the Parent Memory if needed, so we are no longer dirty.
		IsDirty = false;
	}

	// Reads the value at an Address.
	public int Read(uint Address) {
		// Check for Address Validity.
		if (!IsAddressValid(Address))
			throw new ArgumentOutOfRangeException(nameof(Address));

		// Request Cache Copy, this will only actually trigger on a Miss.
		CopyToCache(Address);

		// Return from Cache.
		return CacheMemory[Address - CacheStartAddress];
	}

	// Writes the value at an Address.
	public void Write(uint Address, int Value) {
		// Check for Address Validity.
		if (!IsAddressValid(Address))
			throw new ArgumentOutOfRangeException(nameof(Address));

		// Request Cache Copy, this will only actually trigger on a Miss.
		CopyToCache(Address);

		// Write to the Cache.
		CacheMemory[Address - CacheStartAddress] = Value;

		// This Cache is now Dirty and data shall be copied to RAM Next Copy.
		IsDirty = true;
	}
}

