namespace AdventOfCode.Tests.Year2018;

public class Tests_08_Memory_Maneuver
{
	const int DAY = 8;

	[Theory]
	[InlineData("0 3 1 1 2", 4)]
	[InlineData("2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2", 138)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2", 9999)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input, 2, 0), out int actual);
		Assert.Equal(expected, actual);
	}
}
