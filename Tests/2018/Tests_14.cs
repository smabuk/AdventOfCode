namespace AdventOfCode.Tests.Year2018;

public class Tests_14_Chocolate_Charts
{
	const int DAY = 14;

	[Theory]
	[InlineData("9", "5158916779")]
	[InlineData("5", "0124515891")]
	[InlineData("18", "9251071085")]
	[InlineData("2018", "5941429882")]
	public void Part1(string input, string expected)
	{
		string actual = SolutionRouter.SolveProblem(YEAR, DAY, PART1, input);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("51589", 9 )]
	[InlineData("01245", 5 )]
	[InlineData("92510", 18)]
	[InlineData("59414", 2018)]
	public void Part21(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		Assert.Equal(expected, actual);
	}

}
