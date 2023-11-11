namespace AdventOfCode.Tests.Year2018;

public class Tests_09_Marble_Mania
{
	const int DAY = 9;

	[Theory]
	[InlineData("9 players; last marble is worth 25 points",        32)]
	[InlineData("10 players; last marble is worth 1618 points",   8317)]
	[InlineData("13 players; last marble is worth 7999 points", 146373)]
	[InlineData("17 players; last marble is worth 1104 points",   2764)]
	[InlineData("21 players; last marble is worth 6111 points",  54718)]
	[InlineData("30 players; last marble is worth 5807 points",  37305)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("9 players; last marble is worth 25 points",    9999)]
	[InlineData("10 players; last marble is worth 1618 points", 9999)]
	[InlineData("13 players; last marble is worth 7999 points", 9999)]
	[InlineData("17 players; last marble is worth 1104 points", 9999)]
	[InlineData("21 players; last marble is worth 6111 points", 9999)]
	[InlineData("30 players; last marble is worth 5807 points", 9999)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input, 2, 0), out int actual);
		Assert.Equal(expected, actual);
	}
}
