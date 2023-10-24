namespace AdventOfCode.Tests._2022;

public class Tests_24_Blizzard_Basin {
	[Theory(Skip = "I don't have a workable solution for this yet (it works sometimes).")]
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

	[Theory(Skip = "I don't have a workable solution for this yet.")]
	[InlineData("""
		b
		"""
		, 9999)]
	public void Part2(string input, long expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 24, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
