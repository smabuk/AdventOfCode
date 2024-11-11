namespace AdventOfCode.Tests.Year2017;

public class Tests_15_Dueling_Generators
{
	const int DAY = 15;

	[Theory]
	[InlineData("""
		Generator A starts with 65
		Generator B starts with 8921
		""", 588)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData("""
		Generator A starts with 65
		Generator B starts with 8921
		""", 309)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
