using static AdventOfCode.Solutions._2023.Day23;

namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 23: A Long Walk
/// https://adventofcode.com/2023/day/23
/// </summary>
[Description("A Long Walk")]
public sealed partial class Day23 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadMap(input);
	public static string Part1(string[] input, params object[]? args) => Solution(1).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution(2).ToString();

	public const char PATH        = '.';
	public const char FOREST      = '#';
	public const char SLOPE_RIGHT = '>';
	public const char SLOPE_LEFT  = '<';
	public const char SLOPE_UP    = '^';
	public const char SLOPE_DOWN  = 'v';
	public static readonly char[] SLOPES = [SLOPE_RIGHT, SLOPE_LEFT, SLOPE_UP, SLOPE_DOWN];

	public static char[,] _map = default!;
	public static Point _start;
	public static Point _end;

	private static void LoadMap(string[] input) {
		_map   = input.To2dArray();
		_start = new(_map.RowAsString(0).IndexOf(PATH), 0);
		_end   = new(_map.RowAsString(_map.YMax()).IndexOf(PATH), _map.YMax());
	}

	private static int Solution(int part)
	{
		return
			_map
			.IdentifyJunctions([_start, _end])
			.BuildGraph(part)
			.FindMaxSteps_DepthFirstSearch(_start, []);
	}

}

file static class Day23Helpers
{
	public static List<Point> IdentifyJunctions(this char[,] map, List<Point> initialPoints)
	{
		List<Point> points = [.. initialPoints];

		for (int y = 0; y < map.RowsCount(); y++) {
			for (int x = 0; x < map.ColsCount(); x++) {
				Point current = new(x, y);
				if (map[current.X, current.Y] == FOREST) {
					continue;
				}

				if (map.GetAdjacentCells(current).Where(adj => adj.Value != FOREST).Count() >= 3) {
					points.Add(current);
				}
			}
		}

		return points;
	}

	public static Dictionary<Point, Dictionary<Point, int>> BuildGraph(this List<Point> points, int part = 1)
	{
		Dictionary<Point, Dictionary<Point, int>> graph = points.ToDictionary(pt => pt, pt => new Dictionary<Point, int>());

		foreach (Point current in points) {
			Stack<(Point, int)> stack = new();
			HashSet<Point> seen = [];

			stack.Push((current, 0));
			_ = seen.Add(current);

			while (stack.Count > 0) {
				(Point point, int steps) = stack.Pop();

				if (steps != 0 && points.Contains(point)) {
					graph[current][point] = steps;
					continue;
				}

				foreach (Point neighbour in _map.GetNeighbours(point, part)) {
					if (seen.DoesNotContain(neighbour)) {
						stack.Push((neighbour, steps + 1));
						_ = seen.Add(neighbour);
					}
				}
			}
		}

		return graph;
	}

	public static int FindMaxSteps_DepthFirstSearch(this Dictionary<Point, Dictionary<Point, int>> graph, Point point, HashSet<Point> visited)
	{
		if (point == _end) {
			return 0;
		}

		int maxSteps = int.MinValue;

		_ = visited.Add(point);
		foreach (KeyValuePair<Point, int> next in graph[point]) {
			if (visited.DoesNotContain(next.Key)) {
				maxSteps = Math.Max(maxSteps, FindMaxSteps_DepthFirstSearch(graph, next.Key, visited) + graph[point][next.Key]);
			}
		}
		_ = visited.Remove(point);

		return maxSteps;
	}

	private static IEnumerable<Point> GetNeighbours(this char[,] _map, Point current, int part = 1)
	{
		char currentValue = _map[current.X, current.Y];

		if (part == 1 && currentValue.IsIn(SLOPES)) {
			yield return currentValue switch
			{
				SLOPE_RIGHT => current.Right(),
				SLOPE_LEFT  => current.Left(),
				SLOPE_UP    => current.Up(),
				SLOPE_DOWN  => current.Down(),
				_ => throw new NotImplementedException(),
			};
			yield break;
		}

		foreach (Cell<char> item in _map.GetAdjacentCells(current).Where(adj => adj.Value != FOREST)) {
			yield return item.Index;
		}
	}

	//public static List<int> FindAllPathLengths(this char[,] map, Point current, Point end, List<Point> isVisited)
	//{
	//	if (current == end) {
	//		return [isVisited.Count];
	//	}

	//	List<int> pathLengths = [];

	//	foreach (Point neighbour in map.GetNeighbours(current)) {
	//		if (isVisited.Contains(neighbour) is false) {
	//			pathLengths.AddRange(FindAllPathLengths(map, neighbour, end, [.. isVisited, neighbour]));
	//		}
	//	}

	//	return pathLengths;
	//}


}
