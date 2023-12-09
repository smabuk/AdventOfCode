namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 02: Cube Conundrum
/// https://adventofcode.com/2023/day/02
/// </summary>
[Description("Cube Conundrum")]
public sealed partial class Day02 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadGames(input, args);
	public static string Part1(string[] input, params object[]? args) => Solution1().ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2().ToString();

	private static IEnumerable<Game> _games = [];

	/// <summary>
	/// This solution supports 2 ways of parsing:
	///		Regex
	///		Split
	/// </summary>
	private static void LoadGames(string[] input, object[]? args) {
		string parsingType = GetArgument(args, argumentNumber: 1, defaultResult: "split").ToLowerInvariant();
		_games = parsingType switch
		{
			"split" => input.As<Game>(),
			"regex" => input.As<Game>(provider: ParsingMethod.Regex),
			_ => throw new ArgumentOutOfRangeException(nameof(args), $"That method of parsing [{parsingType}] is not supported."),
		};
	}

	private static int Solution1() {
		return _games
			.Where(game => !game.RevealedCubes
				.Any(cubeSet => cubeSet.Red > 12 || cubeSet.Green > 13 || cubeSet.Blue > 14))
			.Sum(game => game.Id);
	}

	private static int Solution2() => _games.Sum(game => game.MinimumSet.Power);

	private sealed partial record Game(int Id, List<CubesSet> RevealedCubes) : IParsable<Game> {

		public CubesSet MinimumSet { get; } = new(
			RevealedCubes.Max(cubes => cubes.Red),
			RevealedCubes.Max(cubes => cubes.Green),
			RevealedCubes.Max(cubes => cubes.Blue)
			);

		public static Game Parse(string s, IFormatProvider? provider)
		{
			if (provider is ParsingMethod method && method.Type == ParsingMethod.ParsingType.Regex ) {
				return ParseUsingRegex(s);
			}

			string[] SEPS = ["Game", ":", ";"];
			return s.TrimmedSplit(SEPS)
				is [string id, .. string[] cubesSets] ? new(id.As<int>(), [.. cubesSets.As<CubesSet>()]) : throw new InvalidCastException(nameof(s));
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Game result)
			=> ISimpleParsable<Game>.TryParse(s, provider, out result);

		private static Game ParseUsingRegex(string s)
		{
			Match match = GameRegex().Match(s);

			int id = match.As<int>("id");
			IEnumerable<CubesSet> cubesSets = SetsRegex()
				.Matches(match.Groups["sets"].Value)
				.Select(m => m.Value)
				.As<CubesSet>(provider: ParsingMethod.Regex);

			return new(id, [.. cubesSets]);
		}

		[GeneratedRegex(@"Game (?<id>\d+): (?<sets>.*$)")]
		private static partial Regex GameRegex();

		/// <summary>
		///    Looks behind for either the start of the line or a ; followed by a space
		///    Looks ahead for either the end of the line or a ;
		///    Captures everything in-between
		/// </summary>
		/// <returns></returns>
		[GeneratedRegex(@"(?<=^|;\s).*?(?=;|$)")]
		private static partial Regex SetsRegex();
	}

	private sealed partial record CubesSet(int Red, int Green, int Blue) : IParsable<CubesSet> {

		public int Power { get; } = Red * Green * Blue;

		public static CubesSet Parse(string s, IFormatProvider? provider)
		{
			if (provider is ParsingMethod method && method.Type == ParsingMethod.ParsingType.Regex) {
				return ParseUsingRegex(s);
			}

			const char COMMA = ',';
			const char SPACE = ' ';
			char[] SEPS = [COMMA, SPACE];

			List<string> cubes = [.. s.TrimmedSplit(SEPS)];
			return new(GetCount("red"), GetCount("green"), GetCount("blue"));

			int GetCount(string colour)
			{
				int countIndex = cubes.IndexOf(colour) - 1;
				return countIndex >= 0 ? cubes[countIndex].As<int>() : 0;
			}
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out CubesSet result)
			=> ISimpleParsable<CubesSet>.TryParse(s, provider, out result);

		private static CubesSet ParseUsingRegex(string s)
			=> new(	RedRegex()  .Match(s).As<int>("count"),
					GreenRegex().Match(s).As<int>("count"),
					BlueRegex() .Match(s).As<int>("count"));

		[GeneratedRegex(@"(?<count>\d+) (?<colour>red)")]
		private static partial Regex RedRegex();
		[GeneratedRegex(@"(?<count>\d+) (?<colour>green)")]
		private static partial Regex GreenRegex();
		[GeneratedRegex(@"(?<count>\d+) (?<colour>blue)")]
		private static partial Regex BlueRegex();
	}

	public sealed class ParsingMethod(ParsingMethod.ParsingType type) : IFormatProvider
	{
		public ParsingType Type { get; init; } = type;
		public static ParsingMethod Split => new(ParsingType.Split);
		public static ParsingMethod Regex => new(ParsingType.Regex);
		public object? GetFormat(Type? formatType) => formatType == typeof(ParsingMethod) ? this : (object?)null;

		public enum ParsingType
		{
			Split,
			Regex
		}
	}

}
