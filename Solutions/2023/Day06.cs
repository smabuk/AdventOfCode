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

	/// <summary>
	/// This solution supports 3 ways of solving Part2:
	///		Brute force (force)
	///		Binary Chop (chop)
	///		Maths       (maths)
	/// </summary>
	public static string Part2(string[] input, params object[]? args)
	{
		return GetArgument(args, argumentNumber: 1, defaultResult: "maths").ToLowerInvariant() switch
		{
			"force" => Solution2_Using_BruteForce(input).ToString(),
			"chop"  => Solution2_Using_BinaryChop(input).ToString(),
			"maths" => Solution2_Using_Maths(input).ToString(),
			_ => "** Solution not written yet **",
		};
	}

	private static int Solution1(string[] input)
	{
		int[] raceTimes     = [.. input[TIME][NUMBERS_OFFSET..].As<int>(' ')];
		int[] raceDistances = [.. input[DIST][NUMBERS_OFFSET..].As<int>(' ')];

		int productOfWins = 1;
		foreach ((int raceTime, int raceDistance) in raceTimes.Zip(raceDistances)) {
			int wins = 0;
			for (int i = 1; i < raceTime; i++) {
				wins += Win(i, raceTime, raceDistance) ? 1 : 0;
			}
			productOfWins *= wins;
		}

		return productOfWins;
	}

	public static bool Win(long timeButtonHeld, long timeAllowed, long distanceToBeat) =>
		((timeAllowed - timeButtonHeld) * timeButtonHeld) > distanceToBeat;

	private static long Solution2_Using_BruteForce(string[] input)
	{
		long raceTime     = input[TIME][NUMBERS_OFFSET..].Replace(" ", "").As<long>();
		long raceDistance = input[DIST][NUMBERS_OFFSET..].Replace(" ", "").As<long>();

		long wins = 0;
		for (long t = 1; t < raceTime; t++) {
			wins += Win(t, raceTime, raceDistance) ? 1 : 0;
		}
		return wins;
	}

	private static long Solution2_Using_Maths(string[] input)
	{
		long raceTime     = input[TIME][NUMBERS_OFFSET..].Replace(" ", "").As<long>();
		long raceDistance = input[DIST][NUMBERS_OFFSET..].Replace(" ", "").As<long>();

		long firstWin = (long)Math.Ceiling((raceTime - Math.Sqrt((raceTime * raceTime) - (raceDistance * 4))) / 2);
		long lastWin  = (long)Math.Ceiling((raceTime + Math.Sqrt((raceTime * raceTime) - (raceDistance * 4))) / 2) - 1;

		return lastWin - firstWin + 1;
	}

	private static long Solution2_Using_BinaryChop(string[] input)
	{
		long raceTime = input[TIME][NUMBERS_OFFSET..].Replace(" ", "").As<long>();
		long raceDistance = input[DIST][NUMBERS_OFFSET..].Replace(" ", "").As<long>();

		long firstWin = FindWinsByBinaryChop(raceTime, raceDistance, true);
		long lastWin  = FindWinsByBinaryChop(raceTime, raceDistance, false);

		return lastWin - firstWin + 1;
	}
	
	private static long FindWinsByBinaryChop(long time, long distance, bool first = true)
	{
		long min = 1;
		long max = time - 1;
		while (min <= max) {
			long mid = (min + max) / 2;
			if (Win(mid, time, distance) != Win(mid+1, time, distance)) {
				return first ? ++mid : mid;
			} else if (Win(mid, time, distance) == first) {
				max = mid - 1;
			} else {
				min = mid + 1;
			}
		}
		throw new ApplicationException("Didn't find a solution!");
	}
}
