namespace AdventOfCode.Tests.Year2021;

public class Tests_07_The_Treachery_of_Whales {
	[Theory]
	[InlineData(new string[] { "16,1,2,0,4,2,7,1,2,14" }, 37)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2021, 7, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] { "16,1,2,0,4,2,7,1,2,14" }, 168)]
	public void Part2(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2021, 7, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
