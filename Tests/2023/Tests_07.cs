namespace AdventOfCode.Tests.Year2023;

public class Tests_07_Camel_Cards {
	const int DAY = 7;

	private const string TEST_DATA = """
		32T3K 765
		T55J5 684
		KK677 28
		KTJJT 220
		QQQJA 483
		""";

	[Theory]
	[InlineData(TEST_DATA, 6440)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_DATA, 5905)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
