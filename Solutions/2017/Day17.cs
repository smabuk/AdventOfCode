using static AdventOfCode.Solutions._2017.Day17Constants;
using static AdventOfCode.Solutions._2017.Day17Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 17: Spinlock
/// https://adventofcode.com/2016/day/17
/// </summary>
[Description("Spinlock")]
public sealed partial class Day17 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		int steps = input[0].As<int>();

		List<int> buffer = [0];

		int currentIndex = 0;
		for (int i = 1; i <= NO_OF_INSERTIONS; i++) {
			currentIndex = ((currentIndex + steps) % buffer.Count) + 1;
			buffer.Insert(currentIndex, i);
		}

		return buffer[currentIndex + 1];
	}

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class Day17Extensions
{
}

internal sealed partial class Day17Types
{
}

file static class Day17Constants
{
	public const int NO_OF_INSERTIONS = 2017;
}
