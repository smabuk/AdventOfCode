namespace AdventOfCode.Solutions.Year2015;

/// <summary>
/// Day 03: Perfectly Spherical Houses in a Vacuum
/// https://adventofcode.com/2015/day/3
/// </summary>
[Description("Perfectly Spherical Houses in a Vacuum")]
public class Day03 {
	private static int Solution1(string[] input) {
		List<(int x, int y)> houses = new();
		(int x, int y) santa = (0, 0);
		houses.Add(santa);
		foreach (char c in input[0]) {
			_ = c switch {
				'^' => santa.y++,
				'v' => santa.y--,
				'>' => santa.x++,
				'<' => santa.x--,
				_ => throw new NotImplementedException()
			};
			houses.Add(santa);
		}
		return houses.Distinct().Count();
	}

	private static int Solution2(string[] input) {
		List<(int x, int y)> houses = new();
		(int x, int y) santa = (0, 0);
		(int x, int y) roboSanta = (0, 0);
		houses.Add(santa);
		houses.Add(roboSanta);
		int count = 0;
		foreach (char c in input[0]) {
			bool santasTurn = (count++ % 2) == 0;
			_ = (santasTurn, c) switch {
				(true, '^') => santa.y++,
				(true, 'v') => santa.y--,
				(true, '>') => santa.x++,
				(true, '<') => santa.x--,
				(false, '^') => roboSanta.y++,
				(false, 'v') => roboSanta.y--,
				(false, '>') => roboSanta.x++,
				(false, '<') => roboSanta.x--,
				_ => throw new NotImplementedException()
			};
			if (santasTurn) {
				houses.Add(santa);
			} else {
				houses.Add(roboSanta);
			}
		}
		return houses.Distinct().Count();
	}

	public static string Part1(string[]? input) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution1(input).ToString();
	}

	public static string Part2(string[]? input) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution2(input).ToString();
	}

}
