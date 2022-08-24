// Usings...
using VM.Interfaces;

// Namespaces...
namespace VM.Implementations.Instructions;

// The Mov EReg, Reg Instruction.
public class MOVeregreg : IInstruction {
	// The OpCode of the Instruction.
	public static int OpCode => 7;

	// The Name of the Instruction.
	private const string Name = "MOV {0}, {1}";

	// Reads the Paramters from Memory of this Instruction.
	private static Tuple<int, int> ReadParams(Registers Registers, IMemory Memory) {
		return Tuple.Create(Memory.Read(Registers.PC), Memory.Read(Registers.PC + 1));
	}

	// Gets the Instruction name, formatted.
	public static string GetFormattedName(Registers Registers, IMemory Memory) {
		// Read Parameters.
		var Params = ReadParams(Registers, Memory);

		// Return Formatted Name.
		return string.Format(Name, $"[{Registers.GetNameByRegCode(Params.Item1)}]", Registers.GetNameByRegCode(Params.Item2));
	}

	// Executes the Instruction.
	public static void Execute(Registers Registers, IMemory Memory, IO IO) {
		// Read Params.
		var Params = ReadParams(Registers, Memory);

		// EReg means that REG holds an Address, so we copy value from Reg02 to the Memory Address at Reg01.
		Memory.Write((uint) Registers.GetByRegCode(Params.Item1), Registers.GetByRegCode(Params.Item2));

		// Increment PC.
		Registers.PC += 2;
	}
}

