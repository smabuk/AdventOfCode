namespace AdventOfCode.Tests.Year2016;

public class Tests_10_Balance_Bots
{
	const int DAY = 10;

	[Theory]
	[InlineData("""
		value 5 goes to bot 2
		bot 2 gives low to bot 1 and high to bot 0
		value 3 goes to bot 1
		bot 1 gives low to output 1 and high to bot 0
		bot 0 gives low to output 2 and high to output 0
		value 2 goes to bot 2
		""", 2, 5, 2)]
	public void Part1(string input, int compare1, int compare2, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, compare1, compare2), out int actual);
		actual.ShouldBe(expected);
	}
}
