// Usings...
using VM.Interfaces;

// Namespace...
namespace VM.Implementations.Instructions;

// The OUT Instruction.
public class OUT : IInstruction {
	// The OpCode.
	public static int OpCode => 20;

	// The Name of the Instruction.
	private const string Name = "OUT {0}";

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
		// Get Params.
		var Params = ReadParams(Registers, Memory);

		// Output.
		IO.Output($"Output of Register {Registers.GetNameByRegCode(Params)}: {Registers.GetByRegCode(Params)}\n");

		// Increment PC.
		Registers.PC++;
	}
}

