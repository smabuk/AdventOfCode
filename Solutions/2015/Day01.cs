namespace AdventOfCode.Solutions.Year2015;

/// <summary>
/// Day 1: Not Quite Lisp
/// https://adventofcode.com/2015/day/1
/// </summary>
[Description("Not Quite Lisp")]
public class Day01 {
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		string instructions = input[0];
		return instructions.Count(i => i == '(') - instructions.Count(i => i == ')');
	}

	private static int Solution2(string[] input) {
		string instructions = input[0];
		int charPos = 1;
		int floor = 0;
		foreach (char item in instructions) {
			floor += item switch {
				'(' => 1,
				')' => -1,
				_ => 0
			};
			if (floor == -1) {
				break;
			}
			charPos++;
		}
		return charPos;
	}
}
