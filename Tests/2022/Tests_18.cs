namespace AdventOfCode.Tests._2022;

public class Tests_18_Boiling_Boulders {
	[Theory]
	[InlineData("""
		2,2,2
		1,2,2
		3,2,2
		2,1,2
		2,3,2
		2,2,1
		2,2,3
		2,2,4
		2,2,6
		1,2,5
		3,2,5
		2,1,5
		2,3,5
		"""
		, 64)]
	public void Part1(string input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 18, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		2,2,2
		1,2,2
		3,2,2
		2,1,2
		2,3,2
		2,2,1
		2,2,3
		2,2,4
		2,2,6
		1,2,5
		3,2,5
		2,1,5
		2,3,5
		"""
		, 58)]
	public void Part2(string input, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2022, 18, 2, input), out long actual);
		Assert.Equal(expected, actual);
	}
}
