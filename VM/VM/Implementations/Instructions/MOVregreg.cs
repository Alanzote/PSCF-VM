// Usings...
using VM.Interfaces;

// Namespaces...
namespace VM.Implementations.Instructions;

// The MOV Instruction.
public class MOVregreg : IInstruction {

	// The OpCode for this Instruction.
	public static int OpCode => 2;
	
	// The Name of this Instruction.
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
		return string.Format(Name, Registers.GetNameByRegCode(Params.Item1), Registers.GetNameByRegCode(Params.Item2));
	}

	// Executes the Instruction.
	public static void Execute(Registers Registers, IMemory Memory, IO IO) {
		// Rad Parameters.
		var Params = ReadParams(Registers, Memory);

		// Set Reg02 Value to Reg01 Value.
		Registers.SetByRegCode(Params.Item1, Registers.GetByRegCode(Params.Item2));

		// Increment PC.
		Registers.PC += 2;
	}
}

