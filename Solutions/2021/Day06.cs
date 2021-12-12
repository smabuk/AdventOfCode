namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 06: Lanternfish
/// https://adventofcode.com/2021/day/6
/// </summary>
[Description("Lanternfish")]
public class Day06 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	record RecordType(string Name, int Value);

	private static int Solution1(string[] input) {
		List<int> fish = input[0].Split(",").Select(x => int.Parse(x)).ToList();

		for (int i = 0; i < 80; i++) {
			int noOnZero = fish.Count(f => f == 0);
			for (int fIndex = 0; fIndex < fish.Count; fIndex++) {
				int f = fish[fIndex];
				fish[fIndex] = (f == 0 ? 6 : f - 1);
			}
			fish.AddRange(Enumerable.Repeat(8, noOnZero));
		}

		return fish.Count;
	}

	private static long Solution2(string[] input) {
		List<int> fish = input[0].Split(",").Select(x => int.Parse(x)).ToList();

		long[] fishCounts = new long[9];
		for (int i = 0; i <= 8; i++) {
			fishCounts[i] = fish.Count(f => f == i);
		}
		for (int day = 0; day < 256; day++) {
			long noOnZero = fishCounts[0];
			for (int i = 1; i <= 8; i++) {
				fishCounts[i - 1] = fishCounts[i];
			}
			fishCounts[8] = noOnZero;
			fishCounts[6] += noOnZero;
		}

		return fishCounts.Sum();
	}
}
