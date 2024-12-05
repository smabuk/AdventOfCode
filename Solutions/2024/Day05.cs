using static AdventOfCode.Solutions._2024.Day05Constants;
using static AdventOfCode.Solutions._2024.Day05Types;
using static AdventOfCode.Solutions._2024.Day05;

using PageSet = System.Collections.Generic.IEnumerable<int>;

namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 05: Print Queue
/// https://adventofcode.com/2024/day/05
/// </summary>
[Description("Print Queue")]
public sealed partial class Day05 {

	[Init]
	public static void    Init(string[] input, params object[]? args) => LoadRules(input, args.Method());
	public static string Part1(string[] _    , params object[]? args) => Solution1(args.Method()).ToString();
	public static string Part2(string[] _    , params object[]? args) => Solution2(args.Method()).ToString();


	private static ILookup<int, int> _mustAppearAfter  = default!;
	private static ILookup<int, int> _mustAppearBefore = default!;

	// Method "sort"
	private static List<Update>  _updates  = [];

	// Method "lookup"
	private static List<PageSet> _pageSets = [];

	public static void LoadRules(string[] input, string method)
	{
		HashSet<Rule> rules = [..
			input
				.TakeWhile(StringHelpers.HasNonWhiteSpaceContent)
				.Select(i => i.As<int>(separator: '|').ToArray())
				.Select(ints => new Rule(ints[0], ints[1]))
			];

		_mustAppearAfter  = rules.ToLookup(rule => rule.PageBefore, rule => rule.PageAfter);
		_mustAppearBefore = rules.ToLookup(rule => rule.PageAfter , rule => rule.PageBefore);

		if (method is SORT_METHOD) {
			_updates = [..
				input
				.Skip(rules.Count + 1)
				.Select(i => new Update([..i.As<int>(separator: ',').Select(n => new Page(n))]))
			];
		}

		if (method is LOOKUP_METHOD) {
			_pageSets = [..
				input
				.Skip(rules.Count + 1)
				.Select(i => i.As<int>(separator: ','))
			];
		}
	}

	private static int Solution1(string method)
		=> method switch
		{
			SORT_METHOD => _updates
				.Where(Day05Extensions.ObeysTheRules)
				.Sum(Day05Extensions.MiddlePageNo),

			LOOKUP_METHOD => _pageSets
				.Where(pageSet => pageSet.ObeysTheRules(_mustAppearAfter))
				.Sum(Day05Extensions.MiddlePage),

			_ => throw new NotImplementedException()
		};

	private static int Solution2(string method)
		=> method switch
		{
			SORT_METHOD => _updates
				.NotWhere(Day05Extensions.ObeysTheRules)
				.Sum(Day05Extensions.MiddlePageNo),

			LOOKUP_METHOD => _pageSets
				.NotWhere(pageSet => pageSet.ObeysTheRules(_mustAppearAfter))
				.Select(pageSet => pageSet.FixPageOrdering(_mustAppearAfter, _mustAppearBefore))
				.Sum(Day05Extensions.MiddlePage),

			_ => throw new NotImplementedException(),
		};


	public record Page(int PageNo) : IComparable<Page>
	{
		public int CompareTo(Page? other)
		{
			if (other is null) { return 0; }

			if (_mustAppearAfter[PageNo].Contains(other.PageNo)) {
				return BEFORE;
			} else if (_mustAppearBefore[PageNo].Contains(other.PageNo)) {
				return AFTER;
			}

			return 0;
		}
	}
}

file static class Day05Extensions
{
	// ****************************************************************************
	// "sort" version only requires these 3 extension methods
	public static List<Page> OrderedPages(this Update update) => [.. update.Pages.Order()];

	public static bool ObeysTheRules(this Update update)
		=> update.Pages.Zip(update.OrderedPages()).All(p => p.First == p.Second);

	public static int MiddlePageNo(this Update update) => update.OrderedPages()[update.Pages.Count / 2].PageNo;
	// ****************************************************************************


	// ****************************************************************************
	// "lookup" method requires the extension methods
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
	// ****************************************************************************

	public static string Method(this object[]? args)
	=> GetArgument(args, argumentNumber: 1, defaultResult: "sort").ToLowerInvariant();
}

internal static class Day05Types
{
	public record Rule(int PageBefore, int PageAfter);
	public record Update(List<Page> Pages);
}

file static class Day05Constants
{
	public const int BEFORE = -1;
	public const int AFTER  =  1;

	public const string SORT_METHOD   = "sort";
	public const string LOOKUP_METHOD = "lookup";
}
