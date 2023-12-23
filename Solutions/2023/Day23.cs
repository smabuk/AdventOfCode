using static AdventOfCode.Solutions._2023.Day23;

using Map       = char[,];
using Direction = (int dX, int dY);
using System.Numerics;

namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 23: A Long Walk
/// https://adventofcode.com/2023/day/23
/// </summary>
[Description("A Long Walk")]
public sealed partial class Day23 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadHikingTrails(input);
	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	public const char   PATH        = '.';
	public const char   FOREST      = '#';
	public const char   SLOPE_RIGHT = '>';
	public const char   SLOPE_LEFT  = '<';
	public const char   SLOPE_UP    = '^';
	public const char   SLOPE_DOWN  = 'v';
	public static readonly char[] SLOPES      = [SLOPE_RIGHT, SLOPE_LEFT, SLOPE_UP, SLOPE_DOWN];

	private static Map _map = default!;

	private static void LoadHikingTrails(string[] input) {
		_map = input.To2dArray();
	}

	private static int Solution1(string[] input) {
		Point start = new(_map.RowAsString(0).IndexOf(PATH), 0);
		Point end   = new(_map.RowAsString(_map.YMax()).IndexOf(PATH), _map.YMax());

		List<int> pathLengths = _map.FindAllPathLengths(start, end, []);

		int longestPath = pathLengths.Max();
		return longestPath;
	}

	private static int Solution2(string[] input) {
		Point start = new(_map.RowAsString(0).IndexOf(PATH), 0);
		Point end = new(_map.RowAsString(_map.YMax()).IndexOf(PATH), _map.YMax());

		List<int> pathLengths = _map.FindAllPathLengths(start, end, []);

		int longestPath = pathLengths.Max();
		return longestPath;
	}
}

file static class Day23Helpers
{
	//public static Dictionary<Point, int> Dijkstras(this Map grid, Point start, Point end)
	//{
	//	int STEP_VALUE = -1;
	//	int maxLength = int.MinValue;
	//	PriorityQueue<Point, int> priorityQueue = new();
	//	priorityQueue.Enqueue(start, 0);
	//	string path = $"{start}";
	//	Dictionary<Point, int> costs = new() { { start, 0 } };

	//	while (priorityQueue.Count > 0) {
	//		Point point = priorityQueue.Dequeue();

	//		foreach ((int x, int y) in GetNeighbours(grid, point)) {
	//			Cell<int> neighbour = new(x, y, STEP_VALUE);
	//			if (!costs.ContainsKey(neighbour.Index)) {
	//				int cost = costs[point] + STEP_VALUE;
	//				costs[neighbour.Index] = cost;
	//				if (neighbour.Index == end) {
	//					maxLength = int.Max(maxLength, int.Abs(cost));
	//					break;
	//				}
	//				priorityQueue.Enqueue(neighbour, cost);
	//			}
	//		}
	//	}

	//	return costs;
	//}

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

	public static List<int> FindAllPathLengths(this Map map, Point current, Point end, List<Point> isVisited, bool ignoreSlopes = false)
	{
		if (current == end) {
			return [isVisited.Count];
		}

		List<int> pathLengths = [];

		foreach (Point next in map.GetNeighbours(current)) {
			if (isVisited.Contains(next) is false) {
				pathLengths.AddRange(FindAllPathLengths(map, next, end, [.. isVisited, next], ignoreSlopes));
			}
		}

		return pathLengths;
	}

	//public static int FindAllUsingDijkstras(this Map map, Point start, Point end)
	//{
	//	PriorityQueue<Point, int> priorityQueue = new();

	//	priorityQueue.Enqueue(start, 0);

	//	Dictionary<(Point Position, int Steps), int> costs = new() { { (start, 0), 0 } };

	//	while (priorityQueue.Count > 0) {
	//		Point current = priorityQueue.Dequeue();

	//		foreach (Point neighbour in GetNeighbours(map, current)) {
	//			if (!costs.ContainsKey((neighbour, neighbour.Steps))) {
	//				int cost = costs[(current.Block.Index,current.Steps)] + neighbour.Block;
	//				costs[(neighbour.Block.Index, neighbour.Facing, neighbour.Steps)] = cost;
	//				if (neighbour.Block == end) {
	//					break;
	//				}
	//				priorityQueue.Enqueue(neighbour, cost);
	//			}
	//		}
	//	}

	//	return start switch
	//	{
	//		//UltraCrucible => costs.Where(c => c.Key.Position == end && c.Key.Steps >= 3).Select(c => c.Value).Min(),
	//		_ => costs.Where(c => c.Key.Position == end).Select(c => c.Value).Min()
	//	};
	//}

	//private static IEnumerable<Point> GetNeighbours(Map grid, Point current)
	//{
	//	int maxSteps = 3;
	//	int minSteps = 0;

	//	if (current.Steps < maxSteps - 1) {
	//		Point ahead = current.Block.Index + current.Facing;
	//		if (grid.IsInBounds(ahead)) {
	//			yield return current with { Block = new(ahead, grid[ahead.X, ahead.Y]), Steps = current.Steps + 1 };
	//		}
	//	}

	//	if (current.Steps >= minSteps) {
	//		Direction side1 = current.Facing.Transpose();
	//		Direction side2 = (side1.dX * -1, side1.dY * -1);

	//		Point[] turns = [current.Block.Index + side1, current.Block.Index + side2];
	//		foreach (Point neighbour in turns) {
	//			if (grid.IsInBounds(neighbour)) {
	//				Direction newFacing = neighbour - current.Block.Index;
	//				Cell<int> block = new(neighbour, grid[neighbour.X, neighbour.Y]);
	//				yield return current with { Block = block, Facing = newFacing, Steps = 0 };
	//			}
	//		}
	//	}
	//}
}
