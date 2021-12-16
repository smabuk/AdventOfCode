namespace AdventOfCode.Solutions.Helpers;

public static class ParsingHelpers {
	public static IEnumerable<int> AsDigits(this string input) =>
		input.Select(x => int.Parse($"{x}"));

	public static IEnumerable<int> AsInts(this IEnumerable<string> input) =>
		input.Select(x => int.Parse(x));

	public static IEnumerable<long> AsLongs(this IEnumerable<string> input) =>
		input.Select(x => long.Parse(x));

	public static IEnumerable<Point> AsPoints(this IEnumerable<string> input) => 
		input.Select(i => i.Split(",")).Select(x => new Point(int.Parse(x[0]), int.Parse(x[1])));

	/// <summary>
	/// Returns Points from an input of (int x,int y) tuples
	/// </summary>
	/// <param name="input"></param>
	/// <returns>IEnumerable<Point></returns>
	public static IEnumerable<Point> AsPoints(this IEnumerable<(int x, int y)> input) =>
		input.Select(p => new Point(X: p.x, Y: p.y));


	public static string AsBinaryFromHex(this string input) {
		return String.Join(
			String.Empty,
			input.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
	}

	public static string AsBinaryFromHex(this IEnumerable<string> input) {
		return String.Join(String.Empty, input.Select(c => AsBinaryFromHex(c)));
	}
}

