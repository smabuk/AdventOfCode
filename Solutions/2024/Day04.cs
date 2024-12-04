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

	private static int Solution1()
	{
		return XmasRegEx().Count(_wordSearch.RowsAsStrings().AsString())
			 + XmasRegEx().Count(_wordSearch.ColsAsStrings().AsString())
			 + XmasRegEx().Count(_wordSearch.DiagonalsAsStrings().AsString());
	}

	private static int Solution2() => _wordSearch.Walk().Count(_wordSearch.IsXmasCross);
}

file static class Day04Extensions
{
	public static bool IsXmasCross(this char[,] array, (int Col, int Row) point)
	{
		if (array[point.Col, point.Row] == A) {
			char[,] subArray = array.SubArray(point.Col - 1, point.Row - 1, 3, 3);

			if (MasRegEx().Count(subArray.DiagonalsLRAsStrings().AsString()) == 1 &&
				MasRegEx().Count(subArray.DiagonalsRLAsStrings().AsString()) == 1) {
				return true;
			}
		}

		return false;
	}

	public static string AsString(this IEnumerable<string> strings) => string.Join(SPACE, strings);
	public static IEnumerable<string> DiagonalsAsStrings(this char[,] array) =>
		[.. array.DiagonalsLRAsStrings(), .. array.DiagonalsRLAsStrings()];

	public static IEnumerable<string> DiagonalsLRAsStrings(this char[,] array)
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

	public static IEnumerable<string> DiagonalsRLAsStrings(this char[,] array)
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

	[GeneratedRegex("""(MAS|SAM)""")]
	public static partial Regex MasRegEx();
}

file static class Day04Constants
{
	public const char A = 'A';
	public const char SPACE = ' ';
}
