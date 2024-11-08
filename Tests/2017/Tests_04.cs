namespace AdventOfCode.Tests.Year2017;

public class Tests_04_High_Entropy_Passphrases
{
	const int DAY = 04;

	[Theory]
	[InlineData("aa bb cc dd ee", 1)]
	[InlineData("aa bb cc dd aa", 0)]
	[InlineData("aa bb cc dd aaa", 1)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
