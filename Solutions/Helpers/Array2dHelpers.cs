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

	public static T[,] To2dArray<T>(this IEnumerable<Point> input, T initial, T value) {
		int cols = input.Select(i => i.X).Max() + 1;
		int rows = input.Select(i => i.Y).Max() + 1;

		T[,] result = new T[cols, (int)rows];
		for (int r = 0; r < rows; r++) {
			for (int c = 0; c < cols; c++) {
				result[c, r] = initial;
			}
		}
		foreach (Point p in input) {
			result[p.X, p.Y] = value;
		}

		return result;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="array"></param>
	/// <returns></returns>
	public static IEnumerable<Cell<T>> Walk2dArrayWithValues<T>(this T[,] array) {
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
			.Select(cell => (cell.Index.X, cell.Index.Y));
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

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="array"></param>
	/// <param name="width">If -1 then no spaces. If 0 then 1 padded space, otherwise the width of columns required</param>
	/// <returns></returns>
	public static IEnumerable<string> PrintAsStringArray<T>(this T[,] array, int? width = null) where T : struct {
		for (int r = 0; r <= array.GetUpperBound(1); r++) {
			string line = "";
			for (int c = 0; c <= array.GetUpperBound(0); c++) {
				string cell = array[c, r].ToString() ?? "";
				line += width switch {
					0 => $"{cell}",
					_ => $"{new string(' ', (width - cell.Length) ?? 1)}{cell}",
				};
			}
			yield return line;
		}
	}


}
