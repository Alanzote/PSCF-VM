// Usings...
using VM.Interfaces;

// Namespace...
namespace VM.Implementations.Instructions;

// The CMP Instruction.
public class CMP : IInstruction {
	// The OpCode.
	public static int OpCode => 15;

	// The Name of the Instruction.
	private const string Name = "CMP {0}, {1}";

	// Reads the Parameters from Memory of this Instruction.
	private static Tuple<int, int> ReadParams(Registers Registers, IMemory Memory) {
		return Tuple.Create(Memory.Read(Registers.PC), Memory.Read(Registers.PC + 1));
	}

	// Gets the Instruction name, formatted.
	public static string GetFormattedName(Registers Registers, IMemory Memory) {
		// Read Parameters.
		var Params = ReadParams(Registers, Memory);

		// Return Formatted Name.
		return string.Format(Name, Registers.GetNameByRegCode(Params.Item1), Registers.GetNameByRegCode(Params.Item2));
	}

	// Executes the Instruction.
	public static void Execute(Registers Registers, IMemory Memory, IO IO) {
		// Read Parameters.
		var Params = ReadParams(Registers, Memory);

		// Set Flag as Subtraction of both Registers.
		Registers.FL = Registers.GetByRegCode(Params.Item1) - Registers.GetByRegCode(Params.Item2);

		// Add Program Counter.
		Registers.PC += 2;
	}
}

