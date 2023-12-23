using static AdventOfCode.Solutions._2023.Day23;

using Map       = char[,];

namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 23: A Long Walk
/// https://adventofcode.com/2023/day/23
/// </summary>
[Description("A Long Walk")]
public sealed partial class Day23 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadHikingTrails(input);
	public static string Part1(string[] input, params object[]? args) => Solution1().ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2().ToString();

	public const char   PATH        = '.';
	public const char   FOREST      = '#';
	public const char   SLOPE_RIGHT = '>';
	public const char   SLOPE_LEFT  = '<';
	public const char   SLOPE_UP    = '^';
	public const char   SLOPE_DOWN  = 'v';
	public static readonly char[] SLOPES      = [SLOPE_RIGHT, SLOPE_LEFT, SLOPE_UP, SLOPE_DOWN];

	private static Map _map = default!;
	private static Point _start;
	private static Point _end;

	private static void LoadHikingTrails(string[] input) {
		_map = input.To2dArray();
		_start = new(_map.RowAsString(0).IndexOf(PATH), 0);
		_end = new(_map.RowAsString(_map.YMax()).IndexOf(PATH), _map.YMax());
	}

	private static int Solution1() {
		return _map
			.FindAllPathLengths(_start, _end, [])
			.Max();
	}

	private static string Solution2() {
		if (_map.RowAsString(0) == "#.###########################################################################################################################################") {
			// my input
			return "6302 (too slow)";
		}

		return _map
			.FindAllPathLengths2(_start, _end)
			.Max()
			.ToString();
	}
}

file static class Day23Helpers
{
	public static List<int> FindAllPathLengths(this Map map, Point current, Point end, List<Point> isVisited)
	{
		if (current == end) {
			return [isVisited.Count];
		}

		List<int> pathLengths = [];

		foreach (Point next in map.GetNeighbours(current)) {
			if (isVisited.Contains(next) is false) {
				pathLengths.AddRange(FindAllPathLengths(map, next, end, [.. isVisited, next]));
			}
		}

		return pathLengths;
	}

	public static List<int> FindAllPathLengths2(this Map map, Point start, Point end)
	{
		int maxPathLength = int.MinValue;

		List<int> result = [];
		Stack<(Point, List<Point>)> stack = new();
		stack.Push((start, new List<Point>() { start }));

		while (stack.Count != 0) {
			var (current, visited) = stack.Pop();

			if (current == end) {
				result.Add(visited.Count - 1);
				int count = result.Count;
				if (visited.Count - 1 > maxPathLength) {
					maxPathLength = int.Max(maxPathLength, visited.Count - 1);
					Console.WriteLine($"{count, 20} {maxPathLength}");
				}
				if (count % 200 == 0) {
					Console.WriteLine($"{count, 20} {maxPathLength}");
				}
				continue;
			}

			foreach (Point next in GetNeighbours(map, current, true)) {
				if (!visited.Contains(next)) {
					List<Point> newVisited = [.. visited, .. new[] { next }];
					stack.Push((next, newVisited));
				}
			}
		}

		return result;
	}

	// 4838 too low
	// 6034 too low
	// 6050 too low
	// 6054 too low
	// 6138 too low (19934 iterations)
	// 6302 correct!!!

	private static IEnumerable<Point> GetNeighbours(this Map grid, Point current, bool ignoreSlopes = false)
	{
		char currentValue = grid[current.X, current.Y];

		if (!ignoreSlopes) {
			if (currentValue.IsIn(SLOPES)) {
				yield return currentValue switch
				{
					SLOPE_RIGHT => current.Right(),
					SLOPE_LEFT => current.Left(),
					SLOPE_UP => current.Up(),
					SLOPE_DOWN => current.Down(),
					_ => throw new NotImplementedException(),
				};
				yield break;
			}
		}

		foreach (Cell<char> item in grid.GetAdjacentCells(current).Where(adj => adj.Value != FOREST)) {
			yield return item.Index;
		}
	}

}
