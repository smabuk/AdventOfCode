namespace AdventOfCode.Tests._2022;

public class Tests_24_Blizzard_Basin {
	[Theory]
	[InlineData("""
		#.#####
		#.....#
		#>....#
		#.....#
		#...v.#
		#.....#
		#####.#
		"""
		, 10)]
	[InlineData("""
		#.######
		#>>.<^<#
		#.<..<<#
		#>v.><>#
		#<^v^^>#
		######.#
		"""
		, 18)]
	public void Part1(string input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 24, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		b
		"""
		, 9999)]
	public void Part2(string input, long expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 24, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
