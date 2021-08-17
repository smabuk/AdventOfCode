namespace AdventOfCode.Solutions.Year2015;

/// <summary>
/// Day 20: Infinite Elves and Infinite Houses
/// https://adventofcode.com/2015/day/XX
/// </summary>
public class Day20 {

	private static int Solution1(string[] input) {
		int target = int.Parse(input[0]);

		int presents = 0;
		int houseNo;
		// Factorials
		for (houseNo = 1; presents < target; houseNo++) {
			List<int> factors = Factor(houseNo);
			presents = factors.Sum() * 10;
		}
		return houseNo - 1;
	}

	// https://stackoverflow.com/questions/239865/best-way-to-find-all-factors-of-a-given-number#239877
	static List<int> Factor(int number) {
		List<int>? factors = new();
		int max = (int)Math.Sqrt(number);  // Round down

		for (int factor = 1; factor <= max; ++factor) // Test from 1 to the square root, or the int below it, inclusive.
		{
			if (number % factor == 0) {
				factors.Add(factor);
				if (factor != number / factor) // Don't add the square root twice!  Thanks Jon Skeet
					factors.Add(number / factor);
			}
		}
		return factors;
	}

	private static int Solution2(string[] input) {
		int target = int.Parse(input[0]);

		int noOfElves = 50;
		int multiplier = 11;

		int[] houses = new int[target / multiplier + 1];
		for (int elf = 1; elf < houses.Length; elf++) {
			int count = 0;
			for (int house = elf; house < houses.Length && count < noOfElves; house += elf) {
				houses[house] += elf * multiplier;
				count++;
			}
		}

		int houseNo;
		for (houseNo = 1; houseNo < houses.Length; houseNo++) {
			if (houses[houseNo] > target) {
				break;
			}
		}

		return houseNo;
	}


	#region Problem initialisation
	public static string Part1(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		// int arg1 = GetArgument(args, 1, 25);
		input = input.StripTrailingBlankLineOrDefault();
		return Solution1(input).ToString();
	}
	public static string Part2(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		// int arg1 = GetArgument(args, 1, 25);
		input = input.StripTrailingBlankLineOrDefault();
		return Solution2(input).ToString();
	}
	#endregion

}
