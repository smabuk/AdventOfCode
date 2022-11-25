namespace AdventOfCode.Tests.Year2021;

public class Tests_02_Dive {
	[Theory]
	[InlineData(new string[] {
			"forward 5",
			"down 5",
			"forward 8",
			"up 3",
			"down 8",
			"forward 2"
		}, 150)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2021, 2, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] {
			"forward 5",
			"down 5",
			"forward 8",
			"up 3",
			"down 8",
			"forward 2"
		}, 900)]
	public void Part2(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2021, 2, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
