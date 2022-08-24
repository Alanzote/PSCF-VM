// Usings...
using System;

// Namespace...
namespace VM;

// The Registers of this CPU.
public class Registers {

	// Basic Registers.
	public int AX;
	public int BX;
	public int CX;
	public int DX;

	// Program Counter.
	public uint PC;

	// Flags Registry.
	public int FL;

	// Sets Basic Registers by RegCode.
	public void SetByRegCode(int RegCode, int Value) {
		// Switch Reg Code.
		switch (RegCode) {
			case 0: AX = Value; break;
			case 1: BX = Value; break;
			case 2: CX = Value; break;
			case 3: DX = Value; break;
			default: throw new InvalidOperationException($"Invalid RegCode {RegCode}.");
		}
	}

	// Gets Basic Registers by RegCode.
	public int GetByRegCode(int RegCode) {
		// Return Switch by Reg Code.
		return RegCode switch {
			0 => AX,
			1 => BX,
			2 => CX,
			3 => DX,
			_ => throw new InvalidOperationException($"Invalid RegCode {RegCode}.")
		};
	}

	// Gets the Register Name by RegCode.
	public string GetNameByRegCode(int RegCode) {
		// Switch Reg Code.
		return RegCode switch {
			0 => "AX",
			1 => "BX",
			2 => "CX",
			3 => "DX",
			_ => throw new InvalidOperationException($"Invalid RegCode {RegCode}.")
		};
	}
}

