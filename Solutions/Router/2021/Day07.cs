namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 07: The Treachery of Whales
/// https://adventofcode.com/2021/day/7
/// </summary>
[Description("The Treachery of Whales")]
public class Day07 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

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
		List<int> crabPositions = input[0].Split(",").Select(x => int.Parse(x)).ToList();

		int minP = crabPositions.Min();
		int maxP = crabPositions.Max();
		int possibleDistances = maxP - minP + 1;

		// Precalculate the fuel costs for each possible distance
		var fuelCosts =
			Enumerable
				.Range(0, possibleDistances)
				.Select(d => new { Distance = d, Fuel = FuelCost(d) })
				.ToDictionary(df => df.Distance);

		// Return the option with the minimum fuel costs
		return Enumerable
				.Range(minP, possibleDistances)
				.Select(sumOfFuel)
				.Min();



		// Calculate the sum of the fuel costs for all the crabs to reach a particular destinationPosition
		int sumOfFuel(int destinationPosition) =>
			crabPositions
				.Select(crabPosition => fuelCosts[Math.Abs(crabPosition - destinationPosition)].Fuel)
				.Sum();
	}

	static int FuelCost(int n) => SequenceHelpers.TriangularNumber(n);
}
