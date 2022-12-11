namespace AdventOfCode.Tests._2022;

public class Tests_12_e {
	[Theory]
	[InlineData("""
		a
		"""
		, 10605)]
	public void Part1(string input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 12, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		a
		"""
		, 1, 24)]
	public void Part2(string input, int noOfRounds, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2022, 12, 2, input, noOfRounds), out long actual);
		Assert.Equal(expected, actual);
	}
}
