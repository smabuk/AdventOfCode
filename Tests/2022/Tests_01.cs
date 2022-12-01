namespace AdventOfCode.Tests._2022;

public class Tests_01_Calorie_Counting {
	[Theory]
	[InlineData("""
		1000
		2000
		3000

		4000

		5000
		6000

		7000
		8000
		9000

		10000
		"""
		, 24000)]
	public void Part1(string input, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2022, 1, 1, input), out long actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		1000
		2000
		3000

		4000

		5000
		6000

		7000
		8000
		9000

		10000
		"""
		, 45000)]
	public void Part2(string input, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2022, 1, 2, input), out long actual);
		Assert.Equal(expected, actual);
	}
}
