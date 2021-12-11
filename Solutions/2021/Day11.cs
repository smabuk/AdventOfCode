namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 11: Dumbo Octopus
/// https://adventofcode.com/2021/day/11
/// </summary>
[Description("Dumbo Octopus")]
public class Day11 {

	private static int Solution1(string[] input, int steps) {
		int[,] grid = input.Select(i => i.AsDigits()).SelectMany(i => i).To2dArray(input[0].Length);

		return Enumerable.Range(1, steps)
			.Select(i => GenerateStep(grid))
			.Sum();
	}

	private static int GenerateStep(int[,] grid) {
		HashSet<Point> flashedPoints = new();
		int flashesPerStep = 0;

		int cols = grid.GetUpperBound(0);
		int rows = grid.GetUpperBound(1);

		for (int row = 0; row <= rows; row++) {
			for (int col = 0; col <= cols; col++) {
				grid[col, row]++;
			}
		}

		for (int row = 0; row <= rows; row++) {
			for (int col = 0; col <= cols; col++) {
				if (grid[col, row] > 9 && flashedPoints.Add(new(col, row))) {
					flashesPerStep += 1 + CalculateAdjacentChanges(col, row, grid, flashedPoints);
				}
			}
		}

		for (int row = 0; row <= rows; row++) {
			for (int col = 0; col <= cols; col++) {
				if (grid[col, row] > 9) {
					grid[col, row] = 0;
				}
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


	private static int Solution2(string[] input) {
		int[,] grid = input.Select(i => i.AsDigits()).SelectMany(i => i).To2dArray(input[0].Length);

		int step = 1;
		while (GenerateStep(grid) != grid.LongLength) {
			step++;
		}

		return step;
	}


	#region Problem initialisation
	public static string Part1(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		int steps = GetArgument(args, 1, 100);
		input = input.StripTrailingBlankLineOrDefault();
		return Solution1(input, steps).ToString();
	}
	public static string Part2(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution2(input).ToString();
	}
	#endregion

}
