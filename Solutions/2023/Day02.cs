namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 02: Cube Conundrum
/// https://adventofcode.com/2023/day/02
/// </summary>
[Description("Cube Conundrum")]
public sealed partial class Day02 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadGames(input, args);
	public static string Part1(string[] input, params object[]? _) => Solution1().ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2().ToString();

	private static IEnumerable<Game> _games = [];

	/// <summary>
	/// This solution supports 2 ways of parsing:
	///		Regex
	///		Split
	/// </summary>
	private static void LoadGames(string[] input, object[]? args) {
		_games = GetArgument(args, 1, "split").ToLowerInvariant() switch
		{
			"split" => input.Select(Game.Parse),
			"regex" => input.Select(Game.ParseUsingRegex),
			_ => throw new ArgumentOutOfRangeException("That method of parsing is not supported."),
		};
	}

	private static int Solution1() {
		(int red, int green, int blue) = (12, 13, 14);

		return _games
			.Where(game => !game.RevealedCubes
				.Where(cubeSet => cubeSet.Red > red || cubeSet.Green > green || cubeSet.Blue > blue)
				.Any())
			.Sum(game => game.Id);
	}

	private static int Solution2() => _games.Sum(game => game.MinimumSet.Power);

	private partial record Game(int Id, List<CubesSet> RevealedCubes) : IParsable<Game> {

		public CubesSet MinimumSet { get; } = new(
			RevealedCubes.Max(cubes => cubes.Red),
			RevealedCubes.Max(cubes => cubes.Green),
			RevealedCubes.Max(cubes => cubes.Blue)
			);

		public static Game Parse(string s)
		{
			const int ID        = 0;
			const int CUBESETS  = 1;
			const int ID_OFFSET = 5;

			char[] COLON_AND_SEMICOLON = [':', ';'];
			string[] tokens = s.Split(COLON_AND_SEMICOLON, StringSplitOptions.TrimEntries);

			return new(tokens[ID][ID_OFFSET..].AsInt(), [.. tokens[CUBESETS..].Select(CubesSet.Parse)]);
		}

		public static Game ParseUsingRegex(string s)
		{
			Match match = GameRegex().Match(s);

			int id = match.GroupAsInt("id");
			IEnumerable<CubesSet> cubesSets = SetsRegex()
				.Matches(match.Groups["sets"].Value)
				.Select(m => m.Value)
				.Select(CubesSet.ParseUsingRegex);

			return new(id, [.. cubesSets]);
		}

		[GeneratedRegex("""Game (?<id>\d+): (?<sets>.*$)""")]
		private static partial Regex GameRegex();

		/// <summary>
		///    Looks behind for either the start of the line or a ; followed by a space
		///    Looks ahead for either the end of the line or a ;
		///    Captures everything in-between
		/// </summary>
		/// <returns></returns>
		[GeneratedRegex("""(?<=^|;\s).*?(?=;|$)""")]
		private static partial Regex SetsRegex();

		public static Game Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Game result) => throw new NotImplementedException();
	}

	private partial record CubesSet(int Red, int Green, int Blue) : IParsable<CubesSet> {

		public int Power { get; } = Red * Green * Blue;

		public static CubesSet Parse(string s)
		{
			const int COUNT_TOKEN  = 0;
			const int COLOUR_TOKEN = 1;

			Dictionary<string, int> cubes = s
				.Split(',', StringSplitOptions.TrimEntries)
				.Select(countsAndColours => countsAndColours.Split(' '))
				.ToDictionary(countAndColour => countAndColour[COLOUR_TOKEN], countAndColour => countAndColour[COUNT_TOKEN].AsInt());

			return new(cubes.GetValueOrDefault("red"), cubes.GetValueOrDefault("green"), cubes.GetValueOrDefault("blue"));
		}

		public static CubesSet ParseUsingRegex(string s)
		{
			Dictionary<string, int> cubes = CountAndColourRegex()
				.Matches(s)
				.ToDictionary(match => match.Groups["colour"].Value, match => match.GroupAsInt("count"));

			return new(cubes.GetValueOrDefault("red"), cubes.GetValueOrDefault("green"), cubes.GetValueOrDefault("blue"));
		}

		[GeneratedRegex("""(?<count>\d+) (?<colour>red|green|blue);?""")]
		private static partial Regex CountAndColourRegex();

		public static CubesSet Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out CubesSet result) => throw new NotImplementedException();
	}
}
