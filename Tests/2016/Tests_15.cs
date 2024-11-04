namespace AdventOfCode.Tests.Year2016;

public class Tests_15_Timing_is_Everything
{
	const int DAY = 15;

	[Theory]
	[InlineData("""
		Disc #1 has 5 positions; at time=0, it is at position 4.
		Disc #2 has 2 positions; at time=0, it is at position 1.
		""", 5)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
