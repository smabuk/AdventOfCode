namespace AdventOfCode.Tests.Year2017;

public class Tests_17_Spinlock
{
	const int DAY = 17;

	[Theory]
	[InlineData("3", 638)]
	public void Part1(string input,int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
