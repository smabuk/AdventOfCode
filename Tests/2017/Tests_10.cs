namespace AdventOfCode.Tests.Year2017;

public class Tests_10_Knot_Hash
{
	const int DAY = 10;

	[Theory]
	[InlineData("""3, 4, 1, 5""", 5, 12)]
	public void Part1(string input, int listSize,  int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, listSize), out int actual);
		actual.ShouldBe(expected);
	}
}
