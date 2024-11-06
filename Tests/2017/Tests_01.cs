namespace AdventOfCode.Tests.Year2017;

public class Tests_01_Inverse_Captcha
{
	const int DAY = 01;

	[Theory]
	[InlineData("1122", 3)]
	[InlineData("1111", 4)]
	[InlineData("1234", 0)]
	[InlineData("91212129", 9)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
