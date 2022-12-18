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
		return NoOfExposedFaces(cubes);
	}

	private static int Solution2(string[] input) {
		HashSet<Point3d> cubes = input.Select(ParseLine).ToHashSet();
		int noOfExposedFaces = NoOfExposedFaces(cubes);

		// Find the boundaries and attempt to flood fill around the shape to leave 1 block with pockets in it
		// Wo don't have to worry about exterior points after that
		// Slicing the input data shows an irregular sphere with a hollowish centre

		int minX = cubes.Select(cubes => cubes.X).Min() - 1;
		int minY = cubes.Select(cubes => cubes.Y).Min() - 1;
		int minZ = cubes.Select(cubes => cubes.Z).Min() - 1;
		int maxX = cubes.Select(cubes => cubes.X).Max() + 1;
		int maxY = cubes.Select(cubes => cubes.Y).Max() + 1;
		int maxZ = cubes.Select(cubes => cubes.Z).Max() + 1;


		// Pick a point on the edge and work inwards filling up the space
		Queue<Point3d> exteriorQueue = new(new Point3d[] { new(minX, minY, minZ) });

		while ( exteriorQueue.Count > 0 ) {
			Point3d cube = exteriorQueue.Dequeue();
			cubes.Add(cube);
			foreach (Point3d adjacent in Adjacent(cube)) {
				if (   InBounds   (adjacent)
					&& IsSpace    (adjacent)
					&& IsNotQueued(adjacent)
					) {
					exteriorQueue.Enqueue(adjacent);
				}
			}
		}

		int countEnclosedFaces = 0;
		for (int z = minZ; z <= maxZ; z++) {
		for (int y = minY; y <= maxY; y++) {
		for (int x = minX; x <= maxX; x++) {
			Point3d cube = new(x, y, z);
			
			if (   IsSpace      (cube)
				&& IsNotFloating(cube)
				) {
				countEnclosedFaces += AdjacentSolidCubesCount(cube);
			}
		}
		}
		}

		return noOfExposedFaces - countEnclosedFaces;

		// Local functions

		int AdjacentSolidCubesCount(Point3d cube) => Adjacent(cube).Select(cubes.Contains).Count(b => b == true);

		bool IsNotQueued(Point3d cube) => !exteriorQueue.Contains(cube);

		bool IsSpace(Point3d cube) => !cubes.Contains(cube);

		bool IsNotFloating(Point3d cube) => Adjacent(cube).Select(cubes.Contains).Any(b => b == true) ;

		bool InBounds(Point3d cube) =>
			   cube.X >= minX && cube.X <= maxX
			&& cube.Y >= minY && cube.Y <= maxY
			&& cube.Z >= minZ && cube.Z <= maxZ;
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

	private static int NoOfExposedFaces(HashSet<Point3d> cubes) {
		return (cubes.Count * 6) -
					cubes.SelectMany(Adjacent)
					.Select(cubes.Contains)
					.Count(exists => exists == true);
	}

	private static Point3d ParseLine(string input) {
		List<int> numbers = input.Split(',').AsInts().ToList();
		return new Point3d(numbers[0], numbers[1], numbers[2]);
	}
}
