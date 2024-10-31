namespace AdventOfCode.Tests.Year2016;

public class Tests_01_No_Time_for_a_Taxicab
{
	const int DAY = 01;

	[Theory]
	[InlineData("R2, L3", 5)]
	[InlineData("R2, R2, R2", 2)]
	[InlineData("R5, L5, R5, R3", 12)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	//[Theory]
	//[InlineData(TEST_DATA, 9999)]
	//public void Part2(string input, int expected)
	//{
	//	_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
	//	actual.ShouldBe(expected);
	//}
}
