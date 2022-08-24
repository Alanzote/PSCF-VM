// Usings...
using VM.Interfaces;

// Namespace...
namespace VM.Implementations.Instructions;

// The ADD Instruction.
public class ADD : IInstruction {

	// The OpCode.
	public static int OpCode => 9;

	// The Name of the Instruction for formatting.
	private const string Name = "ADD {0}, {1}";

	// Reads the Paramters from Memory of this Instruction.
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

		// Add Reg02 to Reg01.
		Registers.SetByRegCode(Params.Item1, Registers.GetByRegCode(Params.Item1) + Registers.GetByRegCode(Params.Item2));

		// Add Program Counter.
		Registers.PC += 2;
	}
}

