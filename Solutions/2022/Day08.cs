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
				if (IsVisible(x, y, grid)) {
					visibleTrees++;
				}
			}
		}

		return visibleTrees;
	}

	private static long Solution2(string[] input) {
		long scenicScoreMax = 0;
		int[,] grid = input.SelectMany(i => i.AsDigits()).To2dArray(input[0].Length);
		int gridWidth = grid.GetLength(0);
		int gridHeight = grid.GetLength(1);

		for (int y = 1; y < gridHeight - 1; y++) {
			for (int x = 1; x < gridWidth - 1; x++) {
				long scenicScore = ScenicScore(x, y, grid);
				if (scenicScore > scenicScoreMax) {
					scenicScoreMax = scenicScore;
				}
			}
		}

		return scenicScoreMax;
	}

	static bool IsVisible(int x, int y, int[,] grid) {
		int gridWidth = grid.GetLength(0);
		int gridHeight = grid.GetLength(1);

		if (x == 0 || y == 0 || x == gridWidth - 1 || y == gridHeight - 1) {
			return true;
		}

		int tree = grid[x, y];

		for (int x1 = x - 1; x1 >= 0; x1--) {
			int currTree = grid[x1, y];
			if (currTree >= tree) {
				break;
			} else {
				if (x1 == 0) {
					return true;
				}
			}
		}
		for (int x1 = x + 1; x1 <= gridWidth - 1; x1++) {
			int currTree = grid[x1, y];
			if (currTree >= tree) {
				break;
			} else {
				if (x1 == gridWidth - 1) {
					return true;
				}
			}
		}
		for (int y1 = y - 1; y1 >= 0; y1--) {
			int currTree = grid[x, y1];
			if (currTree >= tree) {
				break;
			} else {
				if (y1 == 0) {
					return true;
				}
			}
		}
		for (int y1 = y + 1; y1 <= gridHeight - 1; y1++) {
			int currTree = grid[x, y1];
			if (currTree >= tree) {
				break;
			} else {
				if (y1 == gridHeight - 1) {
					return true;
				}
			}
		}
		return false;
	}

	static long ScenicScore (int x, int y, int[,] grid) {
		int gridWidth = grid.GetLength(0);
		int gridHeight = grid.GetLength(1);
		int s1 = 0;
		int s2 = 0;
		int s3 = 0;
		int s4 = 0;

		if (x == 0 || y == 0 || x == gridWidth - 1 || y == gridHeight - 1) {
			return 0;
		}
		int tree = grid[x, y];
		for (int x1 = x - 1; x1 >= 0; x1--) {
			s1++;
			int currTree = grid[x1, y];
			if (currTree >= tree) {
				break;
			}
		}
		for (int x1 = x + 1; x1 <= gridWidth - 1; x1++) {
			s2++;
			int currTree = grid[x1, y];
			if (currTree >= tree) {
				break;
			}
		}
		for (int y1 = y - 1; y1 >= 0; y1--) {
			s3++;
			int currTree = grid[x, y1];
			if (currTree >= tree) {
				break;
			}
		}
		for (int y1 = y + 1; y1 <= gridHeight - 1; y1++) {
			s4++;
			int currTree = grid[x, y1];
			if (currTree >= tree) {
				break;
			}
		}
		return s1 * s2 * s3 * s4;
	}
}
