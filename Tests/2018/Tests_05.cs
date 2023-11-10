namespace AdventOfCode.Tests.Year2018;

public class Tests_05_Alchemical_Reaction
{
	const int DAY = 5;

	[Theory]
	[InlineData("aA", 0)]
	[InlineData("abBA", 0)]
	[InlineData("abAB", 4)]
	[InlineData("aabAAB", 6)]
	[InlineData("dabAcCaCBAcCcaDA", 10)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("aA", 9999)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
