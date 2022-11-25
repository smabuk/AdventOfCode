namespace AdventOfCode.Tests.Year2021;

public class Tests_01_Sonar_Sweep {
	[Theory]
	[InlineData(new string[] {
			"199",
			"200",
			"208",
			"210",
			"200",
			"207",
			"240",
			"269",
			"260",
			"263"
		}, 7)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2021, 1, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] {
			"199",
			"200",
			"208",
			"210",
			"200",
			"207",
			"240",
			"269",
			"260",
			"263"
		}, 5)]
	public void Part2(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2021, 1, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
