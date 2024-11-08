using static AdventOfCode.Solutions._2017.Day05Constants;
using static AdventOfCode.Solutions._2017.Day05Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 05: A Maze of Twisty Trampolines, All Alike
/// https://adventofcode.com/2016/day/05
/// </summary>
[Description("A Maze of Twisty Trampolines, All Alike")]
public sealed partial class Day05 {

	public static string Part1(string[] input) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		int[] jumps = [.. input.As<int>()];
		int jumpPtr = 0;
		int steps = 0;

		while (jumpPtr >= 0 && jumpPtr < jumps.Length) {
			jumpPtr += jumps[jumpPtr]++;
			steps++;
		}

		return steps;
	}

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class Day05Extensions
{
}

internal sealed partial class Day05Types
{
}

file static class Day05Constants
{
}
