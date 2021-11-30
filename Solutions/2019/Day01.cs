namespace AdventOfCode.Solutions.Year2019;

/// <summary>
/// Day 01: The Tyranny of the Rocket Equation
/// https://adventofcode.com/2019/day/1
/// </summary>
public class Day01 {

	private static long Solution1(string[] input) {
		List<string> inputs = input.ToList();

		long sum = 0;
		foreach (string item in inputs) {
			long mass = long.Parse(item);
			sum += CalculateFuelFromMass(mass);
		}
		return sum;
	}

	private static long CalculateFuelFromMass(long mass) => (long)Math.Floor(mass / 3.0) - 2;

	private static long Solution2(string[] input) {
		List<string> inputs = input.ToList();

		long sum = 0;
		foreach (string item in inputs) {
			long mass = long.Parse(item);
			sum += CalculateFuelFromMassPart2(mass);
		}
		return sum;
	}

	private static long CalculateFuelFromMassPart2(long mass) {
		long sum = 0;
		long fuel = CalculateFuelFromMass(mass);
		while (fuel > 0) {
			sum += fuel;
			fuel = CalculateFuelFromMass(fuel);
		}
		return sum;
	}

	#region Problem initialisation
	public static string Part1(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution1(input).ToString();
	}
	public static string Part2(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution2(input).ToString();
	}
	#endregion

}
