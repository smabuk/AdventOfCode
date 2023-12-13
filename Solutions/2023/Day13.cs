namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 13: Point of Incidence
/// https://adventofcode.com/2023/day/13
/// </summary>
[Description("Point of Incidence")]
public sealed partial class Day13 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadPatterns(input);
	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	public const char MIRROR = '#';
	public const char ASH    = '.';

	private static int Solution1(string[] input) => LoadPatterns(input).Sum(pattern => pattern.Value);

	private static int Solution2(string[] input) => LoadPatterns(input).Sum(pattern => pattern.AlternateValue);

	private static IEnumerable<Pattern> LoadPatterns(string[] input)
	{
		int inputIndex = 0;
		while (inputIndex < input.Length) {
			Pattern pattern = string.Join(Environment.NewLine, input[inputIndex..]).As<Pattern>();
			yield return pattern;
			inputIndex += pattern.NoOfRows + 1;
		}
	}

	private sealed record Pattern(List<Point> Mirrors) : IParsable<Pattern> {

		public int NoOfColumns = Mirrors.Max(mirror => mirror.X) + 1;
		public int NoOfRows    = Mirrors.Max(mirror => mirror.Y) + 1;

		public int Value => LineOfSymmetryValue();
		public int AlternateValue => AlternateLineOfSymmetryValue();

		private int LineOfSymmetryValue()
		{
			string[] columnStrings = [..Enumerable.Range(0, NoOfColumns)
				.Select(x => Mirrors.Where(m => m.X == x).Select(m => m.Y))
				.Select(v => v.ValuesAsString(NoOfRows))];

			for (int col = 0; col < NoOfColumns - 1; col++) {
				if (IsSymmetrical(columnStrings, col, NoOfColumns, out int value)) {
					return value;
				}
			}

			string[] rowStrings = [..Enumerable.Range(0, NoOfRows)
				.Select(y => Mirrors.Where(m => m.Y == y).Select(m => m.X))
				.Select(v => v.ValuesAsString(NoOfColumns))];


			for (int row = 0; row < NoOfRows - 1; row++) {
				if (IsSymmetrical(rowStrings, row, NoOfRows, out int value)) {
					return value * 100;
				}
			}

			throw new ApplicationException("Oh no!!");
		}
		private int AlternateLineOfSymmetryValue()
		{
			int oldValue = Value;

			string[] columnStrings = [..Enumerable.Range(0, NoOfColumns)
				.Select(x => Mirrors.Where(m => m.X == x).Select(m => m.Y))
				.Select(v => v.ValuesAsString(NoOfRows))];

			for (int col = 0; col < NoOfColumns - 1; col++) {
				if (IsSymmetrical(columnStrings, col, NoOfColumns, out int value, true) && value != oldValue) {
					return value;
				}
			}

			string[] rowStrings = [..Enumerable.Range(0, NoOfRows)
				.Select(y => Mirrors.Where(m => m.Y == y).Select(m => m.X))
				.Select(v => v.ValuesAsString(NoOfColumns))];

			for (int row = 0; row < NoOfRows - 1; row++) {
				if (IsSymmetrical(rowStrings, row, NoOfRows, out int value, true) && (value * 100) != oldValue) {
					return value * 100;
				}
			}

			throw new ApplicationException("Oh no!!");
		}

		private static bool IsSymmetrical(string[] strings, int index, int max, out int value, bool fuzzyTest = false)
		{
			value = 0;
			int firstPosition = -1;
			if (!TryFuzzyTest(strings[index], strings[index + 1], out int position, fuzzyTest)) {
				return false;
			} else  if (fuzzyTest && position >= 0) {
				firstPosition = position;
			}
			for (int i = 1; true; i++) {
				if (index + i + 1 >= max || index - i < 0) {
					value = index + 1;
					return true;
				}
				if (!TryFuzzyTest(strings[index - i], strings[index + i + 1], out position, fuzzyTest)) {
					break;
				} else if (fuzzyTest && position >= 0) {
					if (firstPosition >= 0) {
						break;
					}
					firstPosition = position;
				}
			}
			return false;
		}

		private static bool TryFuzzyTest(string string1, string string2, out int position, bool fuzzyTest = false)
		{
			position = -1;
			if (string1 == string2) { return true; }
			if (fuzzyTest is false) { return false; }

			for (int i = 0; i < string1.Length; i++) {
				if (string1[i] != string2[i]) {
					if (position >= 0) {
						return false;
					}
					position = i;
				}
			}

			return true;
		}
		private static bool IsNotABlankLine(string s) => !string.IsNullOrWhiteSpace(s);

		public static Pattern Parse(string s, IFormatProvider? provider)
		{
			return new([.. s
				.Split(Environment.NewLine)
				.TakeWhile(IsNotABlankLine)
				.AsPoints(MIRROR)
				]);
		}

		public static Pattern Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Pattern result)
			=> ISimpleParsable<Pattern>.TryParse(s, provider, out result);

		private enum LineOfSymmetry
		{
			None,
			Horizontal,
			Vertical,
		}
	}
}

public static class Day13Helpers
{
	public static string ValuesAsString(this IEnumerable<int> values, int maxValue)
	{
		HashSet<int> valuesSet = [.. values];
		return string.Join("", Enumerable.Range(0, maxValue).Select(i => valuesSet.Contains(i) ? Day13.MIRROR : Day13.ASH));
	}

}

