// Usings...
using VM.Interfaces;

// Namespace...
namespace VM.Implementations.Instructions;

// The No Operation Instruction, does Nothing.
public class NOP : IInstruction {

	// The OpCode.
	public static int OpCode => 0;

	// The Name of the Instruction in Case we need to Debug.
	private const string Name = "NOP";

	// Gets the Name of the Instruction.
	public static string GetFormattedName(Registers Registers, IMemory Memory) {
		return Name;
	}

	// Executes the Operation.
	public static void Execute(Registers Registers, IMemory Memory, IO IO) {
		// We don't do a thing here since the Program Counter is Automatically Incremented.
	}
}

