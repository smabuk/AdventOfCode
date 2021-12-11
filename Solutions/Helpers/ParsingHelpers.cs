namespace AdventOfCode.Solutions.Helpers;

public static class ParsingHelpers {
	public static IEnumerable<int> AsInts(this IEnumerable<string> input) => input.Select(x => int.Parse(x));
	public static IEnumerable<long> AsLongs(this IEnumerable<string> input) => input.Select(x => long.Parse(x));
	public static IEnumerable<int> AsDigits(this string input) => input.Select(x => int.Parse($"{x}"));

	/// <summary>
	/// Returns Points from an input of (int x,int y) tuples
	/// </summary>
	/// <param name="input"></param>
	/// <returns>IEnumerable<Point></returns>
	public static IEnumerable<Point> AsPoints(this IEnumerable<(int x, int y)> input) =>
		input.Select(p => new Point(X: p.x, Y: p.y));

}
