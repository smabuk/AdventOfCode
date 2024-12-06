using PageSet = System.Collections.Generic.IEnumerable<int>;

namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 05: Print Queue
/// https://adventofcode.com/2024/day/05
/// </summary>
[Description("Print Queue")]
public static partial class Day05 {

	private static ILookup<int, int> _mustAppearAfter  = default!;
	private static ILookup<int, int> _mustAppearBefore = default!;

	// Method "sort"
	private static List<Update>  _updates  = [];

	// Method "lookup"
	private static List<PageSet> _pageSets = [];

	
	[Init] public static void LoadRules(string[] input, params object[]? args)
	{
		string method = args.Method();

		HashSet<Rule> rules = [..
			input
				.TakeWhile(StringHelpers.HasNonWhiteSpaceContent)
				.Select(i => i.As<int>(separator: '|').ToArray())
				.Select(ints => new Rule(ints[0], ints[1]))
			];

		_mustAppearAfter  = rules.ToLookup(rule => rule.PageBefore, rule => rule.PageAfter);

		if (method is SORT_METHOD) {
			_updates = [..
				input
				.Skip(rules.Count + 1)
				.Select(i => new Update([..i.As<int>(separator: ',').Select(n => new Page(n))]))
			];
		}

		if (method is LOOKUP_METHOD) {
			_mustAppearBefore = rules.ToLookup(rule => rule.PageAfter , rule => rule.PageBefore);
			_pageSets = [..
				input
				.Skip(rules.Count + 1)
				.Select(i => i.As<int>(separator: ','))
			];
		}
	}

	[Part1] public static int Part1(string _, params object[]? args)
		=> args.Method() switch
		{
			SORT_METHOD => _updates
				.Where(ObeysTheRules)
				.Sum(MiddlePageNo),

			LOOKUP_METHOD => _pageSets
				.Where(pageSet => pageSet.ObeysTheRules(_mustAppearAfter))
				.Sum(Day05ExtensionsForLookupMethod.MiddlePage),

			_ => throw new NotImplementedException()
		};

	[Part2] public static int Part2(string _, params object[]? args )
		=> args.Method() switch
		{
			SORT_METHOD => _updates
				.NotWhere(ObeysTheRules)
				.Sum(MiddlePageNo),

			LOOKUP_METHOD => _pageSets
				.NotWhere(pageSet => pageSet.ObeysTheRules(_mustAppearAfter))
				.Select(pageSet => pageSet.FixPageOrdering(_mustAppearAfter, _mustAppearBefore))
				.Sum(Day05ExtensionsForLookupMethod.MiddlePage),

			_ => throw new NotImplementedException(),
		};

	private static string Method(this object[]? args) => GetArgument(args, 1, "sort").ToLower();

	// ****************************************************************************
	// "sort" version only requires these 3 extension methods
	private static List<Page> OrderedPages(this Update update) => [.. update.Pages.Order()];

	private static bool ObeysTheRules(this Update update)
		=> update.Pages[..^1].Zip(update.Pages[1..]).All(p => p.First < p.Second);

	private static int MiddlePageNo(this Update update) => update.OrderedPages()[update.Pages.Count / 2].PageNo;
	// ****************************************************************************
}

public static partial class Day05
{
	private const int BEFORE = -1;
	private const int AFTER  =  1;

	private const string SORT_METHOD   = "sort";
	private const string LOOKUP_METHOD = "lookup";

	private record Rule(int PageBefore, int PageAfter);
	private record Update(List<Page> Pages);
	public record Page(int PageNo) : IComparable<Page>
	{
		public static bool operator <(Page page1, Page page2) => page1.CompareTo(page2) == BEFORE;
		public static bool operator >(Page page1, Page page2) => page1.CompareTo(page2) == AFTER;

		public int CompareTo(Page? other)
		{
			if (other is null) { return AFTER; };
			if (PageNo == other.PageNo) { return 0; };

			return _mustAppearAfter[PageNo].Contains(other.PageNo) ? BEFORE : AFTER;
		}
	}
}

file static class Day05ExtensionsForLookupMethod
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
