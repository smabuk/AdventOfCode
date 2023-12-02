using System.Collections.Generic;

namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 02: Cube Conundrum
/// https://adventofcode.com/2023/day/02
/// </summary>
[Description("Cube Conundrum")]
public sealed partial class Day02 {

	[Init]
	public static   void  Init(string[] input, params object[]? _) => LoadGames(input);
	public static string Part1(string[] input, params object[]? _) => Solution1().ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2().ToString();

	private static IEnumerable<Game> _games = [];

	private static void LoadGames(string[] input) {
		_games = input.Select(Game.Parse);
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

	private record Game(int Id, List<CubesSet> RevealedCubes) : IParsable<Game> {

		public CubesSet MinimumSet { get; } = new(
			RevealedCubes.Max(cubes => cubes.Red),
			RevealedCubes.Max(cubes => cubes.Green),
			RevealedCubes.Max(cubes => cubes.Blue)
			);

		public static Game Parse(string s)
		{
			const int ID_TOKEN       = 0;
			const int CUBESETS_TOKEN = 1;
			const int ID_OFFSET      = 5;

			string[] tokens = s.Split(':', StringSplitOptions.TrimEntries);
			int id = tokens[ID_TOKEN][ID_OFFSET..].AsInt();
			List<CubesSet> revealedCubesSets = tokens[CUBESETS_TOKEN]
				.Split(';', StringSplitOptions.TrimEntries)
				.Select(CubesSet.Parse)
				.ToList();

			return new(id, revealedCubesSets);
		}

		public static Game Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Game result) => throw new NotImplementedException();
	}

	private record CubesSet(int Red, int Green, int Blue) : IParsable<CubesSet> {

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
		public static CubesSet Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out CubesSet result) => throw new NotImplementedException();
	}
}
