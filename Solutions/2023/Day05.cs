
using System.Data;

namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 05: If You Give A Seed A Fertilizer
/// https://adventofcode.com/2023/day/05
/// </summary>
[Description("If You Give A Seed A Fertilizer")]
public sealed partial class Day05 {

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


	private static long Solution1(string[] input) {
		Dictionary<string, Map> maps = [];
		
		int inputIndex = 0;
		List<long> seeds = [.. input[inputIndex][7..].AsLongs()];

		inputIndex += 3;
		foreach (string mapName in MAP_NAMES) {
			maps[mapName] = Map.Parse(input[inputIndex..]);
			inputIndex += maps[mapName].Mappings.Count + 2;
		}

		long lowestLocationNumber = int.MaxValue;

		foreach (long seed in seeds) {
			long destination = seed;
			foreach (string mapName in MAP_NAMES) {
				destination =  maps[mapName].Destination(destination);
			}
			if (destination < lowestLocationNumber) {
				lowestLocationNumber = destination;
			}
		}

		return lowestLocationNumber;
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
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

	private record Mapping(long DestinationStart, long SourceStart, long Length) : ISimpleParsable<Mapping>
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

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Mapping result)
			=> ISimpleParsable<Mapping>.TryParse(s, provider, out result);
	}
}
