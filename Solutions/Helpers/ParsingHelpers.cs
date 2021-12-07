namespace AdventOfCode.Solutions.Helpers;

public static class ParsingHelpers {
	public static List<int> AsInts(this string[] input) => input.Select(x => int.Parse(x)).ToList();
	public static List<long> AsLongs(this string[] input) => input.Select(x => long.Parse(x)).ToList();
}
