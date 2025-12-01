namespace AdventOfCode.Tests.Year2025;

public class Tests_01_Secret_Entrance
{
	const int DAY = 01;

	private const string TEST_DATA = """
		L68
		L30
		R48
		L5
		R60
		L55
		L1
		L99
		R14
		L82
		""";

	[Theory]
	[InlineData(TEST_DATA, 3)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_DATA, 6)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
