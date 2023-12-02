namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 02: Cube Conundrum
/// https://adventofcode.com/2023/day/02
/// </summary>
[Description("Cube Conundrum")]
public sealed partial class Day02 {

	[Init]
	public static    void Init(string[] input, params object[]? _) => LoadGames(input);
	public static string Part1(string[] input, params object[]? _) => Solution1().ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2().ToString();

	private static IEnumerable<Game> _games = [];

	private static void LoadGames(string[] input) {
		_games = input.Select(Game.Parse);
	}

	private static int Solution1() {
		GameCubes startingCubes = new(12, 13, 14);
		List<int> possibleGames = [.. _games.Select(g => g.Id)];

		foreach (Game game in _games) {
			foreach ((int red, int green, int blue) in game.ShownCubes) {
				if (red > startingCubes.Red || green > startingCubes.Green || blue > startingCubes.Blue) {
					_ = possibleGames.Remove(game.Id);
					break;
				}
			}
		}

		return possibleGames.Sum();
	}

	private static int Solution2() => _games
		.Select(game => game.MinimumSet.Power)
		.Sum();

	private record Game(int Id, List<GameCubes> ShownCubes) : IParsable<Game> {

		public GameCubes MinimumSet { get; } = new(
			ShownCubes.Max(gc => gc.Red),
			ShownCubes.Max(gc => gc.Green),
			ShownCubes.Max(gc => gc.Blue)
			);

		public static Game Parse(string s)
		{
			string[] tokens1 = s.Split(':', StringSplitOptions.TrimEntries);
			int id = int.Parse(tokens1[0][5..]);
			List<GameCubes> shownCubes = tokens1[1]
				.Split(';', StringSplitOptions.TrimEntries)
				.Select(GameCubes.Parse)
				.ToList();

			return new(id, shownCubes);
		}

		public static Game Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Game result) => throw new NotImplementedException();
	}

	private record GameCubes(int Red, int Green, int Blue) : IParsable<GameCubes> {

		public int Power { get; } = Red * Green * Blue;

		public static GameCubes Parse(string s)
		{
			int red = 0;
			int green = 0;
			int blue = 0;
			string[] tokens1 = s.Split(',', StringSplitOptions.TrimEntries);
			foreach (string colouredCubes in tokens1) {
				string[] tokens2 = colouredCubes.Split(' ', StringSplitOptions.TrimEntries);
				int count = int.Parse(tokens2[0]);
				string colour = tokens2[1];
				switch (colour) {
					case "red":
						red = count;
						break;
					case "green":
						green = count;
						break;
					case "blue":
						blue = count;
						break;
					default:
						break;
				}
			}

			return new(red, green, blue);
		}
		public static GameCubes Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out GameCubes result) => throw new NotImplementedException();
	}
}
