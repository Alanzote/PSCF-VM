// Namespace...
namespace VM;

// IO class, handles input/output.
public class IO {

	// Our Text Writer for Output.
	private readonly TextWriter Writer;

	// Constructor.
	public IO(TextWriter Writer) {
		// Set Writer.
		this.Writer = Writer;
	}

	// Outputs something to the Writer.
	public void Output(string msg) {
		// Write the message.
		Writer.Write(msg);
	}
}

