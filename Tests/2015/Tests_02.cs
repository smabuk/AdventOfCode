namespace AdventOfCode.Tests._2015;

public class Tests_02_I_Was_Told_There_Would_Be_No_Math {
	[Theory]
	[InlineData(new string[] { "2x3x4" }, 58)]
	[InlineData(new string[] { "1x1x10" }, 43)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2015, 02, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] { "2x3x4" }, 34)]
	[InlineData(new string[] { "1x1x10" }, 14)]
	public void Part2(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2015, 02, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
