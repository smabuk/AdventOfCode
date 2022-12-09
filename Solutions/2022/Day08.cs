namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 08: Treetop Tree House
/// https://adventofcode.com/2022/day/8
/// </summary>
[Description("Treetop Tree House")]
public sealed partial class Day08 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		int visibleTrees = 0;
		int[,] grid = input.SelectMany(i => i.AsDigits()).To2dArray(input[0].Length);
		int columns = grid.NoOfColumns();
		int rows = grid.NoOfRows();

		for (int row = 0; row < rows; row++) {
			for (int col = 0; col < columns; col++) {
				if (IsVisible(col, row)) {
					visibleTrees++;
				}
			}
		}

		return visibleTrees;

		bool IsVisible(int col, int row) {
			if (col == 0 || row == 0 || col == columns - 1 || row == rows - 1) {
				return true;
			}

			int tree = grid[col, row];

			return IsVisibleInDirection(-1, 0)
				|| IsVisibleInDirection(1, 0)
				|| IsVisibleInDirection(0, -1)
				|| IsVisibleInDirection(0, 1);

			bool IsVisibleInDirection(int dX, int dY) {
				int newCol = col + dX, newRow = row + dY;
				while (newCol != -1 && newRow != -1 && newCol != columns && newRow != rows) {
					if (grid[newCol, newRow] >= tree) {
						return false;
					}
					newCol += dX;
					newRow += dY;
				}
				return true;
			}
		}
	}

	private static long Solution2(string[] input) {
		long scenicScoreMax = 0;
		int[,] grid = input.SelectMany(i => i.AsDigits()).To2dArray(input[0].Length);
		int columns = grid.NoOfColumns();
		int rows = grid.NoOfRows();

		for (int row = 1; row < rows - 1; row++) {
			for (int col = 1; col < columns - 1; col++) {
				scenicScoreMax = Math.Max(ScenicScore(col, row), scenicScoreMax);
			}
		}

		return scenicScoreMax;

		long ScenicScore(int col, int row) {
			if (col == 0 || row == 0 || col == columns - 1 || row == rows - 1) {
				return 0;
			}

			int tree = grid[col, row];

			return ViewingDistance(-1, 0)
				* ViewingDistance(1, 0)
				* ViewingDistance(0, -1)
				* ViewingDistance(0, 1);

			int ViewingDistance(int dX, int dY) {
				int viewingDistance = 0;
				int newCol = col + dX, newRow = row + dY;
				while (newCol != -1 && newRow != -1 && newCol != columns && newRow != rows) {
					viewingDistance++;
					if (grid[newCol, newRow] >= tree) {
						break;
					}
					newCol += dX;
					newRow += dY;
				}
				return viewingDistance;
			}
		}
	}

}
