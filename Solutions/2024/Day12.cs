namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 12: Garden Groups
/// https://adventofcode.com/2024/day/12
/// </summary>
[Description("Garden Groups")]
public static partial class Day12 {

	private static char[,] _farm = default!;

	[Init]
	public static void LoadFarm(string[] input) => _farm = input.To2dArray();

	public static int Part1(string[] _)
	{
		return
			_farm
			.FindRegions()
			.Sum(region => region.RegionPrice(_farm));
	}

	public static int Part2(string[] input, params object[]? args)
	{
		return
			_farm
			.FindRegions()
			.Sum(region => region.RegionBulkDiscountPrice(_farm));
	}

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
		int perimeter = 0;
		foreach (Point plot in region.Plots) {
			List<Cell<char>> adjacents = [.. farm.GetAdjacentCells(plot)];
			perimeter += 4 - adjacents.Count;
			foreach (Cell<char> adjacentPlot in adjacents) {
				if (adjacentPlot.Value != region.Type) {
					perimeter++;
				}
			}
		}

		return perimeter;
	}

	private static int RegionSidesCount(this Region region, char[,] farm)
	{
		HashSet<(Point, Direction)> edges = [];
		foreach (Point plot in region.Plots) {
			List<Cell<char>> adjacents = [.. farm.GetAdjacentCells(plot)];
			foreach (Direction direction in Directions.AllDirections) {
				Point next = plot + direction.Delta();
				if (farm.TryGetValue(next.X, next.Y, out char value)) {
					if (value != region.Type) {
						_ = edges.Add((next, direction));
					}
				} else {
					_ = edges.Add((next, direction));
				}
			}
		}

		int sides = 0;

		while (edges.Count > 0) {
			sides++;
			(Point edge, Direction direction) = edges.First();
			_ = edges.Remove((edge, direction));

			Direction check = direction switch {
				Direction.North => Direction.East,
				Direction.South => Direction.West,
				Direction.East => Direction.South,
				Direction.West => Direction.North,
				_ => throw new NotImplementedException(),
			};

			bool keepGoing = true;
			int step = 1;
			while (keepGoing) {
				keepGoing = false;
				(int dX, int dY) delta = (check.Delta().dX * step, (check.Delta().dY * step));
				if (edges.Contains((edge + delta, direction))) {
					_ = edges.Remove((edge + delta, direction));
					keepGoing = true;
				}
				if (edges.Contains((edge - delta, direction))) {
					_ = edges.Remove((edge - delta, direction));
					keepGoing = true;
				}

				step++;
			}

		}

		return sides;
	}

	private static int RegionBulkDiscountPrice(this Region r, char[,] farm) => r.RegionArea() * r.RegionSidesCount(farm);
	private static int RegionPrice(this Region r, char[,] farm) => r.RegionArea() * r.RegionPerimeter(farm);
	private static int RegionArea(this Region r) => r.Plots.Count;


	private record Region(char Type, List<Point> Plots);

}
