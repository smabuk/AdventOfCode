
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

	public static string Part2(string[] input, params object[]? args) => NO_SOLUTION_WRITTEN_MESSAGE;

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
				if (adjacentPlot.Value != region.RegionType) {
					perimeter++;
				}
			}
		}

		return perimeter;
	}

	private static int RegionPrice(this Region region, char[,] farm) => region.RegionArea() * region.RegionPerimeter(farm);
	private static int RegionArea(this Region region) => region.Plots.Count;


	private record Region(char RegionType, List<Point> Plots);

}
