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
		return input
			.AsPoints(GALAXY)
			.ExpandUniverse(scale)
			.ToArray()
			.Combinations(2)
			.Sum(pair => pair.First().ManhattanDistance(pair.Last()));
	}
}

public static class Day11Helpers
{
	public static IEnumerable<Point> ExpandUniverse(this IEnumerable<Point> galaxies, int scale)
	{
		int cols = galaxies.Max(g => g.X);
		int rows = galaxies.Max(g => g.Y);
		int[] newRows    = [.. Enumerable.Range(0, rows).Where(row => !galaxies.Any(g => g.Y == row))];
		int[] newColumns = [.. Enumerable.Range(0, cols).Where(col => !galaxies.Any(g => g.X == col))];

		foreach (Point galaxy in galaxies) {
			int xShift = newColumns.Where(col => col < galaxy.X).Count();
			int yShift = newRows.Where(row => row < galaxy.Y).Count();
			yield return new Point (galaxy.X + (xShift * (scale - 1)), galaxy.Y + (yShift * (scale - 1)));
		}
	}

	public static IEnumerable<Point> AsPoints(this string[] input, char match)
	{
		int rows = input.Length;
		int cols = input[0].Length;

		for (int r = 0; r < rows; r++) {
			for (int c = 0; c < cols; c++) {
				if (input[r][c] == match) {
					yield return new Point(c, r);
				}
			}
		}
	}

	public static long ManhattanDistance(this Point point1, Point point2) => Math.Abs(point1.X - point2.X) + Math.Abs(point1.Y - point2.Y);
}
