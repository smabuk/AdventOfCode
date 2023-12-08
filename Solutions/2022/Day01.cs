namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 01: Calorie Counting
/// https://adventofcode.com/2022/day/1
/// </summary>
[Description("Calorie Counting")]
public sealed partial class Day01 {

	[Init]
	public static     void Init(string[] input, params object[]? _) => ProcessInput(input);
	public static  string Part1(string[] input, params object[]? _) => Solution1().ToString();
	public static  string Part2(string[] input, params object[]? _) => Solution2().ToString();

	static int caloriesMax = 0;
	static int[] caloriesPerElf = new int[3];

	private static int Solution1() => caloriesMax;

	private static int Solution2() => caloriesPerElf.Sum();

	private static void ProcessInput(string[] input) {
		caloriesMax = 0;
		caloriesPerElf = new int[3];
		int calories = 0;
		for (int i = 0; i <= input.Length; i++) {
			if (i == input.Length || input[i] == "") {
				if (calories > caloriesMax) {
					caloriesMax = calories;
				}
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
				calories += input[i].As<int>();
			}
		}
	}
}
