namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 01: Calorie Counting
/// https://adventofcode.com/2022/day/1
/// </summary>
[Description("Calorie Counting")]
public sealed partial class Day01 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		int caloriesMax = 0;

		int calories = 0;
		for (int i = 0; i <= input.Length; i++) {
			if (i == input.Length || input[i] == "") {
				if (calories > caloriesMax) {
					caloriesMax = calories;
				}
				calories = 0;
			} else {
				calories += input[i].AsInt();
				;
			}
		}

		return caloriesMax;
	}

	private static int Solution2(string[] input) {
		int[] caloriesPerElf = new int[3];

		int calories = 0;
		for (int i = 0; i <= input.Length; i++) {
			if (i == input.Length || input[i] == "") {
				int minLargeCalories = caloriesPerElf.Min();
				if (calories > minLargeCalories) {
					for (int e = 0; e < 3; e++) {
						if (minLargeCalories == caloriesPerElf[e]) {
							caloriesPerElf[e] = calories;
							break;
						}
					}
				}
				calories = 0;
			} else {
				calories += input[i].AsInt();
			}
		}

		return caloriesPerElf.Sum();
	}

}
