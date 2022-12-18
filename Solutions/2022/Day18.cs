using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 18: Boiling Boulders
/// https://adventofcode.com/2022/day/18
/// </summary>
[Description("Boiling Boulders")]
public sealed partial class Day18 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		HashSet<Point3d> cubes = input.Select(ParseLine).ToHashSet();

		return (cubes.Count * 6)
				- cubes.SelectMany(Adjacent)
					.Select(cubes.Contains)
					.Count(exists => exists == true);
	}

	private static int Solution2(string[] input) {
		HashSet<Point3d> cubes = input.Select(ParseLine).ToHashSet();
		HashSet<Point3d> processedCubes = new();
		int noOfExposedFaces = cubes.Count * 6;

		foreach (Point3d cube in cubes) {
			foreach (Point3d adjacentCube in Adjacent(cube)) {
				if (cubes.Contains(adjacentCube)) {
					noOfExposedFaces--;
				} else if (IsSurrounded(adjacentCube, cubes)) {  // assumes 1x1x1 space
					processedCubes.Add(adjacentCube);
				}
			}
		}

		// If we assume only 1x1x1 block of air enclosed then 4178 is too high
		// Means we need to find the boundaries and attempt to flood fill any spaces
		// Slicing the input data shows an irregular sphere with a hollow centre

		int minX = cubes.Select(cubes => cubes.X).Min() - 1;
		int minY = cubes.Select(cubes => cubes.Y).Min() - 1;
		int minZ = cubes.Select(cubes => cubes.Z).Min() - 1;
		int maxX = cubes.Select(cubes => cubes.X).Max() + 1;
		int maxY = cubes.Select(cubes => cubes.Y).Max() + 1;
		int maxZ = cubes.Select(cubes => cubes.Z).Max() + 1;

		// Pick a point on the edge and work inwards
		Point3d start = new(minX, minY, minZ);
		Queue<Point3d> exteriorQueue = new();
		processedCubes.Clear();
		exteriorQueue.Enqueue(start);
		while ( exteriorQueue.Count > 0 ) {
			Point3d cube = exteriorQueue.Dequeue();
			processedCubes.Add(cube);
			cubes.Add(cube);
			foreach (Point3d adjacent in Adjacent(cube)) {
				if    (adjacent.X < minX || adjacent.X > maxX
					|| adjacent.Y < minY || adjacent.Y > maxY
					|| adjacent.Z < minZ || adjacent.Z > maxZ
						) {
					continue;
				}
				if (cubes.Contains(adjacent) is false) {
					if (processedCubes.Contains(adjacent) is false && exteriorQueue.Contains(adjacent) is false) {
						exteriorQueue.Enqueue(adjacent);
					}
				}
			}

		}

		int countEnclosedFaces = 0;
		for (int z = minZ; z <= maxZ; z++) {
			for (int y = minY; y <= maxY; y++) {
				for (int x = minX; x <= maxX; x++) {
					Point3d cube = new(x, y, z);
					if (cubes.Contains(cube) is false && !IsFloating(cube, cubes)) {
						countEnclosedFaces +=
							Adjacent(cube)
							.Select(cubes.Contains)
							.Count(b => b == true);
					}
				}
			}
		}

		return noOfExposedFaces - countEnclosedFaces;

		void PrintToConsole() {
			// Start Debugging - print 3d slices
			for (int z = minZ; z <= maxZ; z++) {
				Console.WriteLine();
				for (int y = minY; y <= maxY; y++) {
					for (int x = minX; x <= maxX; x++) {
						Console.Write($"{(cubes.Contains(new(x, y, z)) ? '#' : '.')}");
					}
					Console.WriteLine();
				}
				Console.WriteLine();
			}
			// End Debugging
		}
	}

	private static bool IsSurrounded(Point3d cube, HashSet<Point3d> cubes) {
		return Adjacent(cube)
			.Select(cubes.Contains)
			.Count(b => b == true) == 6;
	}

	private static bool IsFloating(Point3d cube, HashSet<Point3d> cubes) {
		return Adjacent(cube)
			.Select(cubes.Contains)
			.Count(b => b == true) == 0;
	}

	private static IEnumerable<Point3d> Adjacent(Point3d cube) {
		return new Point3d[] {
			cube with { X = cube.X - 1 },
			cube with { X = cube.X + 1 },
			cube with { Y = cube.Y - 1 },
			cube with { Y = cube.Y + 1 },
			cube with { Z = cube.Z - 1 },
			cube with { Z = cube.Z + 1 },
		};
	}


	private static Point3d ParseLine(string input) {
		List<int> numbers = input.Split(',').AsInts().ToList();
		return new Point3d(numbers[0], numbers[1], numbers[2]);
	}
}
