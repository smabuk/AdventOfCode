namespace AdventOfCode.Solutions._2015;

/// <summary>
/// Day 25: Let It Snow
/// https://adventofcode.com/2015/day/25
/// </summary>
[Description("Let It Snow")]
public sealed partial class Day25 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	record Position(int Row, int Column);

	private static long Solution1(string[] input) {
		Position targetPosition = ParseLine(input[0]);
		long code = 20151125;

		for (int i = 2; i <= int.MaxValue; i++) {
			for (int row = i, col = 1; row > 0; row--, col++) {
				code = code * 252533 % 33554393;
				if (targetPosition == new Position(row, col)) {
					return code;
				}
			}
		}

		return 0;

	}

	private static string Solution2(string[] input) {
		return "** CONGRATULATIONS **";
	}

	private static Position ParseLine(string input) {
		Match match = PositionRegex().Match(input);
		if (match.Success) {
			return new(int.Parse(match.Groups["row"].Value), int.Parse(match.Groups["col"].Value));
		}
		return null!;
	}

	[GeneratedRegex("""To continue, please consult the code grid in the manual.  Enter the code at row (?<row>\d+), column (?<col>\d+).""")]
	private static partial Regex PositionRegex();
}
