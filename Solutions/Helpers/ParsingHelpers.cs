namespace AdventOfCode.Solutions.Helpers;

public static class ParsingHelpers {
	public static List<int> AsInts(this string[] input) => input.Select(x => int.Parse(x)).ToList();
	public static List<long> AsLongs(this string[] input) => input.Select(x => long.Parse(x)).ToList();
	public static IEnumerable<int> AsDigits(this char[] input) => input.Select(x => int.Parse($"{x}"));
	public static IEnumerable<int> AsDigits(this string input) => input.Select(x => int.Parse($"{x}"));

	/// <summary>
	/// Returns Points from an input of (int x,int y) tuples
	/// </summary>
	/// <param name="input"></param>
	/// <returns>IEnumerable<Point></returns>
	public static IEnumerable<Point> AsPoints(this IEnumerable<(int, int)> input) =>
		input.Select(x => new Point(X: x.Item1, Y: x.Item2));

	/// <summary>
	/// Returns Points from an input of (int x,int y) tuples
	/// </summary>
	/// <param name="input"></param>
	/// <returns>IEnumerable<Point></returns>
	public static List<Point> AsPointsList(this IEnumerable<(int, int)> input) =>
		input.AsPoints().ToList();

}
