namespace AdventOfCode.Solutions._2015;

/// <summary>
/// Day 17: No Such Thing as Too Much
/// https://adventofcode.com/2015/day/17
/// </summary>
[Description("No Such Thing as Too Much")]
public class Day17 {

	public static string Part1(string[] input, params object[]? args) {
		int noOfLiters = GetArgument(args, 1, 150);
		return Solution1(input, noOfLiters).ToString();
	}

	public static string Part2(string[] input, params object[]? args) {
		int noOfLiters = GetArgument(args, 1, 150);
		return Solution2(input, noOfLiters).ToString();
	}

	private static int Solution1(string[] input, int noOfLiters) {
		int[] containers = input.Select(i => int.Parse(i)).ToArray();

		int noOfCombinations = 0;
		List<IEnumerable<int>> combinations = new();
		for (int k = 2; k <= containers.Length; k++) {
			noOfCombinations += containers.Combinations(k).Where(x => x.Sum() == noOfLiters).Count();
		}

		return noOfCombinations;
	}


	private static int Solution2(string[] input, int noOfLiters) {
		int[] containers = input.Select(i => int.Parse(i)).ToArray();

		int noOfCombinations = 0;
		List<IEnumerable<int>> combinations = new();
		for (int k = 2; k <= containers.Length; k++) {
			noOfCombinations = containers.Combinations(k).Where(x => x.Sum() == noOfLiters).Count();
			if (noOfCombinations > 0) {
				break;
			}
		}

		return noOfCombinations;
	}
}
