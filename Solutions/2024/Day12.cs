﻿using static AdventOfCode.Solutions._2024.Day12;

namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 12: Garden Groups
/// https://adventofcode.com/2024/day/12
/// </summary>
[Description("Garden Groups")]
public sealed partial class Day12 {

	private static List<Region> _regions = [];

	[Init]
	public static void LoadFarm(string[] input) => _regions = [.. input.To2dArray().FindRegions()];

	public static int Part1(string[] _) => _regions.Sum(region => region.Price());
	public static int Part2(string[] _) => _regions.Sum(region => region.BulkDiscountPrice());


	internal record Region(char PlantType, List<Point> Plots, List<Edge> Perimeter);
	internal record Edge(Point Plot, Direction Direction);
}

file static partial class Day12Extensions
{
	public static int Price(this Region r) => r.Area() * r.PerimeterSize();
	public static int BulkDiscountPrice(this Region r) => r.Area() * r.Perimeter.SidesCount();

	private static int Area(this Region r) => r.Plots.Count;
	private static int PerimeterSize(this Region region) => region.Perimeter.Count;

	private static IEnumerable<Edge> FindPlotEdges(this List<Point> plots)
	{
		HashSet<Point> plotSet = [.. plots];

		return plots
			.SelectMany(plot =>
				Directions.AllDirections
					.Select(direction => (Direction: direction, Next: plot + direction.Delta()))
					.Where(x => plotSet.DoesNotContain(x.Next))
					.Select(x => new Edge(x.Next, x.Direction)));
	}

	private static int SidesCount(this IEnumerable<Edge> edges)
	{
		HashSet<Edge> edgeSet = [.. edges];

		int sidesCount = 0;
		while (edgeSet.Count > 0) {
			sidesCount++;

			Edge edge = edgeSet.First();
			_ = edgeSet.Remove(edge);

			Direction checkDirection = edge.Direction switch
			{
				Direction.North or Direction.South => Direction.East,
				Direction.East  or Direction.West  => Direction.South,
				_ => throw new NotImplementedException(),
			};

			// Look both ways along the when extending the side
			Point[] deltas =[new Point(checkDirection.Delta()), new Point(checkDirection.Reverse().Delta())];

			foreach (Point delta in deltas) {
				bool keepGoing = true;
				int n = 1;
				while (keepGoing) {
					keepGoing = edgeSet.Remove(edge with { Plot = edge.Plot + (delta * n) });
					n++;
				}
			}
		}

		return sidesCount;
	}

	public static IEnumerable<Region> FindRegions(this char[,] farm)
	{
		HashSet<Point> visited = [];

		return farm
			.ForEachCell()
			.Where(plot => visited.DoesNotContain(plot))
			.Select(plot => farm.FindRegion(plot, visited));
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

		return new(initialPlot.Value, plots, [.. plots.FindPlotEdges()]);
	}
}
