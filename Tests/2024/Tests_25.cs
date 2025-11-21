namespace AdventOfCode.Tests.Year2024;

public class Tests_25_Code_Chronicle
{
	const int DAY = 25;

	[Theory]
	[InlineData("""
		#####
		.####
		.####
		.####
		.#.#.
		.#...
		.....

		#####
		##.##
		.#.##
		...##
		...#.
		...#.
		.....

		.....
		#....
		#....
		#...#
		#.#.#
		#.###
		#####

		.....
		.....
		#.#..
		###..
		###.#
		###.#
		#####

		.....
		.....
		.....
		#....
		#.#..
		#.#.#
		#####
		""", 3)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
