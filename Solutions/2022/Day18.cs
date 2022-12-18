using System.Data;

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

		int minX = cubes.Select(cubes => cubes.X).Min();
		int minY = cubes.Select(cubes => cubes.Y).Min();
		int minZ = cubes.Select(cubes => cubes.Z).Min();
		int maxX = cubes.Select(cubes => cubes.X).Max();
		int maxY = cubes.Select(cubes => cubes.Y).Max();
		int maxZ = cubes.Select(cubes => cubes.Z).Max();

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

		// Pick a point in the middle and work outwards
		Point3d start = new(maxX / 2, maxY / 2, maxZ / 2);
		//start = new(2, 2, 5);
		Queue<Point3d> interiorQueue = new();
		int countEnclosedFaces = 0;
		processedCubes.Clear();
		interiorQueue.Enqueue(start);
		while ( interiorQueue.Count > 0 ) {
			Point3d cube = interiorQueue.Dequeue();
			processedCubes.Add(cube);
			if (!IsFloating(cube, cubes)) {
				countEnclosedFaces += 
					Adjacent(cube)
					.Select(cubes.Contains)
					.Count(b => b == true);
			}
			if (cube.X > 40) {
				throw new Exception("Whoops");
			}
			foreach (Point3d adjacent in Adjacent(cube)) {
				if (processedCubes.Contains(adjacent) is false) {
					if (cubes.Contains(adjacent)) {
						//countEnclosedFaces++;
					} else if (processedCubes.Contains(adjacent) is false && interiorQueue.Contains(adjacent) is false) {
						interiorQueue.Enqueue(adjacent);
					}
				}
			}

		}

		// 2830 is too high
		// 1848 is too low
		return noOfExposedFaces - countEnclosedFaces;
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
