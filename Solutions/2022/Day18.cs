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
		int noOfExposedFaces = cubes.Count * 6;

		foreach (Point3d cube in cubes) {
			if (cubes.Contains(cube with { X = cube.X - 1 })) {
				noOfExposedFaces--;
			}
			if (cubes.Contains(cube with { X = cube.X + 1 })) {
				noOfExposedFaces--;
			}
			if (cubes.Contains(cube with { Y = cube.Y - 1 })) {
				noOfExposedFaces--;
			}
			if (cubes.Contains(cube with { Y = cube.Y + 1 })) {
				noOfExposedFaces--;
			}
			if (cubes.Contains(cube with { Z = cube.Z - 1 })) {
				noOfExposedFaces--;
			}
			if (cubes.Contains(cube with { Z = cube.Z + 1 })) {
				noOfExposedFaces--;
			}
		}

		return noOfExposedFaces;
	}

	private static int Solution2(string[] input) {
		HashSet<Point3d> cubes = input.Select(ParseLine).ToHashSet();
		int noOfExposedFaces = cubes.Count * 6;

		return noOfExposedFaces;
	}

	private static Point3d ParseLine(string input) {
		List<int> numbers = input.Split(',').AsInts().ToList();
		return new Point3d(numbers[0], numbers[1], numbers[2]);
	}
}
