// Usings.
using VM.Interfaces;

// Namespace...
namespace VM.Implementations.Memory;

// RAM Class.
public class RAM : IMemory {

	// The Memory of this RAM.
	private int[] FullMemory;

	// Size of this RAM.
	public uint Size => (uint) FullMemory.Length;

	// RAM should be the final hierarquie, so its actual size is the full size.
	public uint ActualSize => Size;

	// Constructor.
	public RAM(uint Size) {
		// Create full memory.
		FullMemory = new int[Size];
	}

	// Reads at an Address.
	public int Read(uint Address) {
		// Validate Address.
		if (!IsAddressValid(Address))
			throw new ArgumentOutOfRangeException(nameof(Address));

		// Read.
		return FullMemory[Address];
	}

	// Writes at an Address.
	public void Write(uint Address, int Value) {
		// Validate Address.
		if (!IsAddressValid(Address))
			throw new ArgumentOutOfRangeException(nameof(Address));

		// Write.
		FullMemory[Address] = Value;
	}

	// Checks if an Address is Valid.
	public bool IsAddressValid(uint Address) {
		// Validate.
		return Address >= 0 && Address < Size;
	}

	// Grabs the Parent of this Memory.
	public IMemory? GetParent() {
		return null;
	}
}

