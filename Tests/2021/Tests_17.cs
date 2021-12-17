namespace AdventOfCode.Tests.Year2021;

public class Tests_17_Trick_Shot {
	[Theory]
	[InlineData(new string[] { "target area: x=20..30, y=-10..-5" }, 45)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2021, 17, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] { "" }, 9999)]
	public void Part2(string[] input, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2021, 17, 2, input), out long actual);
		Assert.Equal(expected, actual);
	}
}
