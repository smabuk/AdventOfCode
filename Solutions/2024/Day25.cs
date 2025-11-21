namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 25: Code Chronicle
/// https://adventofcode.com/2024/day/25
/// </summary>
[Description("Code Chronicle")]
public partial class Day25 {

	public const char EMPTY = '.';
	public const char FILLED = '#';

	private static List<Schematic> _schematics = [];

	[Init]
	public static void LoadSchematics(string[] input) =>
		_schematics = [
			.. input
				.Chunk(8)
				.Select(chunk => string.Join(Environment.NewLine, chunk))
				.As<Schematic>()
			];

	public static int Part1()
	{
		return _schematics
			.OfType<LockSchematic>()
			.SelectMany(aLock => _schematics
				.OfType<KeySchematic>()
				.Where(aKey => aLock.Heights.Zip(aKey.Heights).All(height => height.First + height.Second <= 5)))
			.Count();
	}

	public static string Part2() => "⭐ CONGRATULATIONS ⭐";


	private abstract record Schematic(int[] Heights) : IParsable<Schematic>
	{
		public static Schematic Parse(string s, IFormatProvider? provider)
		{
			char[,] schematic = s.TrimmedSplit().To2dArray();

			bool isKey = schematic.Row(0).All(c => c == FILLED) || schematic.Row(6).All(c => c == EMPTY);

			return isKey
				? new  KeySchematic([.. Enumerable.Range(0, schematic.ColsCount()).Select(colIndex => schematic.Col(colIndex).Count(c => c == FILLED) - 1)])
				: new LockSchematic([.. Enumerable.Range(0, schematic.ColsCount()).Select(colIndex => schematic.Col(colIndex).Count(c => c == FILLED) - 1)]);
		}

		public static Schematic Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Schematic result)
			=> ISimpleParsable<Schematic>.TryParse(s, provider, out result);
	}

	private sealed record KeySchematic(int[] Heights) : Schematic(Heights);
	private sealed record LockSchematic(int[] Heights) : Schematic(Heights);
}
