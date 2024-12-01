using static AdventOfCode.Solutions._2024.Day01Constants;
using static AdventOfCode.Solutions._2024.Day01Types;
namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 01: Historian Hysteria
/// https://adventofcode.com/2024/day/01
/// </summary>
[Description("Historian Hysteria")]
public sealed partial class Day01 {

	[Init]
	public static void Init(string[] input) => LoadLists(input);
	public static string Part1(string[] _) => Solution1().ToString();
	public static string Part2(string[] _) => Solution2().ToString();

	private static (List<int> Left, List<int> Right) _lists = default;

	private static void LoadLists(string[] input)
	{
		_lists.Left  = [.. input.GetList(LEFT)];
		_lists.Right = [.. input.GetList(RIGHT)];
	}

	private static int Solution1() => _lists.TotalDistance();

	private static int Solution2()
	{
		return
			(_lists.Left, _lists.Right.CountBy(n => n).ToDictionary())
			.TotalSimilarityScore();
	}
}

file static class Day01Extensions
{
	public static IEnumerable<int> GetList(this string[] input, int index) =>
		[.. input.Select(i => i.TrimmedSplit(SPACE)[index].As<int>())];

	public static int Distance(this (int First, int Second) numbers) => int.Abs(numbers.First - numbers.Second);

	public static int TotalDistance(this (IEnumerable<int> List1, IEnumerable<int> List2) lists) =>
		lists.List1.Order()
		.Zip(lists.List2.Order())
		.Sum(Distance);

	public static int SimilarityScore(this int number, Dictionary<int, int> list2Counts) =>
		number * list2Counts.GetValueOrDefault(number, 0);

	public static int TotalSimilarityScore(this (IEnumerable<int> List1, Dictionary<int, int> List2Counts) lists) =>
		lists.List1
		.Sum(number => number.SimilarityScore(lists.List2Counts));
}

internal sealed partial class Day01Types
{
}

file static class Day01Constants
{
	public const char SPACE = ' ';

	public const int  LEFT  = 0;
	public const int  RIGHT = 1;
}
