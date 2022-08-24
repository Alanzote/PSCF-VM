// Usings...
using VM.Interfaces;

// Namespaces...
namespace VM.Implementations.Instructions;

// The Mov Addr, Immediate Instruction.
public class MOVaddrim : IInstruction {
	// The OpCode of the Instruction.
	public static int OpCode => 6;

	// The Name of the Instruction.
	private const string Name = "MOV {0}, {1}";

	// Reads the Paramters from Memory of this Instruction.
	private static Tuple<uint, int> ReadParams(Registers Registers, IMemory Memory) {
		return Tuple.Create((uint) Memory.Read(Registers.PC), Memory.Read(Registers.PC + 1));
	}

	// Gets the Instruction name, formatted.
	public static string GetFormattedName(Registers Registers, IMemory Memory) {
		// Read Parameters.
		var Params = ReadParams(Registers, Memory);

		// Return Formatted Name.
		return string.Format(Name, $"[{Params.Item1}]", Params.Item2);
	}

	// Executes the Instruction.
	public static void Execute(Registers Registers, IMemory Memory, IO IO) {
		// Read Params.
		var Params = ReadParams(Registers, Memory);

		// Write to Memory.
		Memory.Write(Params.Item1, Params.Item2);

		// Increment PC.
		Registers.PC += 2;
	}
}

