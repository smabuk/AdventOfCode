using static AdventOfCode.Solutions._2017.Day02Constants;
using static AdventOfCode.Solutions._2017.Day02Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 02: Corruption Checksum
/// https://adventofcode.com/2016/day/02
/// </summary>
[Description("Corruption Checksum")]
public sealed partial class Day02 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		return input
			.Select(i => i.TrimmedSplit([TAB, SPACE]).As<int>())
			.Sum(s => s.Max() - s.Min());
	}

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class Day02Extensions
{
}

internal sealed partial class Day02Types
{
}

file static class Day02Constants
{
	public const char TAB = '\t';
	public const char SPACE = ' ';
}
