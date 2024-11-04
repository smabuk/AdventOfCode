namespace AdventOfCode.Tests.Year2016;

public class Tests_20_Firewall_Rules
{
	const int DAY = 20;

	[Theory]
	[InlineData("""
		5-8
		0-2
		4-7
		""", 3)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
