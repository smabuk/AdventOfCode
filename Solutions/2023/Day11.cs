using static AdventOfCode.Solutions._2023.Day11;
using static AdventOfCode.Solutions._2023.Day11Helpers;
using Route = System.Collections.Generic.List<Smab.Helpers.Point>;

namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 11: Cosmic Expansion
/// https://adventofcode.com/2023/day/11
/// </summary>
[Description("Cosmic Expansion")]
public sealed partial class Day11 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	public const char EMPTY  = '.';
	public const char GALAXY = '#';


	private static int Solution1(string[] input) {
		char[,] image = input.To2dArray();

		List<Point> galaxies = [..image.ExpandedUniverse()];

		return galaxies
			.Combinations(2)
			.Sum(pair => pair.First().ManhattanDistance(pair.Last()));
	}

	private static string Solution2(string[] input) {
		//List<Instruction> instructions = [.. input.As<Instruction>()];
		return "** Solution not written yet **";
	}
}

public static class Day11Helpers
{
	public static IEnumerable<Point> ExpandedUniverse(this char[,] universe)
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
			yield return new Point (galaxy.X + xShift, galaxy.Y + yShift);
		}
	}

	public static IEnumerable<Cell<T>> Row<T>(this T[,] array, int rowNo)
		=> array.Walk2dArrayWithValues().Where(cell => cell.Index.Y == rowNo);

	public static IEnumerable<Cell<T>> Column<T>(this T[,] array, int columnNo)
		=> array.Walk2dArrayWithValues().Where(cell => cell.Index.X == columnNo);

	public static int FindShortestRoutesFromAToB(Point startingPosition, Point endingPosition, int maxRouteLength)
	{
		List<Route> foundRoutes = [];
		HashSet<Point> visited = [startingPosition];
		Queue<Route> queue = [];
		queue.Enqueue([startingPosition]);
		int shortestRouteLength = maxRouteLength;
		while (queue.Count != 0) {
			Route routeSoFar = queue.Dequeue();
			Point lastPosition = routeSoFar.Last();
			if (lastPosition == endingPosition) {
				if (routeSoFar.Count <= maxRouteLength) {
					foundRoutes.Add(routeSoFar);
				}
			} else if (routeSoFar.Count < maxRouteLength) {
				IEnumerable<Point> nextSteps = lastPosition
					.Adjacent()
					.Where(p => !visited.Contains(p));
				foreach (Point step in nextSteps) {
					queue.Enqueue([.. routeSoFar, step]);
					_ = visited.Add(step);
				}
			}
		}

		return foundRoutes[0].Count;
	}
	public static int ManhattanDistance(this Point point1, Point point2) => Math.Abs(point1.X - point2.X) + Math.Abs(point1.Y - point2.Y);


}
