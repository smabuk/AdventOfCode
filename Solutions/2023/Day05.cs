namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 05: If You Give A Seed A Fertilizer
/// https://adventofcode.com/2023/day/05
/// </summary>
[Description("If You Give A Seed A Fertilizer")]
public sealed partial class Day05 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadMaps(input);
	public static string Part1(string[] input, params object[]? args) => Solution1().ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2().ToString();

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
	static IEnumerable<long> _seedInputValues = [];

	private static void LoadMaps(string[] input)
	{
		const int START_OF_NUMBERS = 7;
		_seedInputValues = [.. input[0][START_OF_NUMBERS..].As<long>(' ')];
		
		int inputIndex = 3;
		foreach (string mapName in MAP_NAMES) {
			_maps[mapName] = string.Join(Environment.NewLine, input[inputIndex..]).As<Map>();
			inputIndex += _maps[mapName].Mappings.Count + 2;
		}
	}

	private static long Solution1() => _seedInputValues.Min(GetLocation);
	private static long GetLocation(long destination)
	{
		long currentDestination = destination;
		foreach (string mapName in MAP_NAMES) {
			currentDestination = _maps[mapName].Destination(currentDestination);
		}
		return currentDestination;
	}

	private static long Solution2()
	{
		return
			_seedInputValues
			.Chunk(2).Select(n => new LongRange(n[0], n[0] + n[1] - 1))
			.SelectMany(GetHumidityRanges)
			.Select(r => r.Start)
			.Min(src => _maps["humidity-to-location"].Destination(src));
	}

	private static IEnumerable<LongRange> GetHumidityRanges(LongRange seedRange)
	{
		List<LongRange> nextSourceRanges = [seedRange];
		foreach (string mapName in MAP_NAMES[..^1]) {
			nextSourceRanges = [.. nextSourceRanges
				.Select(srcRange => _maps[mapName].DestinationRange(srcRange))
				.SelectMany(d => d)];
		}
		foreach (LongRange source in nextSourceRanges) {
			yield return source;
		}
	}

	private sealed record Map(List<Mapping> Mappings)  : IParsable<Map>
	{
		public long Destination(long source)
		{
			foreach (Mapping mapping in Mappings) {
				if (mapping.TryMapToDestination(source, out long destination)) {
					return destination;
				}
			}
			return source;
		}

		public IEnumerable<LongRange> DestinationRange(LongRange range)
		{
			bool overlapFound = false;
			foreach (Mapping mapping in Mappings) {
				if (TryGetOverlap(range, new(mapping.SourceStart, mapping.SourceStart + mapping.Length - 1), out LongRange resultRange)) {
					_ = mapping.TryMapToDestination(resultRange.Start, out long start);
					_ = mapping.TryMapToDestination(resultRange.End,   out long end);

					overlapFound = true;
					yield return new(start, end);
				}
			}
			if (overlapFound is false) {
				yield return range;
			}
		}

		private static bool TryGetOverlap(LongRange range1, LongRange range2, out LongRange result)
			=> range1.TryGetOverlap(range2, out result);

		private static bool IsNotABlankLine(string s) => !string.IsNullOrWhiteSpace(s); 

		public static Map Parse(string s, IFormatProvider? provider) 
			=> new([..
			s
			.Split(Environment.NewLine)
			.TakeWhile(IsNotABlankLine)
			.As<Mapping>()]);

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Map result)
			=> ISimpleParsable<Map>.TryParse(s, provider, out result);
	}

	private sealed record Mapping(long DestinationStart, long SourceStart, long Length) : IParsable<Mapping>
	{
		public bool TryMapToDestination(long source, out long destination)
		{
			destination = 0;
			if (source >= SourceStart && source < SourceStart + Length) {
				destination = DestinationStart + (source - SourceStart);
				return true;
			}
			return false;
		}


		public static Mapping Parse(string s, IFormatProvider? provider)
		{
			const char SEP = ' ';
			long[] values = [.. s.TrimmedSplit(SEP).As<long>()];
			return new(values[0], values[1], values[2]);
		}

		public static Mapping Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Mapping result)
			=> ISimpleParsable<Mapping>.TryParse(s, provider, out result);
	}
}

