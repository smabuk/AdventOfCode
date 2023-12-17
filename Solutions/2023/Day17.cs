using static Smab.Helpers.ArrayHelpers;
using City      = int[,];
using Direction = (int dX, int dY);

namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 17: Clumsy Crucible
/// https://adventofcode.com/2023/day/17
/// </summary>
[Description("Clumsy Crucible")]
public sealed partial class Day17 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();


	private static int Solution1(string[] input)
	{
		City city = input.SelectMany(i => i.AsDigits<int>()).To2dArray(input[0].Length);
		Point start = new(0, 0);
		Point end = new(city.XMax(), city.YMax());

		return city.LeastHeatLossUsingDijkstras(start, end);
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}

}

file static class Day17Helpers
{
	public static int LeastHeatLossUsingDijkstras(this int[,] grid, Point start, Point end)
	{
		PriorityQueue<Crucible, int> priorityQueue = new();

		Crucible startState = new(new(start, 0), RIGHT, 0); ;
		priorityQueue.Enqueue(startState, 0);

		Dictionary<Crucible, int>   costs       = new() { { startState, 0 } };
		Dictionary<Point, Point> routePoints = new() { { start, start } };

		while (priorityQueue.Count > 0) {
			Crucible current = priorityQueue.Dequeue();

			foreach (Crucible neighbour in GetNeighbours(grid, current)) {
				if (!costs.ContainsKey(neighbour)) {
					int cost = costs[current] + neighbour.Block;
					costs[neighbour] = cost;
					routePoints[neighbour.Block] = current.Block;
					if (neighbour.Block == end) {
						break;
					}
					priorityQueue.Enqueue(neighbour, cost);
				}
			}
		}

		// Check the path
		//Point prev = end;
		//string stringOfRoute = $"({prev.X},{prev.Y})";
		//char[,] route = Create2dArray(grid.ColsCount(), grid.RowsCount(), '.');
		//do {
		//	prev = routePoints[prev];
		//	stringOfRoute = $"({prev.X},{prev.Y}),{stringOfRoute}";
		//	route[prev.X, prev.Y] = 'X';

		//} while (prev != new Point(0, 0));
		//string path = route.PrintAsString();


		return costs.Where(c => c.Key.Block.Index == end).Select(c => c.Value).Min();
	}

	private static IEnumerable<Crucible> GetNeighbours(City grid, Crucible current)
	{
		int maxSteps = 3;
		if (current.Steps < maxSteps - 1) {
			Point nextAhead = current.Block.Index + current.Facing;
			if (grid.IsInBounds(nextAhead)) {
				yield return current with { Block = new(nextAhead, grid[nextAhead.X, nextAhead.Y]), Steps = current.Steps + 1 };
			}
		}

		Direction behind = (current.Facing.dX * -1, current.Facing.dY * -1);
		var neighbours = grid.GetAdjacentCells(current.Block, exclude: [behind, current.Facing]);

		foreach (Cell<int> neighbour in neighbours) {
			Direction newFacing = neighbour.Index - current.Block.Index;
			yield return current with { Block = neighbour, Facing = newFacing, Steps = 0 };
		}
	}

	public record Crucible(Cell<int> Block, Direction Facing, int Steps = 0);
}
