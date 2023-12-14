namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 13: Point of Incidence
/// https://adventofcode.com/2023/day/13
/// </summary>
[Description("Point of Incidence")]
public sealed partial class Day13
{
	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static int Solution1(string[] input) => Parse(input).Sum(pattern => GetValue(pattern));
	private static int Solution2(string[] input)
	{
		return Parse(input)
			.Select(pattern => (OldValue: GetValue(pattern), Pattern: pattern))
			.Sum(item => GetValue(item.Pattern, item.OldValue, checkForSmudge: true));
	}

	private static int GetValue(char[,] pattern, int oldValue = int.MinValue, bool checkForSmudge = false)
	{
		const int COL_MULTIPLIER = 1;
		const int ROW_MULTIPLIER = 100;

		string[] cols = [.. Enumerable.Range(0, pattern.ColsCount()).Select(col => pattern.ColAsString(col))];
		if (TryGetValue(cols, oldValue, COL_MULTIPLIER, out int value, checkForSmudge)) {
			return value;
		}

		string[] rows = [.. cols.Transpose()];
		if (TryGetValue(rows, oldValue, ROW_MULTIPLIER, out value, checkForSmudge)) {
			return value;
		}

		throw new ApplicationException("No symmetry found, so unable to return a value!");
	}


	private static bool TryGetValue(string[] strings, int oldValue, int multiplier, out int value, bool checkForSmudge = false)
	{
		value = int.MinValue;
		for (int index = 0; index < strings.Length - 1; index++) {
			if (TryIsSymmetrical(strings, index, strings.Length, out value, checkForSmudge) && (value * multiplier) != oldValue) {
				value *= multiplier;
				return true;
			}
		}

		return false;
	}


	private static bool TryIsSymmetrical(string[] strings, int index, int max, out int value, bool checkForSmudge = false)
	{
		value = int.MinValue;
		bool smudgeAlreadyFound = false;
		int i = 0;
		while (StillLinesToCheck()) {
			if (checkForSmudge is false) {
				if (strings[index - i] != strings[index + i + 1]) {
					return false;
				}
			} else {
				if (!TryCheckForSmudge(strings[index - i], strings[index + i + 1], out bool smudgeFound)) {
					return false;
				} else if (smudgeFound) {
					if (smudgeAlreadyFound is true) {
						return false;
					}
					smudgeAlreadyFound = true;
				}
			}
			i++;
		}

		value = index + 1;
		return true;

		bool StillLinesToCheck() => (index - i >= 0 && index + i + 1 < max);
	}

	private static bool TryCheckForSmudge(string string1, string string2, out bool smudgeFound)
	{
		ReadOnlySpan<char> span1 = string1.AsSpan();
		ReadOnlySpan<char> span2 = string2.AsSpan();

		smudgeFound = false;
		for (int i = 0; i < string1.Length; i++) {
			if (span1[i] != span2[i]) {
				if (smudgeFound) {
					return false;
				}
				smudgeFound = true;
			}
		}

		return true;
	}

	public static IEnumerable<char[,]> Parse(string[] input)
	{
		int inputIndex = 0;
		while (inputIndex < input.Length) {
			char[,] pattern = input[inputIndex..].TakeWhile(IsNotABlankLine).To2dArray();
			inputIndex += pattern.RowsCount() + 1;
			yield return pattern;
		}

		static bool IsNotABlankLine(string s) => !string.IsNullOrWhiteSpace(s);
	}

}
