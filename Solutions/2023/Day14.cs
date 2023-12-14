namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 14: Parabolic Reflector Dish
/// https://adventofcode.com/2023/day/14
/// </summary>
[Description("Parabolic Reflector Dish")]
public sealed partial class Day14 {

	public static string Part1(string[] input) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	public const char ROUNDED_ROCK = 'O';
	public const char FLAT_ROCK    = '#';

	private static int Solution1(string[] input) {
		return input
			.AsCells([ROUNDED_ROCK, FLAT_ROCK])
			.TiltNorth()
			.Sum(r => input.Length - r.Y);
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}

}

public static class Day14Helpers
{
	public static IEnumerable<Cell<char>> TiltNorth(this IEnumerable<Cell<char>> rocks)
	{
		List<Cell<char>> theRocks = [.. rocks.OrderBy(r => r.Index.Y)];

		for (int i = 0; i < theRocks.Count; i++) {
			if (theRocks[i].Value is Day14.ROUNDED_ROCK) {
				List<int> yValues = [..
					theRocks
					.Where(r => r.Index.X == theRocks[i].X && r.Index.Y < theRocks[i].Y)
					.Select(r => r.Y)];
				int newY = yValues.Count != 0 ? yValues.Max() + 1 : 0;
				theRocks[i] = theRocks[i] with { Index = new(theRocks[i].X, newY) };
				yield return theRocks[i];
			}
		}
	}
}
