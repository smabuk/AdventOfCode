namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 09: Smoke Basin
/// https://adventofcode.com/2021/day/XX
/// </summary>
[Description("Smoke Basin")]
public class Day09 {

	record Vector(int DX, int DY);
	record Point(int X, int Y);
	record Basin(Point LowPoint, List<Point> Locations) {
		public int Size => Locations.Count;
	};

	private static int Solution1(string[] input) {
		List<Vector> vectors = new() { new(0, -1), new Vector(0, 1), new Vector(-1, 0), new Vector(1, 0) };
		int[,] heatMap = input.Select(i => ParseLine(i)).SelectMany(i => i).AsArray(input[0].Length);

		int cols = heatMap.GetUpperBound(0);
		int rows = heatMap.GetUpperBound(1);

		List<int> lowPoints = new();

		for (int y = 0; y <= rows; y++) {
			for (int x = 0; x <= cols; x++) {
				if (IsLowPoint(x, y)) {
					lowPoints.Add(heatMap[x, y]);
				}
			}
		}

		return lowPoints.Sum(lp => lp + 1);

		bool IsLowPoint(int col, int row) {
			bool lowPoint = true;
			int value = heatMap[col, row];
			foreach (Vector v in vectors) {
				int newX = col + v.DX;
				int newY = row + v.DY;
				if (newX >= 0 && newX <= cols) {
					if (newY >= 0 && newY <= rows) {
						if (heatMap[newX, newY] <= value) {
							return false;
						}
					}
				}
			}
			return lowPoint;
		}
	}


	private static long Solution2(string[] input) {
		List<Vector> vectors = new() { new(0, -1), new Vector(0, 1), new Vector(-1, 0), new Vector(1, 0) };
		int[,] heatMap = input.Select(i => ParseLine(i)).SelectMany(i => i).AsArray(input[0].Length);

		int cols = heatMap.GetUpperBound(0);
		int rows = heatMap.GetUpperBound(1);

		List<Basin> basins = new();

		for (int y = 0; y <= rows; y++) {
			for (int x = 0; x <= cols; x++) {
				if (IsLowPoint(x, y)) {
					Point lowPoint = new(x, y);
					Basin b = new(lowPoint, new());
					basins.Add(b);
				}
			}
		}

		// Build basins
		foreach (Basin basin in basins) {
			List<Point> points = GetAjacentBasinPoints(basin.LowPoint, new() { basin.LowPoint });
			basin.Locations.AddRange(points);
		}

		// multiply sizes of largest 3
		var biggest = basins.OrderByDescending(b => b.Size).Take(3).ToArray();
		return biggest[0].Size * biggest[1].Size * biggest[2].Size;


		List<Point> GetAjacentBasinPoints(Point p, List<Point> knownPoints) {
			int value = heatMap[p.X, p.Y];
			foreach (Vector v in vectors) {
				int newX = p.X + v.DX;
				int newY = p.Y + v.DY;
				Point newP = new Point(newX, newY);
				if (!knownPoints.Contains(newP)) {
					if (newX >= 0 && newX <= cols) {
						if (newY >= 0 && newY <= rows) {
							if (heatMap[newX, newY] < 9) {
								knownPoints.Add(new Point(newX, newY));
								knownPoints = GetAjacentBasinPoints(newP, knownPoints);
							}
						}
					}
				}
			}
			return knownPoints;
		}

		bool IsLowPoint(int col, int row) {
			bool lowPoint = true;
			int value = heatMap[col, row];
			foreach (Vector v in vectors) {
				int newX = col + v.DX;
				int newY = row + v.DY;
				if (newX >= 0 && newX <= cols) {
					if (newY >= 0 && newY <= rows) {
						if (heatMap[newX, newY] <= value) {
							return false;
						}
					}
				}
			}
			return lowPoint;
		}
	}


	private static IEnumerable<int> ParseLine(string input) {
		foreach (char c in input) {
			yield return int.Parse($"{c}");
		}
	}





	#region Problem initialisation
	public static string Part1(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		// int arg1 = GetArgument(args, 1, 25);
		input = input.StripTrailingBlankLineOrDefault();
		return Solution1(input).ToString();
	}
	public static string Part2(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		// int arg1 = GetArgument(args, 1, 25);
		input = input.StripTrailingBlankLineOrDefault();
		return Solution2(input).ToString();
	}
	#endregion

}
