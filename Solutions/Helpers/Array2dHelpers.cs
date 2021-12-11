namespace AdventOfCode.Solutions.Helpers;

public static partial class ArrayHelpers {

	public static readonly List<(int dX, int dY)> CARDINAL_DIRECTIONS = new()
		{ (0, -1), (0, 1), (-1, 0), (1, 0) };
	public static readonly List<(int dX, int dY)> ORDINAL_DIRECTIONS = new()
		{ (-1, -1), (-1, 1), (1, 1), (1, -1) };
	public static readonly List<(int dX, int dY)> ALL_DIRECTIONS = CARDINAL_DIRECTIONS.Union(ORDINAL_DIRECTIONS).ToList();

	public static T[,] To2dArray<T>(this IEnumerable<T> input, int cols, int? rows = null) {
		T[] array = input.ToArray();
		int arrayLength = array.Length;
		rows ??= arrayLength % cols == 0 ? arrayLength / cols : (arrayLength / cols) + 1;
		T[,] result = new T[cols, (int)rows];
		int i = 0;
		for (int r = 0; r < rows; r++) {
			for (int c = 0; c < cols; c++) {
				result[c, r] = array[i++];
				if (i >= arrayLength) {
					return result;
				}
			}
		}
		return result;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="array"></param>
	/// <returns></returns>
	public static IEnumerable<(int X, int Y, T Value)> Walk2dArrayWithValues<T>(this T[,] array) {
		int cols = array.GetUpperBound(0);
		int rows = array.GetUpperBound(1);

		for (int row = 0; row <= rows; row++) {
			for (int col = 0; col <= cols; col++) {
				yield return new(col, row, array[col, row]);
			}
		}
	}
	public static IEnumerable<(int X, int Y)> Walk2dArray<T>(this T[,] array) {
		return array
			.Walk2dArrayWithValues()
			.Select(cell => (cell.X, cell.Y));
	}


	public static IEnumerable<(int x, int y, T value)> GetAdjacentCells<T>(this T[,] array, int x, int y, bool includeDiagonals = false) {
		int cols = array.GetUpperBound(0);
		int rows = array.GetUpperBound(1);
		IEnumerable<(int dX, int dY)> DIRECTIONS = includeDiagonals switch {
			true => ALL_DIRECTIONS,
			false => CARDINAL_DIRECTIONS,
		};

		foreach ((int dX, int dY) in DIRECTIONS) {
			int newX = x + dX;
			int newY = y + dY;
			if (newX >= 0 && newX <= cols && newY >= 0 && newY <= rows) {
				yield return (newX, newY, array[newX, newY]);
			}
		}
	}

	public static IEnumerable<(int x, int y, T value)> GetAdjacentCells<T>(this T[,] array, (int x, int y) point, bool includeDiagonals = false) {
		return GetAdjacentCells<T>(array, point.x, point.y, includeDiagonals);
	}

	public static IEnumerable<string> PrintAsStringArray<T>(this T[,] array, int width = 0) where T : struct {
		for (int r = 0; r <= array.GetUpperBound(1); r++) {
			string line = "";
			for (int c = 0; c <= array.GetUpperBound(0); c++) {
				string cell = array[c, r].ToString() ?? "";
				line += $"{new string(' ', width == 0 ? 1 : width - cell.Length)}{cell}";
			}
			yield return line;
		}
	}


}
