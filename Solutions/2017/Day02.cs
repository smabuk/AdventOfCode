using static AdventOfCode.Solutions._2017.Day02Constants;
using static AdventOfCode.Solutions._2017.Day02Extensions;
using static AdventOfCode.Solutions._2017.Day02Types;
using System.Linq;

namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 02: Corruption Checksum
/// https://adventofcode.com/2017/day/02
/// </summary>
[Description("Corruption Checksum")]
public sealed partial class Day02 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static int Solution1(string[] input) =>
		input
		.ToRows()
		.Sum(static s => s.Max() - s.Min());

	private static int Solution2(string[] input)
	{
		return input
			.ToRows()
			.Sum(CalculateRowSum);
	}
}

file static class Day02Extensions
{
	public static Func<IEnumerable<int>, int> CalculateRowSum
		= static s => s.Permute(2).Sum(static p => p.ValueIfDivisible());

	public static IEnumerable<IEnumerable<int>> ToRows(this IEnumerable<string> input) =>
		input.Select(static row => row.TrimmedSplit([TAB, SPACE]).As<int>());

	public static int ValueIfDivisible(this int[] pair) =>
		pair.First() % pair.Last() == 0 ? pair.First() / pair.Last() : 0;
}

internal sealed partial class Day02Types
{
}

file static class Day02Constants
{
	public const char TAB = '\t';
	public const char SPACE = ' ';
}
