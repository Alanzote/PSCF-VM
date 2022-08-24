// Usings...
using VM.Interfaces;

// Namespace...
namespace VM.Implementations.Instructions;

// The JG Instruction.
public class JG : IInstruction {
	// The OpCode.
	public static int OpCode => 18;

	// The Name of the Instruction.
	private const string Name = "JG {0}";

	// Reads the Parameters from Memory of this Instruction.
	private static int ReadParams(Registers Registers, IMemory Memory) {
		return Memory.Read(Registers.PC);
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
		// Check for Flags.
		if (Registers.FL <= 0) {
			// Increment Program Counter.
			Registers.PC++;

			// Stop Execution of Instruction.
			return;
		}

		// Read Params.
		var Params = ReadParams(Registers, Memory);

		// Set Program Counter to Read Address.
		Registers.PC = (uint) (Registers.PC + Params);
	}
}

