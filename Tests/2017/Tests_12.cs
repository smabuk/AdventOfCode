namespace AdventOfCode.Tests.Year2017;

public class Tests_12_Digital_Plumber
{
	const int DAY = 12;

	[Theory]
	[InlineData("""
		0 <-> 2
		1 <-> 1
		2 <-> 0, 3, 4
		3 <-> 2, 4
		4 <-> 2, 3, 6
		5 <-> 6
		6 <-> 4, 5
		""", 6)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
