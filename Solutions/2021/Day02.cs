namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 02: Dive!
/// https://adventofcode.com/2021/day/2
/// </summary>
public class Day02 {

	record Instruction(string Direction, int Value);
	record Position(int Horizontal, int Depth, int Aim = 0);

	private static int Solution1(string[] input) {
		List<Instruction> instructions = input.Select(i => ParseLine(i)).ToList();

		Position position = new(0, 0);
		foreach (Instruction instruction in instructions) {
			position = instruction.Direction switch {
				"forward" => position with { Horizontal = position.Horizontal + instruction.Value },
				"down" => position with { Depth = position.Depth + instruction.Value },
				"up" => position with { Depth = position.Depth - instruction.Value },
				_ => throw new NotImplementedException(),
			};
		}

		return position.Horizontal * position.Depth;
	}

	private static int Solution2(string[] input) {
		List<Instruction> instructions = input.Select(i => ParseLine(i)).ToList();

		Position position = new(0, 0, 0);
		foreach (Instruction instruction in instructions) {
			position = instruction.Direction switch {
				"forward" => position with { Horizontal = position.Horizontal + instruction.Value, Depth = position.Depth + (instruction.Value * position.Aim) },
				"down" => position with { Aim = position.Aim + instruction.Value },
				"up" => position with { Aim = position.Aim - instruction.Value },
				_ => throw new NotImplementedException(),
			};
		}

		return position.Horizontal * position.Depth;
	}

	private static Instruction ParseLine(string input) {
		Match match = Regex.Match(input, @"(forward|down|up) (\d+)");
		if (match.Success) {
			return new(match.Groups[1].Value, int.Parse(match.Groups[2].Value));
		}
		return null!;
	}





	#region Problem initialisation
	public static string Part1(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution1(input).ToString();
	}
	public static string Part2(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution2(input).ToString();
	}
	#endregion

}
