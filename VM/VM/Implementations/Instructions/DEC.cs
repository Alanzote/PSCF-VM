// Usings...
using VM.Interfaces;

// Namespace...
namespace VM.Implementations.Instructions;

// The DEC Instruction.
public class DEC : IInstruction {
	// The OpCode.
	public static int OpCode => 14;

	// The Name of the Instruction for formatting.
	private const string Name = "DEC {0}";

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

		// Add Reg02 to Reg01.
		Registers.SetByRegCode(Params, Registers.GetByRegCode(Params) - 1);

		// Add Program Counter.
		Registers.PC++;
	}
}

