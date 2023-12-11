using static AdventOfCode.Solutions._2023.Day11;

namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 11: Cosmic Expansion
/// https://adventofcode.com/2023/day/11
/// </summary>
[Description("Cosmic Expansion")]
public sealed partial class Day11 {

	public static string Part1(string[] input, params object[]? args) => Solution(input, 2).ToString();
	public static string Part2(string[] input, params object[]? args)
	{
		int scale = GetArgument(args, argumentNumber: 1, defaultResult: 1_000_000);
		return Solution(input, scale).ToString();
	}

	public const char EMPTY  = '.';
	public const char GALAXY = '#';

	private static long Solution(string[] input, int scale) {
		return input.To2dArray()
			.ExpandedUniverse(scale).ToHashSet()
			.Combinations(2)
			.Sum(pair => pair.First().ManhattanDistance(pair.Last()));
	}
}

public static class Day11Helpers
{
	public static IEnumerable<Point> ExpandedUniverse(this char[,] universe, int scale)
	{
		HashSet<int> newRows = [.. Enumerable
			.Range(0, universe.NoOfRows())
			.Where(row => universe.Row(row).All(space => space.Value == EMPTY))];

		HashSet<int> newColumns = [.. Enumerable
			.Range(0, universe.NoOfColumns())
			.Where(col => universe.Column(col).All(space => space.Value == EMPTY))];

		foreach (Cell<char> galaxy in universe.Walk2dArrayWithValues().Where(space => space.Value == GALAXY)) {
			int xShift = newColumns.Where(col => col < galaxy.X).Count();
			int yShift = newRows.Where(row => row < galaxy.Y).Count();
			yield return new Point (galaxy.X + (xShift * (scale - 1)), galaxy.Y + (yShift * (scale - 1)));
		}
	}

	public static IEnumerable<Cell<T>> Row<T>(this T[,] array, int rowNo)
		=> array.Walk2dArrayWithValues().Where(cell => cell.Index.Y == rowNo);

	public static IEnumerable<Cell<T>> Column<T>(this T[,] array, int columnNo)
		=> array.Walk2dArrayWithValues().Where(cell => cell.Index.X == columnNo);

	public static long ManhattanDistance(this Point point1, Point point2) => Math.Abs(point1.X - point2.X) + Math.Abs(point1.Y - point2.Y);
}
