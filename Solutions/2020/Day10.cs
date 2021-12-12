namespace AdventOfCode.Solutions.Year2020;

/// <summary>
/// Day 10: Adapter Array
/// https://adventofcode.com/2020/day/10
/// </summary>
[Description("Adapter Array")]
public class Day10 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		List<int> inputs = input.Select(i => int.Parse(i)).ToList();
		inputs.Sort();
		int start = 0;
		int count1Jolts = 0;
		int count2Jolts = 0;
		int count3Jolts = 0;

		foreach (int item in inputs) {
			if (item - start == 3) {
				count3Jolts++;
			} else if (item - start == 2) {
				count2Jolts++;
			} else if (item - start == 1) {
				count1Jolts++;
			}
			start = item;
		}
		return count1Jolts * (count3Jolts + 1);
	}

	private static long Solution2(string[] input) {
		List<int> inputs = input.Select(i => int.Parse(i)).ToList();
		inputs.Sort();
		int outlet = 0;
		int device = inputs.Last() + 3;
		inputs = inputs.Prepend(outlet).Append(device).ToList();

		long total = 1;
		long runningCount = 0;
		for (int i = 0; i < inputs.Count - 1; i++) {
			int item = inputs[i];
			int nearby = inputs.Skip(i + 1)
				.Take(3)
				.Where(a => a <= item + 3).Count();
			if (nearby == 1 && runningCount > 1) {
				if (runningCount == 2) {
					runningCount++;
				}
				total *= (runningCount - 1);
				runningCount = 0;
			} else if (nearby > 1) {
				runningCount += nearby;
			}
		}

		return total;
	}
}
