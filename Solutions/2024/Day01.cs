using static AdventOfCode.Solutions._2024.Day01Constants;
using static AdventOfCode.Solutions._2024.Day01Types;
namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 01: Historian Hysteria
/// https://adventofcode.com/2024/day/01
/// </summary>
[Description("Historian Hysteria")]
public sealed partial class Day01 {

	public static string Part1(string[] input) => Solution1(input).ToString();
	public static string Part2(string[] input) => Solution2(input).ToString();

	private static int Solution1(string[] input)
	{
		IEnumerable<int> list1 = [..input.Select(i => i.TrimmedSplit(SPACE)[0].As<int>()).Order()];
		IEnumerable<int> list2 = [..input.Select(i => i.TrimmedSplit(SPACE)[1].As<int>()).Order()];
		return list1
			.Zip(list2)
			.Sum(numbers => numbers.Distance());
	}

	private static int Solution2(string[] input)
	{
		List<int> list1 = [.. input.Select(i => i.TrimmedSplit(SPACE)[0].As<int>()).Order()];
		Dictionary<int, int> countsOfList2 = 
			input
				.Select(i => i.TrimmedSplit(SPACE)[1].As<int>())
				.CountBy(n => n)
				.ToDictionary();

		return list1
			.Sum(number => number.SimilarityScore(countsOfList2.GetValueOrDefault(number, 0)));
	}
}

file static class Day01Extensions
{
	public static int Distance(this (int First, int Second) numbers) => int.Abs(numbers.First - numbers.Second);
	public static int SimilarityScore(this int number, int count) => number * count;
}

internal sealed partial class Day01Types
{
}

file static class Day01Constants
{
	public const char SPACE = ' ';
}
