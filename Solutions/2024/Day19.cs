using System;
using System.Collections.Concurrent;

namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 19: Linen Layout
/// https://adventofcode.com/2024/day/19
/// </summary>
[Description("Linen Layout")]
public static partial class Day19 {

	private static List<string> _towelPatterns  = [];
	private static List<string> _desiredDesigns = [];

	[Init]
	public static void LoadTowels(string[] input)
	{
		_towelPatterns  = [.. input[0].TrimmedSplit(',')];
		_desiredDesigns = [.. input[2..]];
	}

	public static int Part1(string[] _)
		=> _desiredDesigns
			.Where(design => design.IsPossible(_towelPatterns))
			.Count();

	public static long Part2(string[] _)
	{
		Dictionary<string, long> cache = [];
		cache.Add("", 1);

		return _desiredDesigns
			.Select(design => design.AsSpan().AllPossible([.._towelPatterns], cache))
			.Sum();
	}

	public static bool IsPossible(this string design, List<string> patterns)
	{
		return design is "" ||
			patterns
			.Where(pattern => design.StartsWith(pattern, StringComparison.Ordinal))
			.Any(pattern => design[pattern.Length..].IsPossible(patterns));
	}

	public static long AllPossible(this ReadOnlySpan<char> design, ReadOnlySpan<string> patterns, Dictionary<string, long> cache)
	{
		if (cache.TryGetValue(design.ToString(), out long cacheCount)) {
			return cacheCount;
		}

		long sum = 0;
		foreach (string pattern in patterns) {
			ReadOnlySpan<char> patternSpan = pattern.AsSpan();
			if (!design.StartsWith(patternSpan, StringComparison.Ordinal)) {
				continue;
			}
			sum += design[pattern.Length..].AllPossible(patterns, cache);
		}

		cache.Add(design.ToString(), sum);
		return sum;
	}
}
