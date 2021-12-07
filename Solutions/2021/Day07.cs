namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 07: The Treachery of Whales
/// https://adventofcode.com/2021/day/7
/// </summary>
[Description("The Treachery of Whales")]
public class Day07 {

	record RecordType(string Name, int Value);

	private static int Solution1(string[] input) {
		List<int> positions = input[0].Split(",").Select(x => int.Parse(x)).ToList();
		int minFuel = int.MaxValue;

		int minP = positions.Min();
		int maxP = positions.Max();
		for (int i = minP; i <= maxP; i++) {
			int fuel = positions.Sum(x => Math.Abs(x - i));
			minFuel = Math.Min(minFuel, fuel);
		}

		return minFuel;
	}
	private static int Solution2(string[] input) {
		List<int> positions = input[0].Split(",").Select(x => int.Parse(x)).ToList();
		int minFuel = int.MaxValue;

		int minP = positions.Min();
		int maxP = positions.Max();

		var fuelCosts = Enumerable
			.Range(0, maxP - minP + 1)
			.Select(x => new { N = x, Fuel = FuelCost(x) })
			.ToDictionary(n => n.N);

		for (int i = minP; i <= maxP; i++) {
			int fuel = positions.Sum(x => fuelCosts[Math.Abs(x - i)].Fuel);
			minFuel = Math.Min(minFuel, fuel);
		}
		
		return minFuel;
	}

	static int FuelCost(int n) => SequenceHelpers.TriangularNumber(n);





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
