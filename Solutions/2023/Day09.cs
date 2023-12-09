namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 09: Mirage Maintenance
/// https://adventofcode.com/2023/day/09
/// </summary>
[Description("Mirage Maintenance")]
public sealed partial class Day09 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static long Solution1(string[] input) {
		List<List<long>> histories = [];
		foreach (var line in input) {
			List<long> history = [.. line.As<long>(' ')];
			histories.Add(history);
		}

		return histories.Sum(h => GetNextInSequence(h));
	}

	private static long GetNextInSequence(IEnumerable<long> h)
	{
		List<List<long>> sequences = [[.. h]];

		List<long> differences = [1];
		do {
			List<long> sequence = sequences[^1];
			differences = [];

			for (int i = 0; i < sequence.Count - 1; i++) {
				differences.Add(sequence[i+1] - sequence[i]);
			}
			sequences.Add(differences);
		} while (differences.Any(x => x != 0));

		for (int i = 0; i < sequences.Count - 1; i++) {
			sequences[^(i+2)].Add(sequences[^(i + 1)][^1] + sequences[^(i + 2)][^1]);
		}
		return sequences[0][^1];
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}
}
