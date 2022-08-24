// Usings...
using VM.Interfaces;

// Namespace...
namespace VM.Extensions;

// Extensions for IMemory.
public static class IMemoryExtensions {

	// Reads a Program to an Address.
	public static void LoadProgram(this IMemory Memory, string FileName, uint StartAddress = 0) {
		// Make sure file Exists.
		if (!File.Exists(FileName))
			throw new FileNotFoundException(FileName);

		// Load File.
		BinaryReader Reader = new BinaryReader(File.OpenRead(FileName));

		// Read Program til End.
		while (Reader.BaseStream.Position < Reader.BaseStream.Length)
			Memory.Write(StartAddress++, Reader.ReadInt32());

		// Close Reader.
		Reader.Close();
	}
}

