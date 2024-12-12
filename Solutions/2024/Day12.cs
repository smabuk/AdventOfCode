namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 12: Garden Groups
/// https://adventofcode.com/2024/day/12
/// </summary>
[Description("Garden Groups")]
public static partial class Day12 {

	private static char[,] _farm = default!;
	private static List<Region> _regions = [];

	[Init]
	public static void LoadFarm(string[] input)
	{
		_farm = input.To2dArray();
		_regions = [.. _farm.FindRegions()];
	}

	public static int Part1(string[] _) => _regions.Sum(region => region.RegionPrice(_farm));
	public static int Part2(string[] _) => _regions.Sum(region => region.RegionBulkDiscountPrice(_farm));


	private static IEnumerable<Region> FindRegions(this char[,] farm)
	{
		HashSet<Point> visited = [];

		foreach (Cell<char> plot in farm.ForEachCell()) {
			if (visited.Contains(plot)) { continue; }
			Region region = farm.FindRegion(plot, visited);
			visited = [.. visited, .. region.Plots];
			yield return region;
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



	private static int RegionPerimeter(this Region region, char[,] farm)
	{
		int perimeterCount = 0;
		foreach (Point plot in region.Plots) {
			List<Cell<char>> adjacents = [.. farm.GetAdjacentCells(plot)];
			perimeterCount += 4 - adjacents.Count;
			foreach (Cell<char> adjacentPlot in adjacents) {
				if (adjacentPlot.Value != region.Type) {
					perimeterCount++;
				}
			}
		}

		return perimeterCount;
	}

	private static int RegionSidesCount(this Region region, char[,] farm)
	{
		HashSet<Edge> edges = [.. region.RegionEdges(farm)];

		int sidesCount = 0;
		while (edges.Count > 0) {
			sidesCount++;

			Edge edge = edges.First();
			_ = edges.Remove(edge);

			Direction checkDirection = edge.Direction switch {
				Direction.North or Direction.South => Direction.East,
				Direction.East  or Direction.West  => Direction.South,
				_ => throw new NotImplementedException(),
			};

			bool keepGoing      = true;
			bool keepGoingPlus  = true;
			bool keepGoingMinus = true;

			for (int n = 1; keepGoing; n++) {
				keepGoing = false;
				
				Point delta = new(checkDirection.Delta());

				Edge nextEdge = edge with { Plot = edge.Plot + (delta * n) };
				if (keepGoingPlus && edges.Contains(nextEdge)) {
					_ = edges.Remove(nextEdge);
					keepGoing = true;
				} else {
					keepGoingPlus = false;
				}

				nextEdge = edge with { Plot = edge.Plot - (delta * n) };
				if (keepGoingMinus && edges.Contains(nextEdge)) {
					_ = edges.Remove(nextEdge);
					keepGoing = true;
				} else {
					keepGoingMinus = false;
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
	private static IEnumerable<Edge> RegionEdges(this Region region, char[,] farm)
	{
		foreach (Point plot in region.Plots) {
			List<Cell<char>> adjacents = [.. farm.GetAdjacentCells(plot)];
			foreach (Direction direction in Directions.AllDirections) {
				Point next = plot + direction.Delta();
				if (farm.TryGetValue(next.X, next.Y, out char value)) {
					if (value != region.Type) {
						yield return new(next, direction);
					}
				} else {
					yield return new(next, direction);
				}
			}
		}
	}

	private static int RegionBulkDiscountPrice(this Region r, char[,] farm) => r.RegionArea() * r.RegionSidesCount(farm);
	private static int RegionPrice(this Region r, char[,] farm) => r.RegionArea() * r.RegionPerimeter(farm);
	private static int RegionArea(this Region r) => r.Plots.Count;


	private record Region(char Type, List<Point> Plots);
	private record Edge(Point Plot, Direction Direction);
}
