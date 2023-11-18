namespace AdventOfCode.Tests.Year2018;

public class Tests_16
{
	const int DAY = 16;

	[Theory]
	[InlineData("""
		Before: [3, 2, 1, 1]
		9 2 1 2
		After:  [3, 2, 2, 1]



		0 0 0 0
		1 1 1 1
		""", 1)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
