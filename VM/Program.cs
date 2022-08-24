// Usings...
using VM;
using VM.Interfaces;
using VM.Implementations.Memory;
using VM.Extensions;

// Check for Arguments.
if (args.Length != 2) {
	// Notify.
	Console.WriteLine("Please Specify a Program to Run and load location.");

	// Stop running.
	return;
}

// Create IO to Console.Out.
IO io = new IO(Console.Out);

// Create Memory, a 128 position cache L1, with a 16M memory.
IMemory mem = new Cache(128, new Cache(4, new RAM(16000000)));

// Create our CPU.
CPU cpu = new CPU(io, mem);

// Load Test Progam.
mem.LoadProgram(args[0], uint.Parse(args[1]));

// Interface for writing stuff into memory until we ask it to run.
Console.WriteLine("VM is Initialized, program is loaded.");
Console.WriteLine("Commands:");
Console.WriteLine(" - write <addr> <val> - Writes a Specific Value to an Address.");
Console.WriteLine(" - read <addr> - Reads the Value from an Address.");
Console.WriteLine(" - run <addr> - Runs the Program at an Address.");
Console.WriteLine(" - exit - Exists the VM.");

// Loop until...
while (true) {
	// Notify we want a command.
	Console.Write("Command: ");

	// Read the Line.
	string[]? Cmd = Console.ReadLine()?.ToUpper()?.Split(' ');

	// Ignore null commands.
	if (Cmd is null || Cmd.Length <= 0)
		continue;

	// Switch the command.
	switch (Cmd) {

		// Pattern Match WRITE.
		case ["WRITE", var Address, var Value]: {
			// Convert Address.
			if (!uint.TryParse(Address, out uint AddressCnv))
				continue;

			// Convert Value.
			if (!int.TryParse(Value, out int ValueCnv))
				continue;

			// Make sure Address is Valid.
			if (!mem.IsAddressValid(AddressCnv))
				continue;

			// Set Address.
			mem.Write(AddressCnv, ValueCnv);
		} break;

		// Pattern Match READ.
		case ["READ", var Address]: {
			// Convert Address.
			if (!uint.TryParse(Address, out uint AddressCnv))
				continue;

			// Make sure Address is Valid.
			if (!mem.IsAddressValid(AddressCnv))
				continue;

			// Read Address.
			Console.WriteLine(mem.Read(AddressCnv));
		} break;

		// Pattern Batch RUN.
		case ["RUN", var Address]: {
			// Convert Address.
			if (!uint.TryParse(Address, out uint AddressCnv))
				continue;

			// Make sure Address is Valid.
			if (!mem.IsAddressValid(AddressCnv))
				continue;

			// Run it.
			cpu.Run(AddressCnv);
		} break;

		// Pattern Match EXIT.
		case ["EXIT"]: return;

		// Command not found, ignore.
		default: continue;
	}
}