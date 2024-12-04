namespace AdventOfCode.Solutions._2021;

/// <summary>
/// Day 11: Dumbo Octopus
/// https://adventofcode.com/2021/day/11
/// </summary>
[Description("Dumbo Octopus")]
public class Day11 {

	public static string Part1(string[] input, params object[]? args) {
		int steps = GetArgument(args, argumentNumber: 1, defaultResult: 100);
		return Solution1(input, steps).ToString();
	}

	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input, int steps) {
		int[,] grid = input.SelectMany(i => i.AsDigits<int>()).To2dArray(input[0].Length);

		return Enumerable.Range(1, steps)
			.Select(i => GenerateStep(grid))
			.Sum();
	}

	private static int Solution2(string[] input) {
		int[,] grid = input.SelectMany(i => i.AsDigits<int>()).To2dArray(input[0].Length);

		int step = 1;
		while (GenerateStep(grid) != grid.LongLength) {
			step++;
		}

		return step;
	}

	private static int GenerateStep(int[,] grid) {
		HashSet<Point> flashedPoints = [];
		int flashesPerStep = 0;

		foreach ((int x, int y) in grid.Indexes()) {
			grid[x, y]++;
		}

		foreach ((int x, int y, int energy) in grid.ForEachCell()) {
			if (energy > 9 && flashedPoints.Add(new(x, y))) {
				flashesPerStep += 1 + CalculateAdjacentChanges(x, y, grid, flashedPoints);
			}
		}

		foreach ((int x, int y, int energy) in grid.ForEachCell()) {
			if (energy > 9) {
				grid[x, y] = 0;
			}
		}

		return flashesPerStep;
	}

	private static int CalculateAdjacentChanges(int col, int row, int[,] grid, HashSet<Point> flashedPoints) {
		int flashes = 0;

		foreach ((int x, int y, _) in grid.GetAdjacentCells(col, row, includeDiagonals: true)) {
			grid[x, y]++;
		}

		foreach ((int x, int y, int energyValue) in grid.GetAdjacentCells(col, row, includeDiagonals: true)) {
			if (energyValue > 9 && flashedPoints.Add(new(x, y))) {
				flashes += 1 + CalculateAdjacentChanges(x, y, grid, flashedPoints);
			}
		}

		return flashes;
	}
}
