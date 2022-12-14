namespace AdventOfCode.Tests._2022;

public class Tests_14_ {
	[Theory]
	[InlineData("""
		498,4 -> 498,6 -> 496,6
		503,4 -> 502,4 -> 502,9 -> 494,9
		"""
		, 24)]
	public void Part1(string input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 14, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		498,4 -> 498,6 -> 496,6
		503,4 -> 502,4 -> 502,9 -> 494,9
		"""
		, 1, 9999)]
	public void Part2(string input, int noOfRounds, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2022, 14, 2, input, noOfRounds), out long actual);
		Assert.Equal(expected, actual);
	}
}
