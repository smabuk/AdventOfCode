using static AdventOfCode.Solutions._2024.Day05Constants;
using static AdventOfCode.Solutions._2024.Day05Types;

using PageSet = System.Collections.Generic.IEnumerable<int>;

namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 05: Print Queue
/// https://adventofcode.com/2024/day/05
/// </summary>
[Description("Print Queue")]
public sealed partial class Day05 {

	public static string Part1(string[] _) => Solution1().ToString();
	public static string Part2(string[] _) => Solution2().ToString();

	private static List<Rule> _rules = [];
	private static List<PageSet> _pageSets = [];

	[Init]
	public static void LoadRules(string[] input)
	{
		_rules = [.. 
			input
			.TakeWhile(StringHelpers.HasNonWhiteSpaceContent)
			.As<Rule>()
			];

		_pageSets = [..
			input
			.Skip(_rules.Count + 1)
			.Select(i => i.As<int>(separator: ','))
			];
	}

	private static int Solution1()
		=> _pageSets
			.Where(pages => pages.ObeysTheRules(_rules))
			.Sum(pages => pages.Middle());

	private static int Solution2()
		=> _pageSets
			.NotWhere(pages => pages.ObeysTheRules(_rules))
			.Select(pages => pages.FixPageOrdering(_rules))
			.Sum(pages => pages.Middle());
}

file static class Day05Extensions
{
	public static bool ObeysTheRules(this PageSet pages, IEnumerable<Rule> rules)
	{
		List<int> pageSet = [.. pages];
		for (int i = 0; i < pageSet.Count; i++) {
			if (rules.Any(r => r.PageBefore == pageSet[i] && pageSet[..i].Contains(r.PageAfter))) {
				return false;
			}
		}

		return true;
	}

	public static PageSet FixPageOrdering(this PageSet pageSet, IEnumerable<Rule> rules)
	{
		List<int> newPageSet = [];

		foreach (int page in pageSet) {
			List<int> ixAfter = [..
				rules
				.Where(r => r.PageAfter == page)
				.Select(r => newPageSet.IndexOf(r.PageBefore))
				.Where(IndexFound)
				];
			List<int> ixBefore = [..
				rules
				.Where(r => r.PageBefore == page)
				.Select(r => newPageSet.IndexOf(r.PageAfter))
				.Where(IndexFound)
				];

			if (ixAfter  is []) { ixAfter  = [0]; };
			if (ixBefore is []) { ixBefore = [newPageSet.Count]; };

			int ix = int.Max(ixAfter.Max(), ixBefore.Min());

			newPageSet.Insert(ix, page);
		}

		return newPageSet;
	}

	public static int Middle(this PageSet pages)
	{
		List<int> pageSet = [.. pages];
		return pageSet[pageSet.Count / 2];
	}

	public static bool IndexFound(int ix) => ix is not NOT_FOUND;
}

internal sealed partial class Day05Types
{
	public sealed record Rule(int PageBefore, int PageAfter) : IParsable<Rule>
	{
		public static Rule Parse(string s, IFormatProvider? provider)
		{
			int[] tokens = [.. s.As<int>(separator: '|')];
			return new(tokens[0], tokens[1]);
		}

		public static Rule Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Rule result)
			=> ISimpleParsable<Rule>.TryParse(s, provider, out result);
	}
}

file static class Day05Constants
{
	public const int NOT_FOUND = -1;
}
