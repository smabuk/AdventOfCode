namespace AdventOfCode.Solutions.Year2015;

/// <summary>
/// Day 07: Some Assembly Required
/// https://adventofcode.com/2015/day/7
/// </summary>
[Description("Some Assembly Required")]
public class Day07 {

	public static string Part1(string[] input, params object[]? args) {
		string start = GetArgument(args, 1, "a");
		return Solution1(input, start).ToString();
	}
	public static string Part2(string[] input, params object[]? args) {
		string start = GetArgument(args, 1, "a");
		return Solution2(input, start).ToString();
	}


	record Instruction(string OutWire);
	record NOT_Instruction(string OutWire, string Operand, string InWire) : Instruction(OutWire);
	record SHIFT_Instruction(string OutWire, string Operand, string InWire1, ushort ShiftAmount) : Instruction(OutWire);
	record LOGICAL_Instruction(string OutWire, string Operand, string InWire1, string InWire2) : Instruction(OutWire);
	record NUMBER_Instruction(string OutWire, ushort InputValue, string Operand = "NUMBER") : Instruction(OutWire);
	record NOOP_Instruction(string OutWire, string InWire, string Operand = "NOOP") : Instruction(OutWire);

	private static ushort Solution1(string[] input, string start) {
		List<Instruction> instructions = input.Select(x => ParseLine(x)).ToList();
		Dictionary<string, ushort?> wireValues = instructions.ToDictionary(i => i.OutWire, i => (ushort?)null);

		return CalculateValue(start, instructions, wireValues);
	}

	private static ushort Solution2(string[] input, string start) {
		List<Instruction> instructions = input.Select(x => ParseLine(x)).ToList();
		Dictionary<string, ushort?> wireValues = instructions.ToDictionary(i => i.OutWire, i => (ushort?)null);

		ushort newB = CalculateValue(start, instructions, wireValues);
		instructions.RemoveAll(i => i.OutWire == "b");
		instructions = instructions.Prepend(new NUMBER_Instruction("b", newB)).ToList();

		wireValues = instructions.ToDictionary(i => i.OutWire, i => (ushort?)null);

		return CalculateValue(start, instructions, wireValues);
	}

	private static ushort CalculateValue(string wireId, List<Instruction> instructions, Dictionary<string, ushort?> wireValues) {
		if (Char.IsDigit(wireId[0])) {
			return ushort.Parse(wireId);
		}

		if (wireValues[wireId] is not null) {
			return wireValues[wireId] ?? 0;
		}

		Instruction instruction = instructions.Where(i => i.OutWire == wireId).Single();

		if (instruction is NUMBER_Instruction numI) {
			wireValues[wireId] = numI.InputValue;
			return numI.InputValue;
		}
		if (instruction is NOOP_Instruction noopI) {
			return CalculateValue(noopI.InWire, instructions, wireValues);
		}
		if (instruction is LOGICAL_Instruction logicI) {
			ushort v1 = CalculateValue(logicI.InWire1, instructions, wireValues);
			ushort v2 = CalculateValue(logicI.InWire2, instructions, wireValues);
			ushort value;
			value = logicI.Operand switch {
				"AND" => (ushort)(v1 & v2),
				"OR" => (ushort)(v1 | v2),
				_ => throw new NotImplementedException(),
			};
			wireValues[wireId] = value;
			return value;
		}
		if (instruction is SHIFT_Instruction shiftI) {
			ushort v1 = CalculateValue(shiftI.InWire1, instructions, wireValues);
			ushort value;
			value = shiftI.Operand switch {
				"LSHIFT" => (ushort)(v1 << shiftI.ShiftAmount),
				"RSHIFT" => (ushort)(v1 >> shiftI.ShiftAmount),
				_ => throw new NotImplementedException(),
			};
			wireValues[wireId] = value;
			return value;
		}
		if (instruction is NOT_Instruction notI) {
			ushort v1 = CalculateValue(notI.InWire, instructions, wireValues);
			ushort value = (ushort)~v1;
			wireValues[wireId] = value;
			return value;
		}

		throw new NotImplementedException();
	}

	private static Instruction ParseLine(string input) {
		Match match = Regex.Match(input, @"(?<wire1>\w+) (?<operand>AND|OR) (?<wire2>\w+) -> (?<out>\w+)");
		if (match.Success) {
			return new LOGICAL_Instruction(match.Groups["out"].Value, match.Groups["operand"].Value, match.Groups["wire1"].Value, match.Groups["wire2"].Value);
		}
		match = Regex.Match(input, @"(?<wire1>\w+) (?<operand>LSHIFT|RSHIFT) (?<amount>\d+) -> (?<out>\w+)");
		if (match.Success) {
			return new SHIFT_Instruction(match.Groups["out"].Value, match.Groups["operand"].Value, match.Groups["wire1"].Value, ushort.Parse(match.Groups["amount"].Value));
		}
		match = Regex.Match(input, @"(?<operand>NOT) (?<wire1>\w+) -> (?<out>\w+)");
		if (match.Success) {
			return new NOT_Instruction(match.Groups["out"].Value, match.Groups["operand"].Value, match.Groups["wire1"].Value);
		}
		match = Regex.Match(input, @"(?<value>\d+) -> (?<out>\w+)");
		if (match.Success) {
			return new NUMBER_Instruction(match.Groups["out"].Value, ushort.Parse(match.Groups["value"].Value));
		}
		match = Regex.Match(input, @"(?<wire1>\w+) -> (?<out>\w+)");
		if (match.Success) {
			return new NOOP_Instruction(match.Groups["out"].Value, match.Groups["wire1"].Value);
		}
		throw new Exception("Unrecognised input: {input}");
	}
}

