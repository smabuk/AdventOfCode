namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 06: Signals and Noise
/// https://adventofcode.com/2016/day/06
/// </summary>
[Description("Signals and Noise")]
public sealed partial class Day06 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static string Solution1(string[] input) {
		return new([..
			Enumerable.Range(0, input[0].Length)
			.Select(charIndex => 
				input
				.Select(msg => msg[charIndex])
				.CountBy(c => c)
				.MaxBy(kv => kv.Value)
				.Key)]
		);
	}

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}
