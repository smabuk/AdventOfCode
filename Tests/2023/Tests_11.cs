namespace AdventOfCode.Tests.Year2023;

public class Tests_11_Cosmic_Expansion {
	const int DAY = 11;

	private const string TEST_DATA = """
		...#......
		.......#..
		#.........
		..........
		......#...
		.#........
		.........#
		..........
		.......#..
		#...#.....
		""";

	[Theory]
	[InlineData(TEST_DATA, 374)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_DATA,  10, 1030)]
	[InlineData(TEST_DATA, 100, 8410)]
	public void Part2(string input, int scale, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input, scale), out int actual);
		actual.ShouldBe(expected);
	}
}
