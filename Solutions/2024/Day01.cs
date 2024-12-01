using static AdventOfCode.Solutions._2024.Day01Constants;
namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 01: Historian Hysteria
/// https://adventofcode.com/2024/day/01
/// </summary>
[Description("Historian Hysteria")]
public sealed partial class Day01 {

	public static string Part1(string[] _) => Solution1().ToString();
	public static string Part2(string[] _) => Solution2().ToString();

	private static List<int> _left  = [];
	private static List<int> _right = [];

	[Init]
	public static void LoadLists(string[] input)
	{
		_left  = [.. input.GetList(LEFT)];
		_right = [.. input.GetList(RIGHT)];
	}

	private static int Solution1() => _left.TotalDistance(_right);
	private static int Solution2() => _left.TotalSimilarityScore(_right);
}

file static class Day01Extensions
{
	public static IEnumerable<int> GetList(this string[] input, int index)
		=> [.. input.Select(i => i.TrimmedSplit().As<int>().Skip(index).First())];

	public static int Distance(this int first, int second) => int.Abs(first - second);

	public static int TotalDistance(this IEnumerable<int> list1, IEnumerable<int> list2)
		=> list1.Order()
			.Zip(list2.Order(), Distance)
			.Sum();

	public static int SimilarityScore(this int number, Dictionary<int, int> counts)
		=> number * counts.GetValueOrDefault(number, 0);

	public static int TotalSimilarityScore(this IEnumerable<int> list1, IEnumerable<int> list2)
	{
		Dictionary<int, int> counts = list2.CountBy(n => n).ToDictionary();
		return list1.Sum(number => number.SimilarityScore(counts));
	}
}

file static class Day01Constants
{
	public const int  LEFT  = 0;
	public const int  RIGHT = 1;
}
