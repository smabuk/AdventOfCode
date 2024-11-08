namespace AdventOfCode.Tests.Year2017;

public class Tests_06_Memory_Reallocation
{
	const int DAY = 06;

	[Theory]
	[InlineData("0\t2\t7\t0", 5)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData("0\t2\t7\t0", 4)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
