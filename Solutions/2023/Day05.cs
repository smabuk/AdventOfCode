namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 05: If You Give A Seed A Fertilizer
/// https://adventofcode.com/2023/day/05
/// </summary>
[Description("If You Give A Seed A Fertilizer")]
public sealed partial class Day05 {

	[Init]
	public static void Init(string[] input, params object[]? args)    => LoadMaps(input);
	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static readonly string[] MAP_NAMES = [
		"seed-to-soil",
		"soil-to-fertilizer",
		"fertilizer-to-water",
		"water-to-light",
		"light-to-temperature",
		"temperature-to-humidity",
		"humidity-to-location",
		];

	static readonly Dictionary<string, Map> _maps = [];

	private static void LoadMaps(string[] input)
	{
		int inputIndex = 0;
		List<long> seeds = [.. input[inputIndex][7..].AsLongs()];

		inputIndex += 3;
		foreach (string mapName in MAP_NAMES) {
			_maps[mapName] = Map.Parse(input[inputIndex..]);
			inputIndex += _maps[mapName].Mappings.Count + 2;
		}
	}

	private static long Solution1(string[] input) {
		List<long> seeds = [.. input[0][7..].AsLongs()];
		long lowestLocationNumber = int.MaxValue;

		foreach (long seed in seeds) {
			long destination = seed;
			foreach (string mapName in MAP_NAMES) {
				destination =  _maps[mapName].Destination(destination);
			}
			if (destination < lowestLocationNumber) {
				lowestLocationNumber = destination;
			}
		}

		return lowestLocationNumber;
	}

	private static long Solution2(string[] input) {
		List<Range> seedRanges = [];
		List<long> numbers = [.. input[0][7..].AsLongs()];
		for (int i = 0; i < numbers.Count; i+= 2) {
			seedRanges.Add(new Range(numbers[i], numbers[i] + numbers[i + 1] - 1));
		}

		List<Range> ranges = [];
		foreach (Range seedRange in seedRanges) {
			List<Range> sources = [ seedRange ] ;
			foreach (string mapName in MAP_NAMES[..^1]) {
				List<Range> newSources = [];
				foreach (Range sourceRange in sources) {
					newSources.AddRange([.. _maps[mapName].DestinationRange(sourceRange)]);
				}
				sources = [.. newSources];
			}
			ranges.AddRange(sources);
		}

		long lowestLocationNumber = int.MaxValue;
		foreach (Range lastRange in ranges) {
			lowestLocationNumber = Math.Min(_maps["humidity-to-location"].Destination(lastRange.Start), lowestLocationNumber);
		}

		return lowestLocationNumber;

	}


	private record Map(List<Mapping> Mappings) 
	{
		public long Destination(long source)
		{
			foreach (Mapping mapping in Mappings) {
				if (mapping.TryMap(source, out long value)) {
					return value;
				}
			}
			return source;
		}

		public IEnumerable<Range> DestinationRange(Range range)
		{
			bool overlapFound = false;
			foreach (Mapping mapping in Mappings) {
				if (TryGetOverlap(range, new(mapping.SourceStart, mapping.SourceStart + mapping.Length - 1), out Range resultRange)) {
					_ = mapping.TryMap(resultRange.Start, out long start);
					_ = mapping.TryMap(resultRange.End, out long end);

					yield return new(start, end);
					overlapFound = true;
				}
			}
			if (overlapFound is false) {
				yield return range;
			}
		}
		private static bool TryGetOverlap(Range range1, Range range2, out Range result)
		{
			result = default;
			long max = Math.Max(range1.Start, range2.Start);
			long min = Math.Min(range1.End, range2.End);
			if (max <= min) {
				result = new(max, min);
				return true;
			} else {
				return false;
			}
		}

		public static Map Parse(string[] s)
		{
			List<Mapping> mappings = [];
			foreach (string line in s) {
				if (string.IsNullOrWhiteSpace(line)) {
					return new(mappings);
				}
				mappings.Add(Mapping.Parse(line, null));
			}
			return new(mappings);
		}
	}

	private record Mapping(long DestinationStart, long SourceStart, long Length)
	{
		public bool TryMap(long value, out long match)
		{
			match = 0;
			if (value >= SourceStart && value < SourceStart + Length) {
				match = DestinationStart + (value - SourceStart);
				return true;
			}
			return false;
		}

		public static Mapping Parse(string s, IFormatProvider? provider)
		{
			const char SEP = ' ';
			long[] values = [.. s.Split(SEP).AsLongs()];
			return new(values[0], values[1], values[2]);
		}
	}

	private record struct Range(long Start, long End);
}

