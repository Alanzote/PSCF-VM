// Usings...
using VM.Interfaces;

// Namespaces...
namespace VM.Implementations.Instructions;

// The Mov Addr, Reg Instruction.
public class MOVaddrreg : IInstruction {
	// The OpCode of the Instruction.
	public static int OpCode => 4;

	// The Name of the Instruction.
	public static string Name => "MOV {0}, {1}";

	// Reads the Paramters from Memory of this Instruction.
	private static Tuple<uint, int> ReadParams(Registers Registers, IMemory Memory) {
		return Tuple.Create((uint)Memory.Read(Registers.PC), Memory.Read(Registers.PC + 1));
	}

	// Gets the Instruction name, formatted.
	public static string GetFormattedName(Registers Registers, IMemory Memory) {
		// Read Parameters.
		var Params = ReadParams(Registers, Memory);

		// Return Formatted Name.
		return string.Format(Name, $"[{Params.Item1}]", Registers.GetNameByRegCode(Params.Item2));
	}

	// Executes the Instruction.
	public static void Execute(Registers Registers, IMemory Memory, IO IO) {
		// Read Params.
		var Params = ReadParams(Registers, Memory);

		// Write to Memory the Value of the Registers.
		Memory.Write(Params.Item1, Registers.GetByRegCode(Params.Item2));

		// Increment PC.
		Registers.PC += 2;
	}
}

