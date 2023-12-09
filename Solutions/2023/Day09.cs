using History    = System.Collections.Generic.IEnumerable<int>;
using Sequencies = System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<int>>;

namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 09: Mirage Maintenance
/// https://adventofcode.com/2023/day/09
/// </summary>
/// <remarks>
/// Using lambdas to practice.
/// Don't like how long the lines of code get, but it's elegant.
/// </remarks>
[Description("Mirage Maintenance")]
public sealed partial class Day09 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadHistories(input);
	public static string Part1(string[] input, params object[]? args) => Solution1().ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2().ToString();

	private static Sequencies _histories = [];

	private static void LoadHistories(string[] input)
		=> _histories = [.. input.Select(i => i.As<int>(' '))];

	private static int Solution1()
		=> _histories.Sum(history => Extrapolate(history,
				(Sequencies x) => x.Select(x => x.Last()).Aggregate((a, b) => a + b)
			));

	private static int Solution2()
		=> _histories.Sum(history=> Extrapolate(history,
				(Sequencies x) => x.Select(x => x.First()).Reverse().Aggregate((a, b) => b - a)
			));

	private static int Extrapolate(History history, Func<Sequencies, int> extrapolate)
	{
		List<List<int>> sequences = [[.. history]];

		List<int> differences = [];
		do {
			List<int> sequence = sequences.Last();
			differences = [];
			for (int i = 0; i < sequence.Count - 1; i++) {
				differences.Add(sequence[i + 1] - sequence[i]);
			}
			sequences.Add(differences);
		} while (differences.Any(x => x != 0));

		return extrapolate(sequences);
	}
}
