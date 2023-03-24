// Usings...
using VM;
using VM.Interfaces;
using VM.Implementations.Memory;
using VM.Implementations.Memory.Cache;
using VM.Extensions;
using Humanizer;

// Create IO to Console.Out.
IO io = new IO(Console.Out);

// Memory Reference.
IMemory mem;

// CPU Reference.
CPU cpu;

// Resets the CPU.
void Reset() {
	// Create memory, with a Direct Cache of 4096 positions with 64 lines, a total memory of 8 Megawords.
	mem = new DirectCache(4096, 64, new RAM(8388608));

	// Craete CPU.
	cpu = new CPU(io, mem);
}

// Call Reset at Start.
Reset();

// Displays Memory Information.
{
	// Interface for writing stuff into memory until we ask it to run.
	Console.WriteLine($"VM is Initialized with {mem.Size.Bytes().ToFullWords().Replace("byte", "word")} of Memory, being:");

	// Grab the Current Memory.
	IMemory? _curMem = mem;

	// Loop until we don't have any more memory.
	while (_curMem != null) {
		// Write memory information.
		Console.WriteLine($" - {_curMem.GetType().Name} - {_curMem.ActualSize.Bytes().ToFullWords().Replace("byte", "word")}.");

		// Grab parent memory.
		_curMem = _curMem.GetParent();
	}
}

// Prints the Help.
void PrintHelp() {
	// Print all commands.
	Console.WriteLine("Commands:");
	Console.WriteLine(" - help - Displays this page.");
	Console.WriteLine(" - load \"<program>\" <addr> - Loads a Program to an Address.");
	Console.WriteLine(" - reset - Fully Resets the VM.");
	Console.WriteLine(" - write <addr> <val> - Writes a Specific Value to an Address.");
	Console.WriteLine(" - read <addr> - Reads the Value from an Address.");
	Console.WriteLine(" - run <addr> - Runs the Program at an Address.");
	Console.WriteLine(" - stepprint - Toggles Step Print. (Prints steps, default: true)");
	Console.WriteLine(" - exit - Exists the VM.");
}

// Print Help first.
PrintHelp();

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

		// Pattern Match Help.
		case ["HELP"]: {
			// Print Help.
			PrintHelp();
		} break;

		// Pattern Match LOAD.
		case ["LOAD", var Program, var Address]: {
			// Check for Program Location.
			if (!File.Exists(Program))
				continue;

			// Check for Address Type.
			if (!uint.TryParse(Address, out uint AddressCnv))
				continue;

			// Make sure Address is Valid.
			if (!mem.IsAddressValid(AddressCnv))
				continue;

			// Load Program.
			mem.LoadProgram(Program, AddressCnv);

			// Notify.
			Console.WriteLine("Program Loaded.");
		} break;

		// Pattern Match RESET.
		case ["RESET"]: {
			// Call Reset.
			Reset();

			// Notify.
			Console.WriteLine("VM Reset.");
		} break;

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

			// Notify.
			Console.WriteLine("Value written to specified address.");
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

			// We attempt to...
			try {
				// Run it.
				var Span = cpu.Run(AddressCnv);

				// Notify how much it took.
				Console.WriteLine($"Run took {Span.Humanize()}.");
			} catch (Exception ex) {
				// In case of errors, we notify.
				Console.WriteLine(ex.Message);
			}
		} break;

		// Pattern Match Step Print.
		case ["STEPPRINT"]: {
			// Toggle Step Print.
			cpu.StepPrint = !cpu.StepPrint;

			// Notify.
			Console.WriteLine($"Step Print is now {(cpu.StepPrint ? "Enabled" : "Disabled")}.");
		} break;

		// Pattern Match EXIT.
		case ["EXIT"]: return;

		// Command not found, ignore.
		default: continue;
	}
}