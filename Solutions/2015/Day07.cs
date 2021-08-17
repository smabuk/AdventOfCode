namespace AdventOfCode.Solutions.Year2015;

/// <summary>
/// Day 07: Some Assembly Required
/// https://adventofcode.com/2015/day/7
/// </summary>
public class Day07 {

	record Wire(string Identifier) {
		public ushort? Value { get; set; } = null;
		public List<object> Ins { get; set; } = new();
		public List<object> Outs { get; set; } = new();

		private int myVar;

		internal ushort GetOutputValue() => throw new NotImplementedException();

		public ushort GetOutputValue1() {
			if (Value is not null) {
				return (ushort)Value;
			}
			return 0;
		}


	};
	class Gate {
		public Gate() {

		}
		public Gate(string gateType, string outputWire) {
			if (string.IsNullOrEmpty(gateType)) {
				throw new ArgumentException($"'{nameof(gateType)}' cannot be null or empty", nameof(gateType));
			}

			if (string.IsNullOrEmpty(outputWire)) {
				throw new ArgumentException($"'{nameof(outputWire)}' cannot be null or empty", nameof(outputWire));
			}

			GateType = gateType;
			OutputWire = outputWire;
		}
		public Gate(string gateType, string outputWire, ushort value) {
			GateType = gateType ?? throw new ArgumentNullException(nameof(gateType));
			OutputWire = outputWire ?? throw new ArgumentNullException(nameof(outputWire));
			Value = value;
		}
		public string GateType { get; set; }
		public string OutputWire { get; set; }
		public string? InputWire1 { get; set; } = null;
		public string? InputWire2 { get; set; } = null;
		public ushort? Value { get; set; } = null;

		public ushort OutputValue() {
			switch (GateType) {
				case "NUMBER":
					return (ushort?)Value ?? 0;
				case "OR":
				//return (ushort)(InputWire1 | InputWire2);
				default:
					break;
			}
			return 0;
		}
	}
	//record ShiftGate(string GateType, Wire Output, ushort ShiftAmount) : Gate(GateType, Output);
	//record NumberGate(string GateType, Wire Output, ushort Number) : Gate(GateType, Output);
	//record Gate(string GateType, Wire Output) {
	//	public List<object> Ins { get; set; } = new();
	//	public List<object> Outs { get; set; } = new();

	//	public ushort GetOutputValue1() {
	//		switch (GateType) {
	//			case "NUMBER":
	//				return (NumberGate)this. ;
	//			default:
	//				break;
	//		}
	//		foreach (Gate gate in Ins) {


	//		}
	//	}

	//};


	record Instruction(string OutWire);
	record NOT_Instruction(string OutWire, string Operand, string InWire) : Instruction(OutWire);
	record SHIFT_Instruction(string OutWire, string Operand
		, string InWire1, ushort ShiftAmount) : Instruction(OutWire);
	record LOGICAL_Instruction(string OutWire, string Operand, string InWire1, string InWire2) : Instruction(OutWire);
	record NUMBER_Instruction(string OutWire, ushort InputValue) : Instruction(OutWire) {
		public string Operand { get; set; } = "NUMBER";
	};

	//record Instruction(Wire OutWire);
	//record NOT_Instruction(Wire OutWire, Wire InWire) : Instruction(OutWire);
	//record SHIFT_Instruction(Wire OutWire, string Direction, Wire InWire1, UInt16 ShiftAmount) : Instruction(OutWire);
	//record LOGICAL_Instruction(Wire OutWire, string Operand, Wire InWire1, Wire InWire2) : Instruction(OutWire);
	//record NUMBER_Instruction(Wire OutWire, UInt16 InputValue) : Instruction(OutWire);


	private static ushort Solution1(string[] input, string start) {
		List<string> inputs = input.ToList();
		List<Instruction> instructions = input.Select(x => ParseLine(x)).ToList();
		Dictionary<string, Wire> wireInputs = new();
		List<Gate> gates = new();

		//foreach ( NUMBER_Instruction instruction in instructions.Where(i => i is NUMBER_Instruction)) {
		foreach (Instruction instruction in instructions) {
			Wire outWire = new(instruction.OutWire);
			if (wireInputs.ContainsKey(outWire.Identifier)) {
				wireInputs[outWire.Identifier].Ins.Add(outWire);
			} else {
				wireInputs.Add(outWire.Identifier, outWire);
			}
			if (instruction is NUMBER_Instruction numI) {
				wireInputs[numI.OutWire].Value = numI.InputValue;
				Gate gate = new() {
					GateType = numI.Operand,
					OutputWire = numI.OutWire,
					Value = numI.InputValue
				};
				gates.Add(gate);
			}
			if (instruction is NOT_Instruction notI) {
				Gate gate = new() {
					GateType = notI.Operand,
					OutputWire = notI.OutWire,
					InputWire1 = notI.InWire
				};
				gates.Add(gate);
			}
			if (instruction is LOGICAL_Instruction logI) {
				Gate gate = new() {
					GateType = logI.Operand,
					OutputWire = logI.OutWire,
					InputWire1 = logI.InWire1,
					InputWire2 = logI.InWire2
				};
				gates.Add(gate);
			}
			if (instruction is SHIFT_Instruction shiftI) {
				Gate gate = new() {
					GateType = shiftI.Operand,
					OutputWire = shiftI.OutWire,
					InputWire1 = shiftI.InWire1,
					Value = shiftI.ShiftAmount
				};
				gates.Add(gate);
			}
		}
		//var x = instructions.Where(i => i is NUMBER_Instruction)
		//	.Cast<NUMBER_Instruction>()
		//	.Select(i => new Wire(i.OutWire, i.InputValue))
		//	.;


		ushort result = GetOutputValue(start, gates);
		return result;
	}

	private static ushort GetOutputValue(string start, List<Gate> gates) {
		ushort result = 0;

		//var x = gates.Select(g => g.Outs.Where(x => x. == start);
		var x = gates.Where(g => g.OutputWire == start);



		return result;
	}

	private static string Solution2(string[] input) {
		List<string> inputs = input.ToList();
		throw new NotImplementedException();
	}

	private static Instruction ParseLine(string input) {
		string[] parts = input.Split(" -> ");
		string outWire = parts[^1];
		if (char.IsDigit(input[0])) {
			NUMBER_Instruction instruction = new(outWire, ushort.Parse(parts[0]));
			return instruction;
		}
		if (input.StartsWith("NOT")) {
			string[] moreParts = parts[0].Split(" ");
			NOT_Instruction instruction = new(outWire, "NOT", moreParts[1]);
			return instruction;
		}
		if (input.Contains("SHIFT")) {
			string[] moreParts = parts[0].Split(" ");
			SHIFT_Instruction instruction = new(outWire, moreParts[1], moreParts[0], ushort.Parse(moreParts[2]));
			return instruction;
		}
		if (parts.Length > 0) {
			string[] moreParts = parts[0].Split(" ");
			LOGICAL_Instruction instruction = new(outWire, moreParts[1], moreParts[0], moreParts[2]);
			return instruction;
		}
		return null!;
	}
	//private static Instruction ParseLine(string input) {
	//	string[] parts = input.Split(" -> ");
	//	string outWire = parts[^1];
	//	if (char.IsDigit(input[0])) {
	//		NUMBER_Instruction instruction = new(outWire, ushort.Parse(parts[0]));
	//		return instruction;
	//	}
	//	if (input.StartsWith("NOT")) {
	//		string[] moreParts = parts[0].Split(" ");
	//		NOT_Instruction instruction = new(outWire, "NOT", moreParts[1]);
	//		return instruction;
	//	}
	//	if (input.Contains("SHIFT")) {
	//		string[] moreParts = parts[0].Split(" ");
	//		SHIFT_Instruction instruction = new(outWire, moreParts[1], moreParts[0], ushort.Parse(moreParts[2]));
	//		return instruction;
	//	}
	//	if (parts.Length > 0) {
	//		string[] moreParts = parts[0].Split(" ");
	//		LOGICAL_Instruction instruction = new(outWire, moreParts[1], moreParts[0],moreParts[2]);
	//		return instruction;
	//	}
	//	return null!;
	//}

	public static string Part1(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		string start = args?[0].ToString() ?? "a";
		return Solution1(input, start).ToString();
	}
	public static string Part2(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		string start = args?[0].ToString() ?? "a";
		return Solution2(input).ToString();
	}
}
