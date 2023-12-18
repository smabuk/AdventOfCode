using static AdventOfCode.Solutions._2023.Day17;
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
		Crucible start = new(new (new Point(0,0), 0), RIGHT, 0);
		Point end = new(city.XMax(), city.YMax());

		return city.LeastHeatLossUsingDijkstras(start, end);
	}

	private static int Solution2(string[] input) {
		City city = input.SelectMany(i => i.AsDigits<int>()).To2dArray(input[0].Length);
		UltraCrucible start1 = new(new(new Point(0, 0), 0), RIGHT, 0);
		UltraCrucible start2 = new(new(new Point(0, 0), 0), DOWN,  0);
		Point end = new(city.XMax(), city.YMax());

		return int.Min(city.LeastHeatLossUsingDijkstras(start1, end)
					 , city.LeastHeatLossUsingDijkstras(start2, end));
	}

	public record Crucible(Cell<int> Block, Direction Facing, int Steps = 0);
	public record UltraCrucible(Cell<int> Block, Direction Facing, int Steps = 0) : Crucible(Block, Facing, Steps);
}

file static class Day17Helpers
{
	public static int LeastHeatLossUsingDijkstras(this City city, Crucible start, Point end)
	{
		PriorityQueue<Crucible, int> priorityQueue = new();

		priorityQueue.Enqueue(start, 0);

		Dictionary<(Point Position, Direction Facing, int Steps), int> costs       = new() { { (start.Block.Index, start.Facing, 0), 0 } };

		while (priorityQueue.Count > 0) {
			Crucible current = priorityQueue.Dequeue();

			foreach (Crucible neighbour in GetNeighbours(city, current)) {
				if (!costs.ContainsKey((neighbour.Block.Index, neighbour.Facing, neighbour.Steps))) {
					int cost = costs[(current.Block.Index, current.Facing, current.Steps)] + neighbour.Block;
					costs[(neighbour.Block.Index, neighbour.Facing, neighbour.Steps)] = cost;
					if (neighbour.Block == end) {
						break;
					}
					priorityQueue.Enqueue(neighbour, cost);
				}
			}
		}

		return start switch
		{
			UltraCrucible => costs.Where(c => c.Key.Position == end && c.Key.Steps >= 3).Select(c => c.Value).Min(),
			            _ => costs.Where(c => c.Key.Position == end).Select(c => c.Value).Min()
		};
	}

	private static IEnumerable<Crucible> GetNeighbours(City grid, Crucible current)
	{
		int maxSteps = 3;
		int minSteps = 0;
		if (current is UltraCrucible) {
			maxSteps = 10;
			minSteps = 4 - 1;
		}

		if (current.Steps < maxSteps - 1) {
			Point ahead = current.Block.Index + current.Facing;
			if (grid.IsInBounds(ahead)) {
				yield return current with { Block = new(ahead, grid[ahead.X, ahead.Y]), Steps = current.Steps + 1 };
			}
		}

		if (current.Steps >= minSteps) {
			Direction side1 = current.Facing.Transpose();
			Direction side2 = (side1.dX * -1, side1.dY * -1);

			Point[] turns = [current.Block.Index + side1, current.Block.Index + side2];
			foreach (Point neighbour in turns) {
				if (grid.IsInBounds(neighbour)) {
					Direction newFacing = neighbour - current.Block.Index;
					Cell<int> block = new(neighbour, grid[neighbour.X, neighbour.Y]);
					yield return current with { Block = block, Facing = newFacing, Steps = 0 };
				}
			}
		}
	}
}
