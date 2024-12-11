namespace AdventOfCode.Tests.Year2024;

public class Tests_11_Plutonian_Pebbles
{
	const int DAY = 11;

	[Theory]
	[InlineData("0 1 10 99 999", 1, 7)]
	[InlineData("125 17", 1, 3)]
	[InlineData("125 17", 2, 4)]
	[InlineData("125 17", 3, 5)]
	[InlineData("125 17", 4, 9)]
	[InlineData("125 17", 5, 13)]
	[InlineData("125 17", 6, 22)]
	[InlineData("125 17", 25, 55312)]
	public void Part1(string input, int noOfBlinks, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, noOfBlinks), out int actual);
		actual.ShouldBe(expected);
	}
}
