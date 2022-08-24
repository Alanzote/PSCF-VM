using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

// Check for Args.
if (args.Length != 2) {
	// Write Line.
	Console.WriteLine("Instructions Unclear, usage: VMC <*.casm> <*.run>");

	// Return.
	return;
}

// Paths for Op and Reg Cods.
const string OpCodePath = "./Assets/OPCODES.json";
const string RegCodePath = "./Assets/REGCODES.json";

// Make sure Op Codes Exists.
if (!File.Exists(OpCodePath)) {
	// Write Line.
	Console.WriteLine("OPCODES.json does not exist in Assets directory.");

	// Return.
	return;
}

// Make sure Reg Codes Exists.
if (!File.Exists(RegCodePath)) {
	// Write Line.
	Console.WriteLine("REGCODES.json does not exist in Assets directory.");

	// Return.
	return;
}

// Load OpCodes and RegCodes.
Dictionary<string, int> OpCodes = JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText("./Assets/OPCODES.json"))!;
Dictionary<string, int> RegCodes = JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText("./Assets/REGCODES.json"))!;

// Get Source and Destination.
var Source = args[0];
var Destination = args[1];

// Make sure Source Exits.
if (!File.Exists(Source)) {
	// Write Line.
	Console.WriteLine("Source file does not exist.");

	// Return.
	return;
}

// Delete Destination if it Exists.
if (File.Exists(Destination))
	File.Delete(Destination);

// Create Destination Writer.
var Writer = new BinaryWriter(File.OpenWrite(Destination));

// Read source File by Line.
var SrcLines = File.ReadAllLines(Source)
	.Where(x => !string.IsNullOrEmpty(x))
	.ToArray();

// Reference to all Labels.
Dictionary<string, uint> Labels = new Dictionary<string, uint>();

// Reference to all Jumps not Yet Resolved.
Dictionary<long, Tuple<string, bool>> NRJumps = new Dictionary<long, Tuple<string, bool>>();

// Converts a Value to a Type.
string ConvertValueToType(string Value) {
	// If it is a Reg Code, it is a Register.
	if (RegCodes.ContainsKey(Value))
		return "R";

	// If it is between [] and is a Number, it is a Memory Address.
	if (Regex.IsMatch(Value, @"\[\d+\]"))
		return "M";

	// If it is between [] and it is Not a number (all characters), it is a EReg.
	if (Regex.IsMatch(Value, @"\[[^\d\W]+\]"))
		return "A";

	// If it is a Positive or Negative Number, it is an Immediate.
	if (Regex.IsMatch(Value, @"-?\d+"))
		return "I";

	// If it just a string of characters, it is a Label.
	if (Regex.IsMatch(Value, @"[^\d\W]+"))
		return "L";

	// Otherwise, we say we don't know this pattern.
	throw new InvalidOperationException($"Pattern '{Value}' is not a valid Pattern");
}

// For Each Line...
for (int i = 0; i < SrcLines.Length; i++) {
	// Gets the Line, split.
	var SpltLine = SrcLines[i].Split(';') // By Comments.
		.Take(1) // Only Take what is NOT a Comment.
		.SelectMany(x => x.Split(' ')) // Split by Spaces.
		.SelectMany(x => x.Split(',')) // Split again by commas.
		.Where(x => !string.IsNullOrEmpty(x)) // No Empty Values.
		.Select(x => x.Replace(",", string.Empty)) // Removed all ,
		.Select(x => x.ToUpper()) // Make sure it is all UpperCase.
		.ToArray();

	// Check for Labels...
	if (Regex.IsMatch(SpltLine[0], @"[^\d\W]+:")) {
		// Get Position for Jump.
		uint JmpAddress = (uint) (Writer.BaseStream.Position / 4);

		// Add to Labels.
		Labels.Add(SpltLine[0].Split(':').First(), JmpAddress);

		// Go to Next Instruction.
		continue;
	}

	// Special parsing for MOV.
	if (SpltLine[0] == "MOV") {
		// Check for Params.
		if (SpltLine.Length != 3) {
			// Notify.
			Console.WriteLine($"MOV at Line {i + 1} does not have the correct number of Arguments (2).");

			return;
		}

		// Try to...
		try {
			// Convert Values to Type...
			var T1 = ConvertValueToType(SpltLine[1]);
			var T2 = ConvertValueToType(SpltLine[2]);

			// Add to Splt Line.
			SpltLine[0] += $"_{T1}{T2}";
		}
		catch (Exception Ex) {
			// Notify...
			Console.WriteLine($"{Ex.Message} at Line {i + 1}.");

			// Stop runing.
			return;
		}
	} else if (SpltLine[0].StartsWith('J')) { // Check for Jumps.
		// Check for Params.
		if (SpltLine.Length != 2) {
			// Notify.
			Console.WriteLine($"{SpltLine[0]} at Line {i + 1} does not have the correct number of Arguments (1).");

			// Return.
			return;
		}

		// Convert Values to Type...
		var T1 = ConvertValueToType(SpltLine[1]);

		// If Label add to List of Not Resolved Jumps.
		if (T1 == "L")
			NRJumps.Add(Writer.BaseStream.Position + 4, Tuple.Create(SpltLine[1], SpltLine[0] == "JMP"));
	}

	// Get Instruction.
	if (!OpCodes.TryGetValue(SpltLine[0], out int OpCode)) {
		// Notify.
		Console.WriteLine($"OpCode {SpltLine[0]} at Line {i + 1} not recognized.");

		// Return.
		return;
	}

	// Write OpCode.
	Writer.Write(OpCode);

	// For Each other Instruction...
	for (int j = 1; j < SpltLine.Length; j++) {
		// Attempt to...
		try {
			// Get argument Type.
			var Type = ConvertValueToType(SpltLine[j]);

			// Switch Type...
			switch (Type) {
				// Check for Register.
				case "R": {
					// Write the Register Code.
					Writer.Write(RegCodes[SpltLine[j]]);
				} break;

				// Check for EReg.
				case "A": {
					// Remove [].
					string FilterReg = SpltLine[j].Replace("[", "").Replace("]", "");

					// Write the Register Code.
					Writer.Write(RegCodes[FilterReg]);
				} break;

				// Check for Memory Address.
				case "M": {
					// Remove [].
					string FilterNumber = SpltLine[j].Replace("[", "").Replace("]", "");

					// Attempt to Convert to Address.
					if (!uint.TryParse(FilterNumber, out uint Value)) {
						// Notify.
						Console.WriteLine($"Can't Convert '{FilterNumber}' to Address Value at Line {i + 1}.");

						// Return.
						return;
					}

					// Otherwise, write it.
					Writer.Write(Value);
				} break;

				// Check Immediate Value.
				case "I": {
					// Attempt to Convert to Immediate.
					if (!int.TryParse(SpltLine[j], out int Value)) {
						// Notify.
						Console.WriteLine($"Can't Convert '{SpltLine[j]}' to Immediate Value at Line {i + 1}.");

						// Return.
						return;
					}

					// Otherwise, write it.
					Writer.Write(Value);
				} break;

				// Check for Label...
				case "L": {
					// We just write a placeholder int for later.
					Writer.Write(0);
				} break;

				// Default...
				default: {
					// Notify...
					Console.WriteLine($"Invalid Parameter Type at Line {i + 1}.");

					// Return.
					return;
				}
			}
		} catch (Exception Ex) {
			// Notify...
			Console.WriteLine($"{Ex.Message} at Line {i + 1}.");

			// Stop running...
			return;
		}
	}
}

// For Each NR Jump...
foreach (var Jump in NRJumps) {
	// Go to Position.
	Writer.BaseStream.Position = Jump.Key;

	// The Value we will rewrite.
	uint Value = 0;

	// If our Instruction is JMP, this means we are a fixed memory index.
	// Otherwise, we are an Offset and need to be converted.
	if (Jump.Value.Item2)
		Value = Labels[Jump.Value.Item1];
	else
		Value = (uint) (Labels[Jump.Value.Item1] - (Writer.BaseStream.Position / 4));

	// Rewrite the Value.
	Writer.Write(Value);
}

// Close Writer.
Writer.Close();

// Process Finished, Notify.
Console.WriteLine($"Finished compiling {Source} to {Destination}.");