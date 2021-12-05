namespace AdventOfCode.Tests.Year2021;

public class Tests_06_Lanternfish {
	[Theory]
	[InlineData(new string[] {
		"3,4,3,1,2",
	}, 5934)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2021, 6, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] {
		"3,4,3,1,2",
	}, 26984457539)]
	public void Part2(string[] input, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2021, 6, 2, input), out long actual);
		Assert.Equal(expected, actual);
	}
}
