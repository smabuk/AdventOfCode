namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 15: Beacon Exclusion Zone
/// https://adventofcode.com/2022/day/15
/// </summary>
[Description("Beacon Exclusion Zone")]
public sealed partial class Day15 {

	[Init]
	public static    void Init(string[] input, params object[]? _) => LoadPairs(input);
	public static string Part1(string[] input, params object[]? args) {
		int rowToSearch = GetArgument<int>(args, argumentNumber: 1, 2_000_000);
		return Solution1(input, rowToSearch).ToString();
	}
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static IEnumerable<SensorBeaconPair> _pairs = Array.Empty<SensorBeaconPair>();

	private static void LoadPairs(string[] input) {
		_pairs = input.Select(SensorBeaconPair.Parse);
	}

	private static int Solution1(string[] input, int rowToSearch) {
		int minX = _pairs.Select(p => p.Sensor.X).Union(_pairs.Select(p => p.Beacon.X)).Min();
		int maxX = _pairs.Select(p => p.Sensor.X).Union(_pairs.Select(p => p.Beacon.X)).Max();
		
		List<Point> beacons = _pairs.Select(p => p.Beacon).ToList();
		HashSet<Point> found= new();

		foreach (SensorBeaconPair sbp in _pairs) {
			for (int x = minX; x <= maxX; x++) {
				Point check = new(x, rowToSearch);
				if (ManhattanDistance(sbp.Sensor, check) <= sbp.ManhattanDistance) {
					if (!beacons.Contains(check)) {
						found.Add(check);
					}
				}
			}
		}

		// 4403084 Too Low and too slow
		// 4403085 Too Low about 5s
		return found.Count;
	}

	private static string Solution2(string[] input) {
		//string inputLine = input[0];
		//List<string> inputs = input.ToList();
		//List<SensorBeaconPair> instructions = input.Select(ParseLine).ToList();
		return "** Solution not written yet **";
	}

	private static long ManhattanDistance(Point from, Point to) => Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);

	private partial record SensorBeaconPair(Point Sensor, Point Beacon) : IParsable<SensorBeaconPair> {
		public long ManhattanDistance => ManhattanDistance(Sensor, Beacon);

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
