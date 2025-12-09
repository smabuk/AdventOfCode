using static AdventOfCode.Solutions._2025.Day09;

namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 09: Movie Theater
/// https://adventofcode.com/2025/day/09
/// </summary>
[Description("Movie Theater")]
[GenerateVisualiser]
public partial class Day09 {

	[Init]
	public static void LoadTiles(string[] input) => _redTiles = [.. input.As<Tile>()];
	private static List<Tile> _redTiles = [];

	public static long Part1() => _redTiles.Combinations(2).Max(tiles => tiles.Area());
	public static string Part2() => NO_SOLUTION_WRITTEN_MESSAGE;

	[GenerateIParsable]
	internal sealed partial record Tile(Point Position)
	{
		public static Tile Parse(string s) => new(Point.Parse(s));
	}
}

file static partial class Day09TileExtensions
{
	extension(Tile tile)
	{
		public long X => tile.Position.X;
		public long Y => tile.Position.Y;

		public long Area(Tile tile2)
			=> (Math.Abs(tile.X - tile2.X) + 1) * (Math.Abs(tile.Y - tile2.Y) + 1);
	}

	extension(Tile[] tiles)
	{
		public long Area() => tiles[0].Area(tiles[1]);
	}
}
