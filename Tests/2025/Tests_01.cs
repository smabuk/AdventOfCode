namespace AdventOfCode.Tests.Year2025;

public class Tests_01_
{
	const int DAY = 01;

	private const string TEST_DATA = """
		3   4
		4   3
		2   5
		1   3
		3   9
		3   3
		""";

	[Theory]
	[InlineData(TEST_DATA, 11)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
