using static AdventOfCode.Solutions._2017.Day15Constants;
using static AdventOfCode.Solutions._2017.Day15Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 15: Dueling Generators
/// https://adventofcode.com/2017/day/15
/// </summary>
[Description("Dueling Generators")]
public sealed partial class Day15 {

	public static string Part1(string[] input) => Solution1(input).ToString();
	public static string Part2(string[] input) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		const int NO_OF_PAIRS = 40_000_000;

		int[] genStartValues = [.. input.Select(inp =>inp.TrimmedSplit(' ')[^1].As<int>())];

		return Enumerable.Range(0, NO_OF_PAIRS)
			.Zip(
				genStartValues[A].Generator(GENERATOR_A_FACTOR),
				genStartValues[B].Generator(GENERATOR_B_FACTOR))
			.Count(pair => (pair.Second & MASK_16) == (pair.Third & MASK_16));
	}

	private static int Solution2(string[] input) {
		const int NO_OF_PAIRS = 5_000_000;

		int[] genStartValues = [.. input.Select(inp => inp.TrimmedSplit(' ')[^1].As<int>())];

		return Enumerable.Range(0, NO_OF_PAIRS)
			.Zip(
				genStartValues[A].Generator(GENERATOR_A_FACTOR, (v) => v % 4 == 0),
				genStartValues[B].Generator(GENERATOR_B_FACTOR, (v) => v % 8 == 0))
			.Count(pair => (pair.Second & MASK_16) == (pair.Third & MASK_16));
	}
}

file static class Day15Extensions
{
	public static IEnumerable<int> Generator(this int start, int factor, Predicate<int> predicate = null!)
	{
		int current = start;

		while (true) {
			current = (int)((long)current * factor % DIVISOR);
			if (predicate is null || predicate(current)) {
				yield return current;
			}
		}
	}
}

internal sealed partial class Day15Types
{
}

file static class Day15Constants
{
	public const int A = 0;
	public const int B = 1;

	public const int DIVISOR = 2147483647;

	public const int GENERATOR_A_FACTOR = 16807;
	public const int GENERATOR_B_FACTOR = 48271;

	public const uint MASK_16 = 0b1111111111111111;
}
