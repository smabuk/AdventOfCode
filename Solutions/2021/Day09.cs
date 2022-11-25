namespace AdventOfCode.Solutions._2021;

/// <summary>
/// Day 09: Smoke Basin
/// https://adventofcode.com/2021/day/9
/// </summary>
[Description("Smoke Basin")]
public class Day09 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	record Basin(Point LowPoint, List<Point> Locations);

	private static int Solution1(string[] input) {
		int[,] heightMap = input
			.SelectMany(i =>i.AsDigits())
			.To2dArray(input[0].Length);

		return heightMap
			.Walk2dArrayWithValues()
			.Where(cell => IsLowPoint(heightMap, cell.X, cell.Y))
			.Select(cell => cell.Value)
			.Sum(height => height + 1);
	}

	static bool IsLowPoint(int[,] array, int col, int row) {
		int currValue = array[col, row];
		foreach ((_, _, int value) in array.GetAdjacentCells(col, row)) {
			if (value <= currValue) {
				return false;
			}
		}
		return true;
	}

	private static long Solution2(string[] input) {
		int[,] heightMap = input
			.SelectMany(i => i.AsDigits())
			.To2dArray(input[0].Length);

		return heightMap
			.Walk2dArray()
			.Where(cell => IsLowPoint(heightMap, cell.X, cell.Y))
			.AsPoints()
			.Select(location => 
				new Basin(location, GetAdjacentBasinPoints(location, heightMap, new List<Point>() { location }))
				.Locations
				.Count)
			.OrderByDescending(x => x)
			.Take(3)
			.Aggregate((a, b) => a * b);
	}

	static List<Point> GetAdjacentBasinPoints(Point p, int[,] array, List<Point> knownPoints) {
		foreach ((int x, int y, int value) in array.GetAdjacentCells(p)) {
			Point newP = new(x, y);
			if (!knownPoints.Contains(newP)) {
				if (value < 9) {
					knownPoints.Add(newP);
					knownPoints = GetAdjacentBasinPoints(newP, array, knownPoints);
				}
			}
		}
		return knownPoints;
	}
}
