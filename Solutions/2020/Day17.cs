namespace AdventOfCode.Solutions.Year2020;

/// <summary>
/// Day 17: Conway Cubes
/// https://adventofcode.com/2020/day/17
/// </summary>
public class Day17 {

	public const char OFF = '.';
	public const char ON = '#';
	public const bool OFF_STATE = false;
	public const bool ON_STATE = true;

	record Point(int X, int Y);
	record CubePoint(int X, int Y, int Z) : Point(X, Y);
	record HyperPoint(int W, int X, int Y, int Z) : Point(X, Y);
	record Cube(Point Point, bool State);

	private static int Solution1(string[] input) {
		int noOfIterations = 6;

		Dictionary<Point, Cube> cubes = ParseInput(input, 3);
		Dictionary<Point, Cube> nextCubes = new();

		Point[] directions = (from dx in new[] { -1, 0, 1 }
							  from dy in new[] { -1, 0, 1 }
							  from dz in new[] { -1, 0, 1 }
							  where dx != 0 || dy != 0 || dz != 0
							  select new CubePoint(dx, dy, dz)).ToArray();

		int countOn = 0;
		for (int i = 0; i < noOfIterations; i++) {

			countOn = 0;
			nextCubes = new();
			int minX = cubes.Select(c => ((CubePoint)c.Key).X).Min() - 1;
			int maxX = cubes.Select(c => ((CubePoint)c.Key).X).Max() + 1;
			int minY = cubes.Select(c => ((CubePoint)c.Key).Y).Min() - 1;
			int maxY = cubes.Select(c => ((CubePoint)c.Key).Y).Max() + 1;
			int minZ = cubes.Select(c => ((CubePoint)c.Key).Z).Min() - 1;
			int maxZ = cubes.Select(c => ((CubePoint)c.Key).Z).Max() + 1;

			for (int z = minZ; z <= maxZ; z++) {
				for (int y = minY; y <= maxY; y++) {
					for (int x = minX; x <= maxX; x++) {
						CubePoint p = new(x, y, z);
						Cube current = cubes.GetValueOrDefault(p) ?? new(p, OFF_STATE);
						Cube next = current;
						int adjacentCount = GetAdjacentCubesCount(current, cubes, directions);
						if (current.State == OFF_STATE && adjacentCount == 3) {
							next = next with { State = ON_STATE };
						} else if (current.State == ON_STATE && (adjacentCount is not 2 and not 3)) {
							next = next with { State = OFF_STATE };
						}
						if (next.State == ON_STATE) {
							nextCubes[p] = next;
							countOn++;
						}
					}
				}
			}
			cubes = nextCubes;
		}

		return countOn;
	}

	private static int Solution2(string[] input) {
		int noOfIterations = 6;

		Dictionary<Point, Cube> cubes = ParseInput(input, 4);
		Dictionary<Point, Cube> nextCubes = new();

		Point[] directions = (from dw in new[] { -1, 0, 1 }
							  from dx in new[] { -1, 0, 1 }
							  from dy in new[] { -1, 0, 1 }
							  from dz in new[] { -1, 0, 1 }
							  where dw != 0 || dx != 0 || dy != 0 || dz != 0
							  select new HyperPoint(dw, dx, dy, dz)).ToArray();

		int countOn = 0;
		for (int i = 0; i < noOfIterations; i++) {

			countOn = 0;
			nextCubes = new();
			int minW = cubes.Select(c => ((HyperPoint)c.Key).W).Min() - 1;
			int maxW = cubes.Select(c => ((HyperPoint)c.Key).W).Max() + 1;
			int minX = cubes.Select(c => ((HyperPoint)c.Key).X).Min() - 1;
			int maxX = cubes.Select(c => ((HyperPoint)c.Key).X).Max() + 1;
			int minY = cubes.Select(c => ((HyperPoint)c.Key).Y).Min() - 1;
			int maxY = cubes.Select(c => ((HyperPoint)c.Key).Y).Max() + 1;
			int minZ = cubes.Select(c => ((HyperPoint)c.Key).Z).Min() - 1;
			int maxZ = cubes.Select(c => ((HyperPoint)c.Key).Z).Max() + 1;

			for (int z = minZ; z <= maxZ; z++) {
				for (int y = minY; y <= maxY; y++) {
					for (int x = minX; x <= maxX; x++) {
						for (int w = minW; w <= maxW; w++) {
							HyperPoint p = new(w, x, y, z);
							Cube current = cubes.GetValueOrDefault(p) ?? new(p, OFF_STATE);
							Cube next = current;
							int adjacentCount = GetAdjacentCubesCount(current, cubes, directions);
							if (current.State == OFF_STATE && adjacentCount == 3) {
								next = next with { State = ON_STATE };
							} else if (current.State == ON_STATE && (adjacentCount is not 2 and not 3)) {
								next = next with { State = OFF_STATE };
							}
							if (next.State == ON_STATE) {
								nextCubes[p] = next;
								countOn++;
							}
						}
					}
				}
			}
			cubes = nextCubes;
		}

		return countOn;
	}

	private static int GetAdjacentCubesCount(Cube cube, Dictionary<Point, Cube> cubes, Point[] directions) {
		int count = 0;
		foreach (Point dir in directions) {
			Point p = cube.Point switch {
				HyperPoint hp when dir is HyperPoint d => new HyperPoint(hp.W + d.W, hp.X + d.X, hp.Y + d.Y, hp.Z + d.Z),
				CubePoint cp when dir is CubePoint d => new CubePoint(cp.X + d.X, cp.Y + d.Y, cp.Z + d.Z),
				_ => new Point(cube.Point.X + dir.X, cube.Point.Y + dir.Y)
			};
			if (cubes.GetValueOrDefault(p)?.State == ON_STATE) {
				count++;
			}
		}
		return count;
	}

	private static Dictionary<Point, Cube> ParseInput(string[] input, int noOfDimensions) {
		Dictionary<Point, Cube> cubes = new();
		int width = input[0].Length;
		int height = input.Length;

		for (int y = 0; y < height; y++) {
			string itemLine = input[y];
			for (int x = 0; x < width; x++) {
				if (itemLine[x] == ON) {
					Point point = noOfDimensions switch {
						3 => new CubePoint(x, y, 0),
						4 => new HyperPoint(0, x, y, 0),
						_ => new Point(x, y)
					};
					cubes[point] = new(point, ON_STATE);
				}
			}
		}
		return cubes;
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
