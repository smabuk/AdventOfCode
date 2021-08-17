namespace AdventOfCode.Solutions.Year2015;

/// <summary>
/// Day 1: Not Quite Lisp
/// https://adventofcode.com/2015/day/1
/// </summary>
public class Day01 {
	public static string Part1(string[]? input) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Part1_Solution(input).ToString();
	}

	public static string Part2(string[]? input) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Part2_Solution(input).ToString();
	}



	private static int Part1_Solution(string[] input) {
		string instructions = input[0];
		return instructions.Count(i => i == '(') - instructions.Count(i => i == ')');
	}

	private static int Part2_Solution(string[] input) {
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
