namespace AdventOfCode.Tests.Year2016;

public class Tests_05_How_About_a_Nice_Game_of_Chess
{
	const int DAY = 05;

	[Theory]
	[InlineData("abc", "18f47a30")]
	public void Part1(string input, string expected)
	{
		SolutionRouter.SolveProblem(YEAR, DAY, PART1, input)
			.ShouldBe(expected);
	}

	[Theory]
	[InlineData("abc", "05ace8e3")]
	public void Part2(string input, string expected)
	{
		SolutionRouter.SolveProblem(YEAR, DAY, PART2, input)
			.ShouldBe(expected);
	}
}
