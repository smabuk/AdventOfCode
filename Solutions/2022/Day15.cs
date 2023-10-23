namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 15: Beacon Exclusion Zone
/// https://adventofcode.com/2022/day/15
/// </summary>
[Description("Beacon Exclusion Zone")]
public sealed partial class Day15 {

	[Init]
	public static void    Init(string[] input, params object[]? _) => LoadPairs(input);
	public static string Part1(string[] input, params object[]? args) {
		int rowToSearch = GetArgument<int>(args, argumentNumber: 1, 2_000_000);
		return Solution1(input, rowToSearch).ToString();
	}
	public static string Part2(string[] input, params object[]? args) {
		int max = GetArgument<int>(args, argumentNumber: 1, 4_000_000);
		// Skip for now as I have no solution
		if (max > 20) {
			return "* Too slow *";
		}
		return Solution2(input, max).ToString();
	}

	private static IEnumerable<SensorBeaconPair> _pairs = Array.Empty<SensorBeaconPair>();

	private static void LoadPairs(string[] input) {
		_pairs = input.Select(SensorBeaconPair.Parse);
	}

	private static int Solution1(string[] input, int rowToSearch) {
		List<Point> beacons = _pairs.Select(p => p.Beacon).ToList();
		HashSet<Point> found = new();

		foreach (SensorBeaconPair sbp in _pairs) {
			if (sbp.Sensor.Y - sbp.ManhattanDistance <= rowToSearch && sbp.Sensor.Y + sbp.ManhattanDistance >= rowToSearch) {
				int rangeMin = Math.Abs(sbp.Sensor.Y - rowToSearch);
				int distance = sbp.ManhattanDistance - rangeMin;
				for (int x = sbp.Sensor.X - distance; x <= sbp.Sensor.X + distance; x++) {
					Point check = new(x, rowToSearch);
					if (ManhattanDistance(sbp.Sensor, check) <= sbp.ManhattanDistance) {
						if (!beacons.Contains(check)) {
							_ = found.Add(check);
						}
					}
				}
			}
		}

		return found.Count;
	}

	private static long Solution2(string[] input, int max) {
		List<Point> beacons = _pairs.Select(p => p.Beacon).ToList();
		HashSet<Point> found = new();
		int BLOCK_SIZE = int.Clamp(max, 10, 10_000);
		Console.WriteLine();
		char[,] cave = new char[BLOCK_SIZE + 1, BLOCK_SIZE + 1];
		for (int yBlock = 0; yBlock < max; yBlock+= BLOCK_SIZE) {
			for (int xBlock = 0; xBlock < max; xBlock += BLOCK_SIZE) {
				int offsetY = yBlock;
				int offsetX = xBlock;
				Console.WriteLine($"Checking : {offsetX}, {offsetY}");
				foreach ((int x, int y) in cave.Walk2dArray()) {
					cave[x, y] = ' ';
				}
				foreach (SensorBeaconPair sbp in _pairs) {
					for (int y = int.Clamp(sbp.Sensor.Y - sbp.ManhattanDistance, offsetY, Math.Min(max, offsetY + BLOCK_SIZE)); y <= int.Clamp(sbp.Sensor.Y + sbp.ManhattanDistance, offsetY, Math.Min(max, offsetY + BLOCK_SIZE)); y++) {
						for (int x = int.Clamp(sbp.Sensor.X - sbp.ManhattanDistance, offsetX, Math.Min(max, offsetX + BLOCK_SIZE)); x <= int.Clamp(sbp.Sensor.X + sbp.ManhattanDistance, offsetX, Math.Min(max, offsetX + BLOCK_SIZE)); x++) {
							if (ManhattanDistance(sbp.Sensor, new(x, y)) <= sbp.ManhattanDistance) {
								cave[x - offsetX, y - offsetY] = '#';
							}
						}
					}
				}
				foreach ((int x, int y, char value) in cave.Walk2dArrayWithValues()) {
					if (value == ' ') {
						return ((x + offsetX) * 4_000_000) + offsetY + y;
					}
				}
			}
		}

		return 0;
	}

	private static int ManhattanDistance(Point from, Point to) => Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);

	private partial record SensorBeaconPair(Point Sensor, Point Beacon) : IParsable<SensorBeaconPair> {
		public int ManhattanDistance => ManhattanDistance(Sensor, Beacon);

		public static SensorBeaconPair Parse(string s) {
			Match match = SensorBeaconPairRegEx().Match(s);
			if (match.Success) {
				return new(new(match.Groups["sx"].Value.AsInt(), match.Groups["sy"].Value.AsInt()),
					new(match.Groups["bx"].Value.AsInt(), match.Groups["by"].Value.AsInt()));
			}
			return null!;
		}

		[GeneratedRegex("""Sensor at x=(?<sx>[\+\-]*\d+), y=(?<sy>[\+\-]*\d+): closest beacon is at x=(?<bx>[\+\-]*\d+), y=(?<by>[\+\-]*\d+)""")]
		private static partial Regex SensorBeaconPairRegEx();

		public static SensorBeaconPair Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out SensorBeaconPair result) => throw new NotImplementedException();
	}

}
