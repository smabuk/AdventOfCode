namespace AdventOfCode.Tests.Year2024;

public class Tests_02_Red_Nosed_Reports
{
	const int DAY = 02;

	private const string TEST_DATA = """
		7 6 4 2 1
		1 2 7 8 9
		9 7 6 2 1
		1 3 2 4 5
		8 6 4 4 1
		1 3 6 7 9
		""";

	[Theory]
	[InlineData(TEST_DATA, 2)]
	[InlineData("7 6 4 2 1", 1)]
	[InlineData("1 2 7 8 9", 0)]
	[InlineData("9 7 6 2 1", 0)]
	[InlineData("1 3 2 4 5", 0)]
	[InlineData("8 6 4 4 1", 0)]
	[InlineData("1 3 6 7 9", 1)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
