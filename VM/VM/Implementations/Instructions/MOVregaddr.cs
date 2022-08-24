﻿// Usings...
using VM.Interfaces;

// Namespaces...
namespace VM.Implementations.Instructions;

// The Mov Reg, Addr Instruction.
public class MOVregaddr : IInstruction {

	// The OpCode of the Instruction.
	public static int OpCode => 3;

	// The Name of the Instruction.
	public static string Name => "MOV {0}, {1}";

	// Reads the Paramters from Memory of this Instruction.
	private static Tuple<int, uint> ReadParams(Registers Registers, IMemory Memory) {
		return Tuple.Create(Memory.Read(Registers.PC), (uint) Memory.Read(Registers.PC + 1));
	}

	// Gets the Instruction name, formatted.
	public static string GetFormattedName(Registers Registers, IMemory Memory) {
		// Read Parameters.
		var Params = ReadParams(Registers, Memory);

		// Return Formatted Name.
		return string.Format(Name, Registers.GetNameByRegCode(Params.Item1), $"[{Params.Item2}]");
	}

	// Executes the Instruction.
	public static void Execute(Registers Registers, IMemory Memory, IO IO) {
		// Read Params.
		var Params = ReadParams(Registers, Memory);

		// Set Reg to Address Value.
		Registers.SetByRegCode(Params.Item1, Memory.Read(Params.Item2));

		// Increment PC.
		Registers.PC += 2;
	}
}

