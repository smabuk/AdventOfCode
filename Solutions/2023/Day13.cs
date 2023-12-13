namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 13: Point of Incidence
/// https://adventofcode.com/2023/day/13
/// </summary>
[Description("Point of Incidence")]
public sealed partial class Day13 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadPatterns(input);
	public static string Part1(string[] input, params object[]? args) => Solution1().ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static readonly List<Pattern> _patterns = [];

	private static void LoadPatterns(string[] input) {
		int inputIndex = 0;
		while (inputIndex < input.Length) {
			Pattern pattern = string.Join(Environment.NewLine, input[inputIndex..]).As<Pattern>();
			_patterns.Add(pattern);
			inputIndex += pattern.NoOfRows + 1;
		}
	}

	private static int Solution1() => _patterns.Sum(pattern => pattern.Value);

	private static string Solution2(string[] input) {
		List<Pattern> instructions = [.. input.As<Pattern>()];
		return "** Solution not written yet **";
	}

	private sealed record Pattern(List<Point> Mirrors) : IParsable<Pattern> {

		public int NoOfColumns = Mirrors.Max(mirror => mirror.X) + 1;
		public int NoOfRows    = Mirrors.Max(mirror => mirror.Y) + 1;

		private LineOfSymmetry lineOfSymmetry = LineOfSymmetry.None;
		private int left  = 0;
		private int above = 0;

		public int Value
		{
			get
				{
					if (lineOfSymmetry is LineOfSymmetry.None) {
						CalculateLineOfSymmetry();
					}
					return left + (100 * above);
				}
		}

		private void CalculateLineOfSymmetry()
		{
			string[] columnStrings = [..Enumerable.Range(0, NoOfColumns)
				.Select(x => Mirrors.Where(m => m.X == x).Select(m => m.Y))
				.Select(v => v.ValuesAsString())];

			for (int col = 0; col < NoOfColumns - 1; col++) {
				if (columnStrings[col] != columnStrings[col+1]) {
					continue;
				}
				for (int i = 1; true; i++) {
					if (col + i + 1 >= NoOfColumns || col - i < 0) {
						lineOfSymmetry = LineOfSymmetry.Vertical;
						left = col + 1;
						return;
					}
					if (columnStrings[col - i] != columnStrings[col + i + 1]) {
						break;
					}
				}
			}

			string[] rowStrings = [..Enumerable.Range(0, NoOfRows)
				.Select(y => Mirrors.Where(m => m.Y == y).Select(m => m.X))
				.Select(v => v.ValuesAsString())];


			for (int row = 0; row < NoOfRows - 1; row++) {
				if (rowStrings[row] != rowStrings[row + 1]) {
					continue;
				}
				for (int i = 1; true; i++) {
					if (row + i + 1 >= NoOfRows || row - i < 0) {
						lineOfSymmetry = LineOfSymmetry.Horizontal;
						above = row + 1;
						return;
					}
					if (rowStrings[row - i] != rowStrings[row + i + 1]) {
						break;
					}
				}
			}

			throw new ApplicationException("Oh no!!");
		}

		private static bool IsNotABlankLine(string s) => !string.IsNullOrWhiteSpace(s);

		public static Pattern Parse(string s, IFormatProvider? provider)
		{
			const char MIRROR = '#';
			
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
	public static string ValuesAsString(this IEnumerable<int> values)
	{
		return string.Join(",", values.Select(value => value.ToString()));
	}
}

