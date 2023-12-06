namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 06: Wait For It
/// https://adventofcode.com/2023/day/06
/// </summary>
[Description("Wait For It")]
public sealed partial class Day06
{

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static IEnumerable<Race> _races = [];

	private static int Solution1(string[] input)
	{
		int[] raceTimes     = [.. input[0][10..].AsInts()];
		int[] raceDistances = [.. input[1][10..].AsInts()];

		List<Race> races = [.. raceTimes.Zip(raceDistances).Select(td => new Race(td.First, td.Second))];

		int productOfWins = 1;

		foreach (Race race in races) {
			int wins = 0;
			for (int i = 1; i < race.Time - 1; i++) {
				wins += (Boat.Win(i, race.Time, race.Distance)) ? 1 : 0;
			}
			productOfWins *= wins;
		}

		return productOfWins;
	}

	private static string Solution2(string[] input)
	{
		//string inputLine = input[0];
		//List<string> inputs = input.ToList();
		//List<Race> races = [.. input.As<Race>()];
		return "** Solution not written yet **";
	}

	private record Race(int Time, int Distance);
	private record Boat()
	{
		public static int Distance(int timeButtonHeld, int timeAllowed) => (timeAllowed - timeButtonHeld) * timeButtonHeld;
		public static bool Win(int timeButtonHeld, int timeAllowed, int distanceToBeat) => ((timeAllowed - timeButtonHeld) * timeButtonHeld) > distanceToBeat;

	}

}
