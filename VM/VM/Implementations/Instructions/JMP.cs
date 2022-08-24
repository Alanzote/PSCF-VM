// Usings...
using VM.Interfaces;

// Namespace...
namespace VM.Implementations.Instructions;

// The JMP Instruction.
public class JMP : IInstruction {
	// The OpCode.
	public static int OpCode => 16;

	// The Name of the Instruction.
	private const string Name = "JMP {0}";

	// Reads the Parameters from Memory of this Instruction.
	private static uint ReadParams(Registers Registers, IMemory Memory) {
		return (uint) Memory.Read(Registers.PC);
	}

	// Gets the Instruction name, formatted.
	public static string GetFormattedName(Registers Registers, IMemory Memory) {
		// Read Parameters.
		var Params = ReadParams(Registers, Memory);

		// Return Formatted Name.
		return string.Format(Name, Params);
	}

	// Executes the Instruction.
	public static void Execute(Registers Registers, IMemory Memory, IO IO) {
		// Read Params.
		var Params = ReadParams(Registers, Memory);

		// Set Program Counter to Read Address.
		Registers.PC = Params;
	}
}

