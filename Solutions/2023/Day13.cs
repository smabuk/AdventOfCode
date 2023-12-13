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

	public const char MIRROR = '#';
	public const char ASH    = '.';

	private const int COL_MULTIPLIER = 1;
	private const int ROW_MULTIPLIER = 100;


	private static int Solution1(string[] input) => Pattern.Parse(input).Sum(pattern => pattern.Value);
	private static int Solution2(string[] input) => Pattern.Parse(input).Sum(pattern => pattern.AlternateValue);

	private sealed record Pattern(List<Point> Mirrors) : IParsable<Pattern>
	{
		public int NoOfColumns = Mirrors.Max(mirror => mirror.X) + 1;
		public int NoOfRows    = Mirrors.Max(mirror => mirror.Y) + 1;

		public int Value => GetValue(int.MinValue);
		public int AlternateValue => GetValue(Value, fuzzyTest: true);

		private int GetValue(int oldValue, bool fuzzyTest = false)
		{
			string[] strings = [..Enumerable.Range(0, NoOfColumns)
				.Select(x => Mirrors.Where(m => m.X == x).Select(m => m.Y))
				.Select(v => v.ValuesAsString(NoOfRows))];

			if (TryFindLineOfSymmetry(strings, oldValue, COL_MULTIPLIER, out int value, fuzzyTest)) {
				return value;
			}

			strings = [..Enumerable.Range(0, NoOfRows)
				.Select(y => Mirrors.Where(m => m.Y == y).Select(m => m.X))
				.Select(v => v.ValuesAsString(NoOfColumns))];

			if (TryFindLineOfSymmetry(strings, oldValue, ROW_MULTIPLIER, out value, fuzzyTest)) {
				return value;
			}

			throw new ApplicationException("Oh no!!");
		}

		private static bool TryFindLineOfSymmetry(string[] strings, int oldValue, int multiplier, out int value, bool fuzzyTest = false)
		{
			value = -1;
			for (int index = 0; index < strings.Length - 1; index++) {
				if (IsSymmetrical(strings, index, strings.Length, out value, fuzzyTest) && (value * multiplier) != oldValue) {
					value *= multiplier;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Checks a set of strings to see if they have symmetry placed between index and index+1
		/// </summary>
		/// <param name="strings"></param>
		/// <param name="index"></param>
		/// <param name="max"></param>
		/// <param name="value"></param>
		/// <param name="fuzzyTest"></param>
		/// <returns></returns>
		private static bool IsSymmetrical(string[] strings, int index, int max, out int value, bool fuzzyTest = false)
		{
			value = 0;
			int possibleSmudgePosition = -1;
			
			if (!TryFuzzyTest(strings[index], strings[index + 1], out int position, fuzzyTest)) {
				return false;
			} else if (fuzzyTest && position >= 0) {
				possibleSmudgePosition = position;
			}

			for (int i = 1; true; i++) {
				if (index + i + 1 >= max || index - i < 0) {
					value = index + 1;
					return true;
				}
				if (!TryFuzzyTest(strings[index - i], strings[index + i + 1], out position, fuzzyTest)) {
					break;
				} else if (fuzzyTest && position >= 0) {
					if (possibleSmudgePosition >= 0) {
						break;
					}
					possibleSmudgePosition = position;
				}
			}
			return false;
		}

		/// <summary>
		/// Compares 2 strings and returns true if they match 
		/// </summary>
		/// <param name="string1"></param>
		/// <param name="string2"></param>
		/// <param name="position">Is set to the position of the difference, or -1 if not found</param>
		/// <param name="fuzzyTest">If true will allow matches that differ by 1 position</param>
		/// <returns>True if the strings are identical or differ in only 1 position and fuzzyTest is true</returns>
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

		public static IEnumerable<Pattern> Parse(string[] s)
		{
			int inputIndex = 0;
			while (inputIndex < s.Length) {
				Pattern pattern = Parse(string.Join(Environment.NewLine, s[inputIndex..]), null);
				yield return pattern;
				inputIndex += pattern.NoOfRows + 1;
			}
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Pattern result)
			=> ISimpleParsable<Pattern>.TryParse(s, provider, out result);
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

