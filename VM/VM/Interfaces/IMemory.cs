// Namespace...
namespace VM.Interfaces;

// Interface that defines a Memory.
public interface IMemory {

	// The Size of this Memory.
	// FULL hierarquical memory.
	uint Size { get; }

	// The Actual Size of this Memory.
	// Memory of this Single Memory Chip.
	uint ActualSize { get; }

	// Reads an Address for the Memory.
	int Read(uint Address);

	// Writes a Value to an Memory Address.
	void Write(uint Address, int Value);

	// Used to Validate if a specific memory Address is valid.
	bool IsAddressValid(uint Address);

	// Grabs the Parent of this Memory, if any.
	IMemory? GetParent();
}

