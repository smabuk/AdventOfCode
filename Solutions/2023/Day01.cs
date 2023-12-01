namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 01: Trebuchet
/// https://adventofcode.com/2023/day/01
/// </summary>
[Description("Trebuchet")]
public sealed partial class Day01 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();


	private static int Solution1(string[] input) {
		return input.Select(i => ParseLine(i)).Sum();
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}

	private static int ParseLine(string input)
	{
		MatchCollection match = InputRegEx().Matches(input);
		return (int.Parse(match[0].Value) * 10) + int.Parse(match[^1].Value);
	}

	[GeneratedRegex("""(?<number>[\d])""")]
	private static partial Regex InputRegEx();
}
