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
		int gridWidth = grid.GetLength(0);
		int gridHeight = grid.GetLength(1);

		for (int y = 0; y < gridHeight; y++) {
			for (int x = 0; x < gridWidth; x++) {
				if (IsVisible(x, y)) {
					visibleTrees++;
				}
			}
		}

		return visibleTrees;

		bool IsVisible(int x, int y) {
			if (x == 0 || y == 0 || x == gridWidth - 1 || y == gridHeight - 1) {
				return true;
			}

			int tree = grid[x, y];

			return IsVisibleInDirection(-1, 0)
				|| IsVisibleInDirection(1, 0)
				|| IsVisibleInDirection(0, -1)
				|| IsVisibleInDirection(0, 1);

			bool IsVisibleInDirection(int dX, int dY) {
				int x1 = x + dX, y1 = y + dY;
				while (x1 != -1 && y1 != -1 && x1 != gridWidth && y1 != gridHeight) {
					if (grid[x1, y1] >= tree) {
						return false;
					}
					x1 += dX;
					y1 += dY;
				}
				return true;
			}
		}
	}

	private static long Solution2(string[] input) {
		long scenicScoreMax = 0;
		int[,] grid = input.SelectMany(i => i.AsDigits()).To2dArray(input[0].Length);
		int gridWidth = grid.GetLength(0);
		int gridHeight = grid.GetLength(1);

		for (int y = 1; y < gridHeight - 1; y++) {
			for (int x = 1; x < gridWidth - 1; x++) {
				scenicScoreMax = Math.Max(ScenicScore(x, y), scenicScoreMax);
			}
		}

		return scenicScoreMax;

		long ScenicScore(int x, int y) {
			if (x == 0 || y == 0 || x == gridWidth - 1 || y == gridHeight - 1) {
				return 0;
			}

			int tree = grid[x, y];

			return ViewingDistance(-1, 0)
				* ViewingDistance(1, 0)
				* ViewingDistance(0, -1)
				* ViewingDistance(0, 1);

			int ViewingDistance(int dX, int dY) {
				int viewingDistance = 0;
				int x1 = x + dX, y1 = y + dY;
				while (x1 != -1 && y1 != -1 && x1 != gridWidth && y1 != gridHeight) {
					viewingDistance++;
					if (grid[x1, y1] >= tree) {
						break;
					}
					x1 += dX;
					y1 += dY;
				}
				return viewingDistance;
			}
		}
	}

}
