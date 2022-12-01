namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 01: Calorie Counting
/// https://adventofcode.com/2022/day/1
/// </summary>
[Description("Calorie Counting")]
public partial class Day01 {

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
				calories += int.Parse(input[i]); ;
			}
		}

		return caloriesMax;
	}

	private static int Solution2(string[] input) {
		List<int> caloriesPerElf = new();

		int calories = 0;
		for (int i = 0; i <= input.Length; i++) {
			if (i == input.Length || input[i] == "") {
				caloriesPerElf.Add(calories);
				calories = 0;
			} else {
				calories += int.Parse(input[i]);
			}
		}

		return caloriesPerElf
			.OrderByDescending(x => x)
			.Take(3)
			.Sum();
	}

}
