namespace AdventOfCode.Tests.Year2018;

public class Tests_06_Chronal_Coordinates
{
	const int DAY = 6;

	[Theory]
	[InlineData("""
		1, 1
		1, 6
		8, 3
		3, 4
		5, 5
		8, 9
		"""
		, 17)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		1, 1
		1, 6
		8, 3
		3, 4
		5, 5
		8, 9
		"""
		, 16)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input, 32), out int actual);
		Assert.Equal(expected, actual);
	}
}
