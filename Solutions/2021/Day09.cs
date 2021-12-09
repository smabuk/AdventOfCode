namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 09: Smoke Basin
/// https://adventofcode.com/2021/day/9
/// </summary>
[Description("Smoke Basin")]
public class Day09 {

	private static readonly List<(int dX, int dY)> DIRECTIONS = new()
		{ (0, -1), (0, 1), (-1, 0), (1, 0) };

	record Point(int X, int Y);

	record Basin(Point LowPoint, List<Point> Locations) {
		public int Size => Locations.Count;
	};

	private static int Solution1(string[] input) {
		int[,] heightMap = input.Select(i => ParseLine(i)).SelectMany(i => i).As2dArray(input[0].Length);

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
		bool lowPoint = true;
		int cols = array.GetUpperBound(0);
		int rows = array.GetUpperBound(1);
		int value = array[col, row];
		foreach ((int dX, int dY) in DIRECTIONS) {
			int newX = col + dX;
			int newY = row + dY;
			if (newX >= 0 && newX <= cols && newY >= 0 && newY <= rows) {
				if (array[newX, newY] <= value) {
					return false;
				}
			}
		}
		return lowPoint;
	}

	private static long Solution2(string[] input) {
		int[,] heightMap = input.Select(i => ParseLine(i)).SelectMany(i => i).As2dArray(input[0].Length);

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
		int cols = array.GetUpperBound(0);
		int rows = array.GetUpperBound(1);
		foreach ((int dX, int dY) in DIRECTIONS) {
			int newX = p.X + dX;
			int newY = p.Y + dY;
			Point newP = new(newX, newY);
			if (!knownPoints.Contains(newP)) {
				if (newX >= 0 && newX <= cols && newY >= 0 && newY <= rows) {
					if (array[newX, newY] < 9) {
						knownPoints.Add(newP);
						knownPoints = GetAdjacentBasinPoints(newP, array, knownPoints);
					}
				}
			}
		}
		return knownPoints;
	}


	private static IEnumerable<int> ParseLine(string input) {
		foreach (char c in input) {
			yield return int.Parse($"{c}");
		}
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
