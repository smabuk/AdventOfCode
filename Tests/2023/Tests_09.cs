namespace AdventOfCode.Tests.Year2023;

public class Tests_09_Mirage_Maintenance {
	const int DAY = 9;

	private const string TEST_DATA = """
		0 3 6 9 12 15
		1 3 6 10 15 21
		10 13 16 21 30 45
		""";

	[Theory]
	[InlineData(TEST_DATA, 114)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_DATA, 2)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
