// Usings...
using VM.Interfaces;

// Namespace...
namespace VM;

// Our CPU Class.
public class CPU {

	// The Program Counter.
	private uint ProgramCounter;

	// References to our IO and Memory.
	private readonly IO Io;
	private readonly IMemory Memory;

	// Construtor.
	public CPU(IO Io, IMemory Memory) {
		// Sets
		this.Io = Io;
		this.Memory = Memory;
	}

	// Runs the Program at the Address.
	public void Run(uint Address) {
		// Set Program Counter.
		ProgramCounter = Address;

		// Read Start and End Points.
		int StartPoint = Memory.Read(ProgramCounter);
		int EndPoint = Memory.Read(ProgramCounter + 1);

		// Make sure Operation is Valid.
		if (EndPoint < StartPoint)
			throw new InvalidOperationException();

		// Write Values from 1 to n from start point to end point.
		for (var i = 0; i <= (EndPoint - StartPoint); i++)
			Memory.Write((uint) (StartPoint + i), i + 1);
	}
}

