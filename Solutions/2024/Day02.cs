using static AdventOfCode.Solutions._2024.Day02Constants;

namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 02: Red-Nosed Reports
/// https://adventofcode.com/2024/day/02
/// </summary>
[Description("Red-Nosed Reports")]
public sealed partial class Day02 {

	public static string Part1(string[] input) => Solution1(input).ToString();
	public static string Part2(string[] input) => Solution2(input).ToString();

	private static int Solution1(string[] input)
		=> input.GetReports().Count(r => r.IsSafe());

	private static int Solution2(string[] input)
		=> input.GetReports().Count(r => r.IsTolerablySafe());
}

file static class Day02Extensions
{
	public static IEnumerable<IEnumerable<int>> GetReports(this IEnumerable<string> input)
		=> input.Select(i => i.As<int>(SPACE));

	public static bool HasMinMaxGap(this (int First, int Second) pair)
		=> int.Abs(pair.First - pair.Second) is >= MIN_GAP and <= MAX_GAP;

	public static bool HasSign(this (int First, int Second) pair, int sign)
		=> int.Sign(pair.First - pair.Second) == sign;

	public static bool IsSafe(this IEnumerable<int> report)
	{
		int sign = int.Sign(report.First() - report.Skip(1).First());
		return report.Zip(report.Skip(1))
				.All(pair => pair.HasMinMaxGap() && pair.HasSign(sign));
	}

	public static bool IsTolerablySafe(this IEnumerable<int> report)
	{
		return Enumerable
			.Range(0, report.Count())
			.Any(i => report.Where((_, index) => index != i).IsSafe());
	}
}

file static class Day02Constants
{
	public const char SPACE = ' ';

	public const int MIN_GAP = 1;
	public const int MAX_GAP = 3;
}
