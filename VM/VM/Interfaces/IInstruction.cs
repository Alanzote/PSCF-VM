// Usings...
using System;

// Namespace...
namespace VM.Interfaces;

// Instruction Interface that Specifies an Instruction.
public interface IInstruction {

	// The OpCode for this Instruction.
	static abstract int OpCode { get; }
	
	// Gets the Name of the Instruction for Debugging.
	static abstract string GetFormattedName(Registers Registers, IMemory Memory);

	// Executes this Instruction.
	static abstract void Execute(Registers Registers, IMemory Memory, IO IO);
}

