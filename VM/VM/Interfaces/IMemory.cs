// Namespace...
namespace VM.Interfaces;

// Interface that defines a Memory.
public interface IMemory {

	// The Size of this Memory.
	uint Size { get; }

	// Reads an Address for the Memory.
	int Read(uint Address);

	// Writes a Value to an Memory Address.
	void Write(uint Address, int Value);

	// Used to Validate if a specific memory Address is valid.
	bool IsAddressValid(uint Address);
}

