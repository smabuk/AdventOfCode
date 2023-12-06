namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 06: Wait For It
/// https://adventofcode.com/2023/day/06
/// </summary>
[Description("Wait For It")]
public sealed partial class Day06
{
	private const int NUMBERS_OFFSET = 10;
	private const int TIME = 0;
	private const int DIST = 1;

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args)
	{
		return GetArgument(args, 1, "maths").ToLowerInvariant() switch
		{
			"force" => Solution2_Using_BruteForce(input).ToString(),
			"chop"  => Solution2_Using_BinaryChop(input).ToString(),
			"maths" => Solution2_Using_Maths(input).ToString(),
			_ => "** Solution not written yet **",
		};
	}

	private static int Solution1(string[] input)
	{
		int[] raceTimes     = [.. input[TIME][NUMBERS_OFFSET..].AsInts()];
		int[] raceDistances = [.. input[DIST][NUMBERS_OFFSET..].AsInts()];

		List<Race> races = [.. raceTimes.Zip(raceDistances).Select(td => new Race(td.First, td.Second))];

		int productOfWins = 1;

		foreach (Race race in races) {
			int wins = 0;
			for (int i = 1; i < race.Time - 1; i++) {
				wins += Win(i, race.Time, race.Distance) ? 1 : 0;
			}
			productOfWins *= wins;
		}

		return productOfWins;
	}

	private static long Solution2_Using_BinaryChop(string[] input)
	{
		long raceTime     = input[TIME][NUMBERS_OFFSET..].Replace(" ", "").AsLong();
		long raceDistance = input[DIST][NUMBERS_OFFSET..].Replace(" ", "").AsLong();

		(long firstWin, long lastWin) = FindWinsByBinaryChop(raceTime, raceDistance);

		return lastWin - firstWin + 1;
	}

	private static long Solution2_Using_BruteForce(string[] input)
	{
		long raceTime     = input[TIME][NUMBERS_OFFSET..].Replace(" ", "").AsLong();
		long raceDistance = input[DIST][NUMBERS_OFFSET..].Replace(" ", "").AsLong();

		long wins = 0;
		for (long t = 1; t < raceTime - 1; t++) {
			wins += Win(t, raceTime, raceDistance) ? 1 : 0;
		}
		return wins;
	}

	private static long Solution2_Using_Maths(string[] input)
	{
		long raceTime     = input[TIME][NUMBERS_OFFSET..].Replace(" ", "").AsLong();
		long raceDistance = input[DIST][NUMBERS_OFFSET..].Replace(" ", "").AsLong();

		long firstWin = (long)Math.Ceiling((raceTime - Math.Sqrt((raceTime * raceTime) - (raceDistance * 4))) / 2);
		long lastWin  = (long)Math.Ceiling((raceTime + Math.Sqrt((raceTime * raceTime) - (raceDistance * 4))) / 2) - 1;

		return lastWin - firstWin + 1;
	}

	public static bool Win(long timeButtonHeld, long timeAllowed, long distanceToBeat) =>
		((timeAllowed - timeButtonHeld) * timeButtonHeld) > distanceToBeat;

	private static long FindWinsByBruteForce(long time, long distance)
	{
		long wins = 0;
		for (long t = 1; t < time - 1; t++) {
			wins += Win(t, time, distance) ? 1 : 0;
		}
		return wins;
	}

	/// <summary>
	///		Uses a binary chop to find the range of wins
	/// </summary>
	/// <param name="time"></param>
	/// <param name="distance"></param>
	/// <param name="first">if true this finds the first win in the range, otherwise finds the last</param>
	/// <returns></returns>
	private static (long Lower, long Upper) FindWinsByBinaryChop(long time, long distance, bool first = true)
	{
		long min = 1;
		long max = time - 1;
		while (min <= max) {
			long mid = (min + max) / 2;
			if (Win(mid, time, distance) != Win(mid+1, time, distance)) {
				return first
					? (++mid, FindWinsByBinaryChop(time, distance, false).Upper)
					: (int.MinValue, mid);
			} else if (Win(mid, time, distance) == first) {
				max = mid - 1;
			} else {
				min = mid + 1;
			}
		}
		throw new ApplicationException("Didn't find a solution!");
	}

	private record Race(long Time, long Distance);
}
