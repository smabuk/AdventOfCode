namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 08: Treetop Tree House
/// https://adventofcode.com/2022/day/8
/// </summary>
[Description("Treetop Tree House")]
public sealed partial class Day08 {

	[Init]
	public static    void Init(string[] input, params object[]? _) => CreateMap(input);
	public static string Part1(string[] input, params object[]? _) => Solution1().ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2().ToString();

	private static int[,] _heightMap = default!;

	private static void CreateMap(string[] input) {
		_heightMap = input.SelectMany(i=> i.AsDigits<int>()).To2dArray(input[0].Length);
	}

	private static int Solution1() {
		int visibleTrees = 0;
		int columns = _heightMap.ColsCount();
		int rows = _heightMap.RowsCount();

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

			int tree = _heightMap[col, row];

			return IsVisibleInDirection(-1,  0)
				|| IsVisibleInDirection( 1,  0)
				|| IsVisibleInDirection( 0, -1)
				|| IsVisibleInDirection( 0,  1);

			bool IsVisibleInDirection(int dX, int dY) {
				int newCol = col + dX, newRow = row + dY;
				while (newCol != -1 && newRow != -1 && newCol != columns && newRow != rows) {
					if (_heightMap[newCol, newRow] >= tree) {
						return false;
					}
					newCol += dX;
					newRow += dY;
				}
				return true;
			}
		}
	}

	private static long Solution2() {
		long scenicScoreMax = 0;
		int columns = _heightMap.ColsCount();
		int rows = _heightMap.RowsCount();

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

			int tree = _heightMap[col, row];

			return ViewingDistance(-1,  0)
				 * ViewingDistance( 1,  0)
				 * ViewingDistance(0 , -1)
				 * ViewingDistance( 0,  1);

			int ViewingDistance(int dX, int dY) {
				int viewingDistance = 0;
				int newCol = col + dX, newRow = row + dY;
				while (newCol != -1 && newRow != -1 && newCol != columns && newRow != rows) {
					viewingDistance++;
					if (_heightMap[newCol, newRow] >= tree) {
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
