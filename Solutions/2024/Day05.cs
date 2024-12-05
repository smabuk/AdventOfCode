using PageSet = System.Collections.Generic.IEnumerable<int>;
using Rule = (int PageBefore, int PageAfter);

namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 05: Print Queue
/// https://adventofcode.com/2024/day/05
/// </summary>
[Description("Print Queue")]
public sealed partial class Day05 {

	public static string Part1(string[] _) => Solution1().ToString();
	public static string Part2(string[] _) => Solution2().ToString();

	private static ILookup<int, int> _mustAppearAfter = default!;
	private static ILookup<int, int> _mustAppearBefore = default!;
	private static List<PageSet> _pageSets = [];

	[Init]
	public static void LoadRules(string[] input)
	{
		List<Rule> rules = [.. 
			input
			.TakeWhile(StringHelpers.HasNonWhiteSpaceContent)
			.Select(i => i.As<int>(separator: '|').ToArray())
			.Select(ints => (ints[0], ints[1]))
			];

		_pageSets = [..
			input
			.Skip(rules.Count + 1)
			.Select(i => i.As<int>(separator: ','))
			];

		_mustAppearAfter  = rules.ToLookup(rule => rule.PageBefore, rule => rule.PageAfter);
		_mustAppearBefore = rules.ToLookup(rule => rule.PageAfter , rule => rule.PageBefore);
	}

	private static int Solution1()
		=> _pageSets
			.Where(pageSet => pageSet.ObeysTheRules(_mustAppearAfter))
			.Sum(pageSet => pageSet.MiddlePage());

	private static int Solution2()
		=> _pageSets
			.NotWhere(pageSet => pageSet.ObeysTheRules(_mustAppearAfter))
			.Select(pageSet => pageSet.FixPageOrdering(_mustAppearAfter, _mustAppearBefore))
			.Sum(pageSet => pageSet.MiddlePage());
}

file static class Day05Extensions
{
	public static bool ObeysTheRules(this PageSet pages, ILookup<int, int> mustAppearAfter)
	{
		List<int> pageSet = [.. pages];
		for (int i = 0; i < pageSet.Count; i++) {
			int pageNo = pageSet[i];
			// Make sure none of the pages that must appear after pageNo are in the
			// subset of pages before pageNo.
			if (mustAppearAfter[pageNo].Intersect(pageSet[..i]).Any()) {
				return false;
			}
		}

		return true;
	}
	public static int MiddlePage(this PageSet pages)
	{
		List<int> pageSet = [.. pages];
		return pageSet[pageSet.Count / 2];
	}

	public static PageSet FixPageOrdering(
		this PageSet pageSet,
		ILookup<int, int> mustAppearAfter,
		ILookup<int, int> mustAppearBefore)
	{
		List<int> newPageSet = [];

		foreach (int pageNo in pageSet) {
			int ix = PositionToInsert(newPageSet, pageNo, mustAppearBefore, mustAppearAfter);
			newPageSet.Insert(ix, pageNo);
		}

		return newPageSet;
	}

	private static int PositionToInsert(this List<int> newPageSet, int pageNo, ILookup<int, int> mustAppearBefore, ILookup<int, int> mustAppearAfter)
	{
		// list of positions of the pages that need to be before pageNo
		List<int> ixBefore =
			mustAppearBefore[pageNo]
			.Select(p => newPageSet.IndexOf(p))
			.Where(IndexFound)
			.ToList() switch
			{
				[] => [0],
				List<int> before => before
			};

		// list of positions of the pages that need to be after pageNo
		List<int> ixAfter =
			mustAppearAfter[pageNo]
			.Select(p => newPageSet.IndexOf(p))
			.Where(IndexFound)
			.ToList() switch
			{
				[] => [newPageSet.Count],
				List<int> after => after
			};

		// return the safe place in the list where we can position the page
		return int.Max(ixBefore.Max(), ixAfter.Min());

		static bool IndexFound(int ix) => ix is not -1;
	}
}
