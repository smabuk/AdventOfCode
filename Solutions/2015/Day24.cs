using System.Diagnostics;

namespace AdventOfCode.Solutions._2015;

/// <summary>
/// Day 24: It Hangs in the Balance
/// https://adventofcode.com/2015/day/24
/// </summary>
[Description("It Hangs in the Balance")]
public sealed partial class Day24 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static long Solution1(string[] input) {
		List<int> packageWeights = input.AsInts().ToList();
		int targetWeight = packageWeights.Sum() / 3;
		int maxI = packageWeights.Count;
		long minQE = long.MaxValue;

		for (int i = 1; i < maxI; i++) {
			foreach (var passengerWeights in packageWeights.Combinations(i).Where(x => x.Sum() == targetWeight)) {
				long qe = passengerWeights.Select(l => Convert.ToInt64(l)).Aggregate((total, next) => total * next);
				if (qe >= minQE) {
					break;
				}

				List<int> remainingWeights = packageWeights.Except(passengerWeights).ToList();
				int j = 1;
				bool found = false;
				while (j < remainingWeights.Count && found == false) {
					foreach (var weights in remainingWeights.Combinations(j).Where(x => x.Sum() == targetWeight)) {
						maxI = i + 1;
						minQE = Math.Min(minQE, qe);
						found = true;
						break;
					}
					j++;
				}
			}
		}

		return minQE;
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}

}
