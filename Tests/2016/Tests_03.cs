namespace AdventOfCode.Tests.Year2016;

public class Tests_03_Squares_With_Three_Sides
{
	const int DAY = 03;

	[Theory]
	[InlineData("  5 10 25", 0)]
	[InlineData("  3  4  5", 1)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	private const string TEST_INPUT = """
		101 301 501
		102 302 502
		103 303 503
		201 401 601
		202 402 602
		203 403 603
		""";

	[Theory]
	[InlineData(TEST_INPUT, 6)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
