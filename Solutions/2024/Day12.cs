using static AdventOfCode.Solutions._2024.Day12;

namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 12: Garden Groups
/// https://adventofcode.com/2024/day/12
/// </summary>
[Description("Garden Groups")]
public sealed partial class Day12 {

	private static char[,]      _farm    = default!;
	private static List<Region> _regions = [];

	[Init]
	public static void LoadFarm(string[] input)
	{
		_farm    = input.To2dArray();
		_regions = [.. _farm.FindRegions()];
	}

	public static int Part1(string[] _) => _regions.Sum(region => region.Price(_farm));
	public static int Part2(string[] _) => _regions.Sum(region => region.BulkDiscountPrice(_farm));


	internal record Region(char PlantType, List<Point> Plots);
	internal record Edge(Point Plot, Direction Direction);
}

file static partial class Day12Extensions
{
	public static int Price(this Region r, char[,] farm) => r.Area() * r.Perimeter(farm);
	public static int BulkDiscountPrice(this Region r, char[,] farm) => r.Area() * r.SidesCount(farm);

	private static int Area(this Region r) => r.Plots.Count;
	private static int Perimeter(this Region region, char[,] farm)
	{
		int perimeterCount = 0;
		foreach (Point plot in region.Plots) {
			List<Cell<char>> adjacentPlots = [.. farm.GetAdjacentCells(plot)];

			perimeterCount += 4 - adjacentPlots.Count;

			foreach (Cell<char> adjacentPlot in adjacentPlots) {
				if (adjacentPlot.Value != region.PlantType) {
					perimeterCount++;
				}
			}
		}

		return perimeterCount;
	}

	private static int SidesCount(this Region region, char[,] farm)
	{
		HashSet<Edge> edges = [.. region.FindEdges(farm)];

		int sidesCount = 0;
		while (edges.Count > 0) {
			sidesCount++;

			Edge edge = edges.First();
			_ = edges.Remove(edge);

			Direction checkDirection = edge.Direction switch
			{
				Direction.North or Direction.South => Direction.East,
				Direction.East  or Direction.West  => Direction.South,
				_ => throw new NotImplementedException(),
			};

			// Look both ways along the side
			Point[] deltas =[new Point(checkDirection.Delta()), new Point(checkDirection.Delta()) * -1];

			foreach (Point delta in deltas) {
				bool keepGoing = true;
				int n = 1;
				while (keepGoing) {
					keepGoing = false;

					Edge nextEdge = edge with { Plot = edge.Plot + (delta * n) };
					if (edges.Contains(nextEdge)) {
						_ = edges.Remove(nextEdge);
						keepGoing = true;
					}

					n++;
				}
			}
		}

		return sidesCount;
	}

	/// <summary>
	/// Returns the outside edges of each individual plot
	/// </summary>
	/// <param name="region"></param>
	/// <param name="farm"></param>
	/// <returns></returns>
	private static IEnumerable<Edge> FindEdges(this Region region, char[,] farm)
	{
		foreach (Point plot in region.Plots) {
			List<Cell<char>> adjacents = [.. farm.GetAdjacentCells(plot)];
			foreach (Direction direction in Directions.AllDirections) {
				Point next = plot + direction.Delta();
				if (farm.TryGetValue(next.X, next.Y, out char value)) {
					if (value != region.PlantType) {
						yield return new(next, direction);
					}
				} else {
					yield return new(next, direction);
				}
			}
		}
	}

	public static IEnumerable<Region> FindRegions(this char[,] farm)
	{
		HashSet<Point> visited = [];

		foreach (Cell<char> plot in farm.ForEachCell()) {
			if (visited.DoesNotContain(plot)) {
				yield return farm.FindRegion(plot, visited);
			}
		}
	}

	private static Region FindRegion(this char[,] farm, Cell<char> initialPlot, HashSet<Point> visited)
	{
		Queue<Point> plotQueue = [];

		plotQueue.Enqueue(initialPlot);
		_ = visited.Add(initialPlot);

		List<Point> plots = [];
		while (plotQueue.Count > 0) {
			Point plot = plotQueue.Dequeue();
			plots.Add(plot);

			foreach (Cell<char> adjacentPlot in farm.GetAdjacentCells(plot)) {
				if (visited.DoesNotContain(adjacentPlot) && adjacentPlot.Value == initialPlot.Value) {
					plotQueue.Enqueue(adjacentPlot.Index);
					_ = visited.Add(adjacentPlot.Index);
				}
			}
		}

		return new(initialPlot.Value, plots);
	}
}
