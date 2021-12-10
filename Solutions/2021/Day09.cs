namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 09: Smoke Basin
/// https://adventofcode.com/2021/day/9
/// </summary>
[Description("Smoke Basin")]
public class Day09 {

	record Basin(Point LowPoint, List<Point> Locations) {
		public int Size => Locations.Count;
	};

	private static int Solution1(string[] input) {
		int[,] heightMap = input.Select(i =>i.AsDigits()).SelectMany(i => i).To2dArray(input[0].Length);

		int cols = heightMap.GetUpperBound(0);
		int rows = heightMap.GetUpperBound(1);

		List<int> lowPoints = new();

		for (int y = 0; y <= rows; y++) {
			for (int x = 0; x <= cols; x++) {
				if (IsLowPoint(heightMap, x, y)) {
					lowPoints.Add(heightMap[x, y]);
				}
			}
		}

		return lowPoints.Sum(lp => lp + 1);
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
		int[,] heightMap = input.Select(i => i.AsDigits()).SelectMany(i => i).To2dArray(input[0].Length);

		int cols = heightMap.GetUpperBound(0);
		int rows = heightMap.GetUpperBound(1);

		List<Basin> basins = new();

		for (int y = 0; y <= rows; y++) {
			for (int x = 0; x <= cols; x++) {
				if (IsLowPoint(heightMap, x, y)) {
					Point lowPoint = new(x, y);
					Basin basin = new(lowPoint, GetAdjacentBasinPoints(lowPoint, heightMap, new() { lowPoint }));
					basins.Add(basin);
				}
			}
		}

		return basins
			.OrderByDescending(b => b.Size)
			.Take(3)
			.Select(b => b.Size)
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


	#region Problem initialisation
	public static string Part1(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution1(input).ToString();
	}
	public static string Part2(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution2(input).ToString();
	}
	#endregion

}
