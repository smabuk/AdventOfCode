namespace AdventOfCode.Tests.Year2023;

public class Tests_15
{
	const int DAY = 15;

	private const string TEST_DATA = """
		
		""";

	[Theory]
	[InlineData(TEST_DATA, 9999)]
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
