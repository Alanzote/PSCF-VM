// Usings...
using VM.Interfaces;

// Namespace...
namespace VM.Implementations.Instructions;

// The INC Instruction.
public class INC : IInstruction {
	// The OpCode.
	public static int OpCode => 13;

	// The Name of the Instruction.
	private const string Name = "INC {0}";

	// Reads the Parameters from Memory of this Instruction.
	private static int ReadParams(Registers Registers, IMemory Memory) {
		return Memory.Read(Registers.PC);
	}

	// Gets the Instruction name, formatted.
	public static string GetFormattedName(Registers Registers, IMemory Memory) {
		// Read Parameters.
		var Params = ReadParams(Registers, Memory);

		// Return Formatted Name.
		return string.Format(Name, Registers.GetNameByRegCode(Params));
	}

	// Executes the Instruction.
	public static void Execute(Registers Registers, IMemory Memory, IO IO) {
		// Get Parameters.
		var Params = ReadParams(Registers, Memory);

		// Add 1 to Reg at Params.
		Registers.SetByRegCode(Params, Registers.GetByRegCode(Params) + 1);

		// Add Program Counter.
		Registers.PC++;
	}
}

