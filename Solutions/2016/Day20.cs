﻿using static AdventOfCode.Solutions._2016.Day20Constants;
using static AdventOfCode.Solutions._2016.Day20Types;
namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 20: Firewall Rules
/// https://adventofcode.com/2016/day/20
/// </summary>
[Description("Firewall Rules")]
public sealed partial class Day20 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadExclusions(input);
	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static IEnumerable<Exclusion> _exclusions = [];

	private static void LoadExclusions(string[] input) {
		_exclusions = input.As<Exclusion>();
	}

	private static uint Solution1(string[] input) {
		List<Exclusion> exclusions = [.. input.As<Exclusion>().OrderBy(e => e.MinValue)];
		uint currentMax = 0;

		foreach (Exclusion exclusion in exclusions) {
			if (exclusion.MinValue <= currentMax) {
				currentMax = uint.Max(exclusion.MaxValue, currentMax);
			} else {
				break;
			}
		}

		return currentMax + 1;
	}

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class Day20Extensions
{
}

internal sealed partial class Day20Types
{

	public sealed record Exclusion(uint MinValue, uint MaxValue) : IParsable<Exclusion>
	{
		public static Exclusion Parse(string s, IFormatProvider? provider)
		{
			string[] tokens = s.TrimmedSplit('-');
			return new(tokens[0].As<uint>(), tokens[1].As<uint>());
		}

		public static Exclusion Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Exclusion result)
			=> ISimpleParsable<Exclusion>.TryParse(s, provider, out result);
	}
}

file static class Day20Constants
{
}
