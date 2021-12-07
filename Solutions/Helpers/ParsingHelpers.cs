namespace AdventOfCode.Solutions.Helpers;

public static class ParsingHelpers {
	public static List<int> AsInts(this string[] input) => input.Select(x => int.Parse(x)).ToList();
	public static List<long> AsLongs(this string[] input) => input.Select(x => long.Parse(x)).ToList();
	public static T[,] AsArray<T>(this T[] input, int cols, int rows) {
		T[,] result = new T[cols, rows];
		int i = 0;
		for (int r = 0; r < rows; r++) {
			for (int c = 0; c < cols; c++) {
				result[c, r] = input[i++];
			}
		}
		return result;
	}
}
