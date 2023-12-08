namespace AdventOfCode.Solutions._2018;

/// <summary>
/// Day 01: Chronal Calibration
/// https://adventofcode.com/2018/day/01
/// </summary>
[Description("Chronal Calibration")]
public sealed partial class Day01 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		return input.As<int>().Sum();
	}

	private static int Solution2(string[] input) {

		int[] frequencyChanges = input.As<int>().ToArray();
		HashSet<int> results = [0];
		int index = 0;
		int result = 0;

		do {
			result += frequencyChanges[index];
			index = (index + 1) % frequencyChanges.Length;
		} while (results.Add(result));

		return result;
	}

}
