// Usings...
using System.Reflection;
using VM.Exceptions;
using VM.Implementations.Instructions;
using VM.Interfaces;

// Namespace...
namespace VM;

// Our CPU Class.
public class CPU {

	// Our Registers.
	public Registers Registers = new Registers();

	// References to our IO and Memory.
	private readonly IO Io;
	private readonly IMemory Memory;

	// Our Instruction Set.
	private static IReadOnlyDictionary<int, Tuple<Type, MethodInfo, MethodInfo>>? Instructions;

	// Invokation Parameters for Execute and Name Methods.
	private readonly object[] ExecuteParams;
	private readonly object[] NameParams;

	// The Amount of NOPs until a mandatory break.
	private const uint NOPsBreak = 30;
	private uint CurNOPsBreak = 0;

	// If we should print each step while runing.
	public bool StepPrint = true;

	// Construtor.
	public CPU(IO Io, IMemory Memory) {
		// Sets
		this.Io = Io;
		this.Memory = Memory;

		// Create Execute Params.
		ExecuteParams = new object[] {
			this.Registers,
			this.Memory,
			this.Io
		};

		// Create Name Params.
		NameParams = new object[] {
			this.Registers,
			this.Memory
		};

		// Only prepare instructions if we haven't already.
		if (Instructions != null)
			return;

		// Gather all Instructions.
		var InstructionTypes = typeof(CPU).Assembly.GetTypes().Where(x => !x.IsInterface && x.IsAssignableTo(typeof(IInstruction)));

		// Create Temporary Dictionary.
		Dictionary<int, Tuple<Type, MethodInfo, MethodInfo>> InstructionDict = new Dictionary<int, Tuple<Type, MethodInfo, MethodInfo>>();

		// For each Instruction...
		foreach (var Instruction in InstructionTypes) {
			// Get OpCode.
			var OpCode = (int) Instruction.GetProperty("OpCode")!.GetValue(null)!;

			// Check if it doesn't already exist.
			if (InstructionDict.ContainsKey(OpCode))
				throw new InvalidOperationException($"Duplicate OpCode {OpCode}.");

			// Add to Dict.
			InstructionDict.Add(OpCode, Tuple.Create(Instruction, Instruction.GetMethod("Execute")!, Instruction.GetMethod("GetFormattedName")!));
		}

		// Set Statis Dictionary.
		Instructions = InstructionDict.AsReadOnly();
	}

	// Runs the Program at the Address.
	public TimeSpan Run(uint Address = 0) {
		// Time we Started Calculating.
		DateTime StartTime = DateTime.Now;

		// Set Program Counter.
		Registers.PC = Address;

		// Loop Forever...
		while (true) {
			// The Instruction OpCode.
			int OpCode = Memory.Read(Registers.PC);

			// Update Cur NOP Breaks.
			CurNOPsBreak = (OpCode == NOP.OpCode ? CurNOPsBreak + 1 : 0);

			// If we exceed it, we stop running.
			if (CurNOPsBreak > NOPsBreak)
				throw new Exception("Reached maximum amount of NOPs, breaking automatically to prevent infinite loop.");

			// Find the Instruction.
			if (!Instructions!.TryGetValue(OpCode, out var Instruction))
				throw new InvalidOperationException($"Instruction {Memory.Read(Registers.PC)} at {Registers.PC} not found.");

			// We increment the Program Counter so the Instruction can access data without incrementing too much.
			Registers.PC++;

			// Notify Instruction.
			if (StepPrint)
				Io.Output($"{Registers.PC - 1}	{Instruction.Item3.Invoke(null, NameParams)}\n");

			// Let's try to run the Instruction...
			try {
				// Run the Instruction.
				Instruction.Item2.Invoke(null, ExecuteParams);
			} catch (Exception Ex) {
				// This is a treated exception that requests simulation to be halted,
				// so, we stop running.
				if (Ex.InnerException?.GetType() == typeof(SimulationHaltException))
					break;
				else
					throw; // We throw it back again since it isn't an actual treated exception.
			}

			// No other exceptions are catched, as they are simulation errors
			// and those need to be showed to be fixed.
		}

		// When we finish, return timespan.
		return DateTime.Now - StartTime;
	}
}

