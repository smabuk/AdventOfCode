using System.Text;

using static AdventOfCode.Solutions._2024.Day04Constants;
using static AdventOfCode.Solutions._2024.Day04Types;
namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 04: Ceres Search
/// https://adventofcode.com/2024/day/04
/// </summary>
[Description("Ceres Search")]
public sealed partial class Day04 {

	public static string Part1(string[] _) => Solution1().ToString();
	public static string Part2(string[] _) => Solution2().ToString();

	private static char[,] _wordSearch = default!;

	[Init]
	public static void LoadWordSearch(string[] input) => _wordSearch = input.To2dArray();

	private static int Solution1() => _wordSearch.XmasCount();
	private static int Solution2() => _wordSearch.Walk().Count(_wordSearch.IsXmasCross);
}

file static class Day04Extensions
{
	public static int XmasCount(this char[,] array)
	{
		// RowsAsStrings() and ColsAsStrings() are existing helpers
		return XmasRegEx().Count(array.RowsAsStrings().AsString())
			 + XmasRegEx().Count(array.ColsAsStrings().AsString())
			 + XmasRegEx().Count(array.DiagonalsAsStrings().AsString());
	}

	public static int XmasCrossCount(this char[,] array) => array.Walk().Count(array.IsXmasCross);

	public static bool IsXmasCross(this char[,] array, (int Col, int Row) point)
	{
		if (array[point.Col, point.Row] is not A) { return false; }

		// get the 4 adjacent corners using a helper
		char[] corners = [.. array.GetAdjacentCells(point.Col, point.Row, includeDiagonals: true).Skip(4)];
		
		return corners is [M, S, M, S] or [S, M, S, M] or [M, M, S, S] or [S, S, M, M];
	}

	public static string AsString(this IEnumerable<string> strings) => string.Join(SPACE, strings);

	public static IEnumerable<string> DiagonalsAsStrings(this char[,] array) =>
		[.. array.DiagonalsSouthEastAsStrings(), .. array.DiagonalsSouthWestAsStrings()];

	public static IEnumerable<string> DiagonalsSouthEastAsStrings(this char[,] array)
	{
		StringBuilder stringBuilder = new();

		for (int col = array.ColsMax(); col >= 0; col--) {
			for (int row = 0; row < array.RowsCount(); row++) {
				if (array.TryGetValue(col + row, row, out char c)) {
					_ = stringBuilder.Append(c);
				} else {
					break;
				}
			}

			yield return stringBuilder.ToString();
			_ = stringBuilder.Clear();
		}

		for (int row = 1; row < array.RowsCount(); row++) {
			for (int col = 0; col < array.ColsCount(); col++) {
				if (array.TryGetValue(col, row + col, out char c)) {
					_ = stringBuilder.Append(c);
				} else {
					break;
				}
			}

			yield return stringBuilder.ToString();
			_ = stringBuilder.Clear();
		}
	}

	public static IEnumerable<string> DiagonalsSouthWestAsStrings(this char[,] array)
	{
		StringBuilder stringBuilder = new();

		for (int col = 0; col < array.ColsCount(); col++) {
			for (int row = 0; row < array.RowsCount(); row++) {
				if (array.TryGetValue(col - row, row, out char c)) {
					_ = stringBuilder.Append(c);
				} else {
					break;
				}
			}

			yield return stringBuilder.ToString();
			_ = stringBuilder.Clear();
		}

		for (int row = 1; row < array.RowsCount(); row++) {
			for (int col = 0; col < array.ColsCount(); col++) {
				if (array.TryGetValue(array.ColsMax() - col, row + col, out char c)) {
					_ = stringBuilder.Append(c);
				} else {
					break;
				}
			}

			yield return stringBuilder.ToString();
			_ = stringBuilder.Clear();
		}
	}
}

internal sealed partial class Day04Types
{
	[GeneratedRegex("""(?=XMAS|SAMX)""")]
	public static partial Regex XmasRegEx();
}

file static class Day04Constants
{
	public const char A     = 'A';
	public const char M     = 'M';
	public const char S     = 'S';
	public const char SPACE = ' ';
}
