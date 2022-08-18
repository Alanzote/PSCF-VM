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

	// The Address the Cache begins.
	private uint CacheStartAddress;

	// Memory associated to this cache.
	private readonly int[] CacheMemory;

	// The Size of the Cache.
	public uint Size => (uint) CacheMemory.Length;

	// Constructor.
	public Cache(uint Size, IMemory Parent) {
		// Set Data.
		_parent = Parent;

		// Create Cache Memory.
		CacheMemory = new int[Size];
	}

	// If this Address is Valid.
	public bool IsAddressValid(uint Address) {
		return Address >= 0 && Address < _parent.Size;
	}

	// Copies data to the Cache.
	private void CopyToCache(uint NewAddress) {
		// For with the size of the Cache
		for (uint i = 0; i < Size; i++) {
			// If we are initialized, this means we are switching cache position
			// therefore, we must copy stuff over to RAM that we have on the Cache.
			if (Initialized)
				_parent.Write(CacheStartAddress + i, CacheMemory[i]);

			// Calculate Address.
			uint Address = NewAddress + i;

			// Copy From RAM.
			CacheMemory[i] = IsAddressValid(Address) ? _parent.Read(Address) : 0;
		}

		// Set Cache Start Address.
		CacheStartAddress = NewAddress;

		// Mark if we are Initialized.
		Initialized = true;
	}

	// Reads the value at an Address.
	public int Read(uint Address) {
		// Check for Address Validity.
		if (!IsAddressValid(Address))
			throw new ArgumentOutOfRangeException(nameof(Address));

		// Check Cache Miss, copy.
		if (!Initialized || Address < CacheStartAddress || Address >= (CacheStartAddress + Size))
			CopyToCache(Address);

		// Return from Cache.
		return CacheMemory[Address - CacheStartAddress];
	}

	// Writes the value at an Address.
	public void Write(uint Address, int Value) {
		// Check for Address Validity.
		if (!IsAddressValid(Address))
			throw new ArgumentOutOfRangeException(nameof(Address));

		// Check Cache Miss, copy.
		if (!Initialized || Address < CacheStartAddress || Address >= (CacheStartAddress + Size))
			CopyToCache(Address);

		// Write to the Cache.
		CacheMemory[Address - CacheStartAddress] = Value;
	}
}

