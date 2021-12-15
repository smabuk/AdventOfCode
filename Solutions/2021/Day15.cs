namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 15: Chiton
/// https://adventofcode.com/2021/day/15
/// </summary>
[Description("Chiton")]
public class Day15 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	record RecordType(string Name, int Value);

	private static int Solution1(string[] input) {
		int[,] grid = input.SelectMany(i => i.AsDigits()).To2dArray(input[0].Length);

		Point start = new(0, 0);
		Point end = new(grid.GetUpperBound(0), grid.GetUpperBound(1));

		Point current = start;
		HashSet<Point> visited = new();
		var neighbours = grid.GetAdjacentCells(current);
		Point start1 = new(neighbours.First().x, neighbours.First().y);
		Point start2 = new(neighbours.Last().x, neighbours.Last().y);
		(bool failed1, int result1) = FindShortestRoute(start1, end, grid, visited.Append(start).ToHashSet());
		(bool failed2, int result2) = FindShortestRoute(start2, end, grid, visited.Append(start).ToHashSet());

		return Math.Min(result1, result2);
	}

	private static (bool success, int result) FindShortestRoute(Point current, Point end, int[,] grid, HashSet<Point> visited) {
		int risk = grid[current.X, current.Y];
		if (current == end) {
			return (true, risk);
		}
		if (visited.Contains(current)) {
			return (false, risk);
		}
		List<int> results = new();
		var neighbours = grid.GetAdjacentCells(current);
		foreach (var (x, y, value) in neighbours) {
			if (x >= current.X && y >= current.Y) {
				(bool success, int result) = FindShortestRoute(new(x, y), end, grid, visited.Append(current).ToHashSet());
				if (success) {
					results = results.Append(result).ToList();
				}
			}
		}

		if (results.Count == 0) {
			return (false, risk);
		} else {
			return (true, results.Min() + risk);
		}
	}

	private static string Solution2(string[] input) {
		int[,] grid = input.SelectMany(i => i.AsDigits()).To2dArray(input[0].Length);
		return "** Solution not written yet **";
	}
}
