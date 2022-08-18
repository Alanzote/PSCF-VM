// Usings...
using VM;
using VM.Interfaces;
using VM.Implementations.Memory;

// Create IO to Console.Out.
IO io = new IO(Console.Out);

// Create Memory, a 4 position cache with a 128 position RAM.
IMemory mem = new Cache(4, new RAM(128));

// Create our CPU.
CPU cpu = new CPU(io, mem);

// Write the Program.
mem.Write(10, 120);
mem.Write(11, 125);

// Run program at address 10.
cpu.Run(10);