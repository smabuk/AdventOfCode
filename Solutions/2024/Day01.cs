﻿using static AdventOfCode.Solutions._2024.Day01Constants;
using static AdventOfCode.Solutions._2024.Day01Types;
namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 01: Historian Hysteria
/// https://adventofcode.com/2024/day/01
/// </summary>
[Description("Historian Hysteria")]
public sealed partial class Day01 {

	public static string Part1(string[] _) => Solution1().ToString();
	public static string Part2(string[] _) => Solution2().ToString();

	private static List<int> _left  = [];
	private static List<int> _right = [];

	[Init]
	public static void LoadLists(string[] input)
	{
		_left  = [.. input.GetList(LEFT)];
		_right = [.. input.GetList(RIGHT)];
	}

	private static int Solution1() => (_left, _right).TotalDistance();
	private static int Solution2() => _left.TotalSimilarityScore(_right.CountBy(n => n).ToDictionary());
}

file static class Day01Extensions
{
	public static IEnumerable<int> GetList(this string[] input, int index) =>
		[.. input.Select(i => i.TrimmedSplit(SPACE)[index].As<int>())];

	public static int Distance(this (int First, int Second) numbers) => int.Abs(numbers.First - numbers.Second);

	public static int TotalDistance(this (IEnumerable<int> List1, IEnumerable<int> List2) lists) =>
		lists.List1.Order()
		.Zip(lists.List2.Order())
		.Sum(Distance);

	public static int SimilarityScore(this int number, Dictionary<int, int> counts) =>
		number * counts.GetValueOrDefault(number, 0);

	public static int TotalSimilarityScore(this IEnumerable<int> list, Dictionary<int, int> counts) =>
		list.Sum(number => number.SimilarityScore(counts));
}

internal sealed partial class Day01Types
{
}

file static class Day01Constants
{
	public const char SPACE = ' ';

	public const int  LEFT  = 0;
	public const int  RIGHT = 1;
}
