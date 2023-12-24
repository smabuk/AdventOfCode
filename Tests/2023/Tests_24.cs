namespace AdventOfCode.Tests.Year2023;

public class Tests_24_Never_Tell_Me_The_Odds
{
	const int DAY = 24;

	private const string TEST_DATA = """
		19, 13, 30 @ -2,  1, -2
		18, 19, 22 @ -1, -1, -2
		20, 25, 34 @ -2, -2, -4
		12, 31, 28 @ -1, -2, -1
		20, 19, 15 @  1, -5, -3
		""";

	[Theory]
	[InlineData(TEST_DATA, 7, 27, 2)]
	public void Part1(string input, long targetMin, long targetMax, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, targetMin, targetMax), out int actual);
		actual.ShouldBe(expected);
	}
}
