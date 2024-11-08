using static AdventOfCode.Solutions._2017.Day04Constants;
using static AdventOfCode.Solutions._2017.Day04Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 04: Title
/// https://adventofcode.com/2016/day/04
/// </summary>
[Description("")]
public sealed partial class Day04 {

	public static string Part1(string[] input) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		int validCount = 0;

		foreach (string passphrase in input) {
			string[] words = passphrase.TrimmedSplit(SPACE);
			if (words.Length == words.Distinct().Count()) {
				validCount += 1;
			}
		}

		return validCount;
	}

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class Day04Extensions
{
}

internal sealed partial class Day04Types
{
}

file static class Day04Constants
{
	public const char SPACE = ' ';
}
