namespace AdventOfCode.Solutions.Helpers;

public static class ParsingHelpers {
	public static List<int> AsInts(this string[] input) => input.Select(x => int.Parse(x)).ToList();
	public static List<long> AsLongs(this string[] input) => input.Select(x => long.Parse(x)).ToList();
	public static IEnumerable<int> AsDigits(this char[] input) => input.Select(x => int.Parse($"{x}"));
	public static IEnumerable<int> AsDigits(this string input) => input.Select(x => int.Parse($"{x}"));
}
