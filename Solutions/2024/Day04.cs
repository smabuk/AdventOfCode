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
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static char[,] _wordSearch = default!;

	[Init]
	public static void LoadWordSearch(string[] input) => _wordSearch = input.To2dArray();

	private static int Solution1()
	{
		int count = 0;
		count += XmasRegEx().Count(string.Join(" ", _wordSearch.RowsAsStrings()));
		count += XmasRegEx().Count(string.Join(" ", _wordSearch.ColsAsStrings()));
		count += XmasRegEx().Count(string.Join(" ", _wordSearch.DiagonalsLRAsStrings()));
		count += XmasRegEx().Count(string.Join(" ", _wordSearch.DiagonalsRLAsStrings()));

		return count;
	}

	private static string Solution2(string[] input) => NO_SOLUTION_WRITTEN_MESSAGE;
}

file static class Day04Extensions
{
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
	[GeneratedRegex("""(?={XMAS}|SAMX)""")]
	public static partial Regex XmasRegEx();
}

file static class Day04Constants
{
}
