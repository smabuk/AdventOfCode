namespace AdventOfCode.Tests.Year2023;

public class Tests_13_Point_of_Incidence
{
	const int DAY = 13;

	private const string TEST_DATA = """
		#.##..##.
		..#.##.#.
		##......#
		##......#
		..#.##.#.
		..##..##.
		#.#.##.#.

		#...##..#
		#....#..#
		..##..###
		#####.##.
		#####.##.
		..##..###
		#....#..#
		""";

	[Theory]
	[InlineData(TEST_DATA, 405)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
