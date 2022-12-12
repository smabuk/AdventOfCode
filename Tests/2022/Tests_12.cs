namespace AdventOfCode.Tests._2022;

public class Tests_12_Hill_Climbing_Algorithm {
	[Theory]
	[InlineData("""
		Sabqponm
		abcryxxl
		accszExk
		acctuvwj
		abdefghi
		"""
		, 31)]
	public void Part1(string input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 12, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		Sabqponm
		abcryxxl
		accszExk
		acctuvwj
		abdefghi
		"""
		, 1, 29)]
	public void Part2(string input, int noOfRounds, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2022, 12, 2, input, noOfRounds), out long actual);
		Assert.Equal(expected, actual);
	}
}
