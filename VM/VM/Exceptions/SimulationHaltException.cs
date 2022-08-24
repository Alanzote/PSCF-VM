// Usings...
using System;

// Namespace...
namespace VM.Exceptions;

// Notifies Simulation has been Halted.
public class SimulationHaltException : Exception {

	// Constructor.
	public SimulationHaltException() : base("Simulation halted.") { }
}

