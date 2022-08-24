// Usings...
using VM.Exceptions;
using VM.Interfaces;

// Namespace...
namespace VM.Implementations.Instructions;

// The HALT Operation stop the processor execution.
public class HALT : IInstruction {

	// The OpCode for the Instruction.
	public static int OpCode => 1;

	// The Name of the Instruction.
	private const string Name = "HALT";

	// Gets the Instruction name, formatted.
	public static string GetFormattedName(Registers Registers, IMemory Memory) {
		return Name;
	}

	// Executes the Instruction.
	public static void Execute(Registers Registers, IMemory Memory, IO IO) {
		// So our while loop will catch this, we just raise the Simulation Halt Exception.
		throw new SimulationHaltException();
	}
}

