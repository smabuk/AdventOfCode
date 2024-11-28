namespace AdventOfCode.Tests.Year2017;

public class Tests_24_Electromagnetic_Moat
{
	const int DAY = 24;

	[Theory]
	[InlineData("""
		0/2
		2/2
		2/3
		3/4
		3/5
		0/1
		10/1
		9/10
		""", 31)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData("""
		0/2
		2/2
		2/3
		3/4
		3/5
		0/1
		10/1
		9/10
		""", 19)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}

}
