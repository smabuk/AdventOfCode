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

	//[Theory]
	//[InlineData("  5 10 25", 9999)]
	//public void Part2(string input, int expected)
	//{
	//	_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
	//	actual.ShouldBe(expected);
	//}
}
