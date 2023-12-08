namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 14: Regolith Reservoir
/// https://adventofcode.com/2022/day/14
/// </summary>
[Description("Regolith Reservoir")]
public sealed partial class Day14 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static readonly char AIR = '.';
	private static readonly char ROCK = '#';
	private static readonly char SAND_SOURCE = '+';
	private static readonly char SAND = 'o';

	private static int Solution1(string[] input) {
		(
			List<List<Point>> linePaths, 
			int noOfColumns, 
			int noOfRows, 
			Point newStartPoint
		) = LoadPaths(input, startPoint: new(500, 0));

		char[,] cave = BuildCave(
			linePaths: linePaths,
			noOfColumns: noOfColumns,
			noOfRows: noOfRows,
			startPoint: newStartPoint,
			includeFloor: false);

		return CalculateUnitsOfSand(
			cave: cave,
			startPoint: newStartPoint,
			partNo: 1);
	}

	private static int Solution2(string[] input) {
		(
			List<List<Point>> linePaths,
			int noOfColumns,
			int noOfRows,
			Point newStartPoint
		) = LoadPaths(input, startPoint: new(500, 0));

		char[,] cave = BuildCave(
			linePaths: linePaths,
			noOfColumns: noOfColumns,
			noOfRows: noOfRows,
			startPoint: newStartPoint,
			includeFloor: true);

		return CalculateUnitsOfSand(
			cave: cave,
			startPoint: newStartPoint,
			partNo: 2);
	}

	private static (List<List<Point>> LinePaths, int NoOfColumns, int NoOfrows, Point NewStartPoint) LoadPaths(string[] input, Point startPoint) {
		int minX = int.MaxValue;
		int maxX = int.MinValue;
		int maxY = int.MinValue;
		List<List<Point>> linePaths = [];

		for (int i = 0; i < input.Length; i++) {
			List<Point> pathPoints = [];
			int[] coords = input[i].Replace("-", "").Split(['>', ',']).As<int>().ToArray();
			for (int j = 0; j < coords.Length; j += 2) {
				pathPoints.Add(new(coords[j], coords[j + 1]));
				minX = Math.Min(minX, coords[j]);
				maxX = Math.Max(maxX, coords[j]);
				maxY = Math.Max(maxY, coords[j + 1]);
			}
			linePaths.Add(pathPoints);
		}

		int noOfRows = maxY + 1;
		int noOfColumns = maxX - minX + (noOfRows * 3) + 1;
		int xOffset = maxX - minX + (noOfRows / 2) + 1;
		for (int i = 0; i < linePaths.Count; i++) {
			for (int j = 0; j < linePaths[i].Count; j++) {
				linePaths[i][j] = linePaths[i][j] with { X = linePaths[i][j].X - minX + xOffset };
			}
		}

		return (linePaths, noOfColumns, noOfRows, startPoint with { X = startPoint.X - minX + xOffset });
	}


	private static char[,] BuildCave(List<List<Point>> linePaths, int noOfColumns, int noOfRows, Point startPoint, bool includeFloor = false) {
		if (includeFloor) {
			noOfRows += 2;
		}

		char[,] cave = new char[noOfColumns, noOfRows];

		foreach ((int X, int Y) in cave.Walk2dArray()) {
			cave[X, Y] = AIR;
		}
		if (includeFloor) {
			for (int i = 0; i < cave.NoOfColumns(); i++) {
				cave[i, noOfRows - 1] = ROCK;
			}
		}
		cave[startPoint.X, startPoint.Y] = SAND_SOURCE;

		foreach (List<Point> linePath in linePaths) {
			for (int lp = 0; lp < linePath.Count - 1; lp++) {
				Point start = linePath[lp];
				Point diff = start - linePath[lp + 1];
				int steps = Math.Max(Math.Abs(diff.X), Math.Abs(diff.Y)) + 1;
				for (int i = 0; i < steps; i++) {
					int x = start.X + (i * -Math.Sign(diff.X));
					int y = start.Y + (i * -Math.Sign(diff.Y));
					cave[x, y] = ROCK;
				}
			}
		}

		return cave;
	}

	private static int CalculateUnitsOfSand(char[,] cave, Point startPoint, int partNo) {
		int unitsOfSand = 0;
		int floorDepth = cave.NoOfRows() - 1;

		do {
			Point sand = startPoint;
			for (int y = 1; y <= floorDepth; y++) {
				if (cave[sand.X, y] == AIR) {
					sand = sand with { Y = y };
				} else if (cave[sand.X - 1, y] == AIR) {
					sand = sand with { X = sand.X - 1, Y = y };
				} else if (cave[sand.X + 1, y] == AIR) {
					sand = sand with { X = sand.X + 1, Y = y };
				} else {
					break;
				}
			}

			if (partNo == 1 && sand.Y == floorDepth) {
				return unitsOfSand;
			} else if (partNo == 2 && sand == startPoint) {
				cave[sand.X, sand.Y] = SAND;
				return unitsOfSand + 1;
			}

			unitsOfSand++;
			cave[sand.X, sand.Y] = SAND;

		} while (true);
	}

}
