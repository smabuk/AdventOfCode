namespace AdventOfCode.Tests.Year2017;

public class Tests_23_Coprocessor_Conflagration
{
	const int DAY = 23;

	[Theory]
	[InlineData("""
		mul b 100
		""", 1)]
	[InlineData("""
		set b 57
		set c b
		mul b 1
		mul b 1
		mul b 100
		""", 3)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

}
