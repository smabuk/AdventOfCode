using System.ComponentModel;

namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 01: Sonar Sweep
/// https://adventofcode.com/2021/day/1
/// </summary>
[Description("Sonar Sweep")]
public class Day01 {

	private static int Solution1(string[] input) {
		List<int> depths = input.ToList().Select(x => int.Parse(x)).ToList();

		return depths.Zip(depths.Skip(1), (d1, d2) => d2 - d1)
			.Count(increase => increase > 0);
	}

	private static int Solution2(string[] input) {
		int[] depths = input.ToList().Select(x => int.Parse(x)).ToArray();

		int current = depths[0..3].Sum();
		int count = 0;
		for (int i = 1; i < depths.Length - 2; i++) {
			int depth = depths[i..(i + 3)].Sum();
			if (depth > current) {
				count++;
			}
			current = depth;
		}
		return count;
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
