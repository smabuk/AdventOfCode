namespace AdventOfCode.Solutions.Year2019;

/// <summary>
/// Day 01: The Tyranny of the Rocket Equation
/// https://adventofcode.com/2019/day/1
/// </summary>
[Description("The Tyranny of the Rocket Equation")]
public class Day01 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		List<int> moduleMasses = input.Select(x => int.Parse(x)).ToList();

		return moduleMasses
			.Select(m => CalculateFuelFromMass(m))
			.Sum();
	}

	private static int Solution2(string[] input) {
		List<int> moduleMasses = input.Select(x => int.Parse(x)).ToList();

		return moduleMasses
			.Select(m => CalculateFuelFromMassPart2(m))
			.Sum();
	}

	private static int CalculateFuelFromMass(int mass) => (int)Math.Floor(mass / 3.0) - 2;

	private static int CalculateFuelFromMassPart2(int mass) {
		int sum = 0;
		int fuel = CalculateFuelFromMass(mass);
		while (fuel > 0) {
			sum += fuel;
			fuel = CalculateFuelFromMass(fuel);
		}
		return sum;
	}
}
