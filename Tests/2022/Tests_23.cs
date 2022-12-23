namespace AdventOfCode.Tests._2022;

public class Tests_23_Unstable_Diffusion {
	[Theory]
	[InlineData("""
		##
		#.
		..
		##
		"""
		, 25)]
	[InlineData("""
		....#..
		..###.#
		#...#.#
		.#...##
		#.###..
		##.#.##
		.#..#..
		"""
		, 110)]
	public void Part1(string input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 23, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		....#..
		..###.#
		#...#.#
		.#...##
		#.###..
		##.#.##
		.#..#..
		"""
		, 20)]
	public void Part2(string input, long expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 23, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
