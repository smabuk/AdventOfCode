namespace AdventOfCode.Tests.Year2023;

public class Tests_14_Parabolic_Reflector_Dish
{
	const int DAY = 14;

	private const string TEST_DATA = """
		O....#....
		O.OO#....#
		.....##...
		OO.#O....O
		.O.....O#.
		O.#..O.#.#
		..O..#O..O
		.......O..
		#....###..
		#OO..#....
		
		""";

	[Theory]
	[InlineData(TEST_DATA, 136)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_DATA, 64)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
