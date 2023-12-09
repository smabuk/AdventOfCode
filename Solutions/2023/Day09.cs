using Sequence   = System.Collections.Generic.List<int>;

namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 09: Mirage Maintenance
/// https://adventofcode.com/2023/day/09
/// </summary>
[Description("Mirage Maintenance")]
public sealed partial class Day09 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadHistories(input);
	public static string Part1(string[] input, params object[]? args) => Solution1().ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2().ToString();

	private static IEnumerable<Sequence> _histories = [];

	private static void LoadHistories(string[] input)
		=> _histories = [.. input.Select(i => i.As<int>(' ').ToList())];

	private static int Solution1()
		=> _histories.Sum(history => history.Extrapolate().Select(x => x.Last()).Aggregate((a, b) => a + b));

	private static int Solution2()
		=> _histories.Sum(history => history.Extrapolate().Select(x => x.First()).Reverse().Aggregate((a, b) => b - a));
}

public static class Day09Helpers
{
	public static IEnumerable<Sequence> Extrapolate(this Sequence history)
	{
		Sequence differences = [.. history];
		yield return history;
		do {
			differences = [.. differences.Zip(differences.Skip(1)).Select(x => x.Second - x.First)];;
			yield return differences;
		} while (NotAllZeroes(differences));
	}
	
	private static readonly Predicate<IEnumerable<int>> NotAllZeroes = (IEnumerable<int> sequence) => (!sequence.All(x => x == 0));
}
