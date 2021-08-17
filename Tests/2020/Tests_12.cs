namespace AdventOfCode.Tests.Year2020;

public class Tests_12_Rain_Risk {
	[Theory]
	[InlineData(new string[] {
			"F10",
			"N3",
			"F7",
			"R90",
			"F11"
		}, 25)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2020, 12, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] {
			"F10",
			"N3",
			"F7",
			"R90",
			"F11"
		}, 286)]
	public void Part2(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2020, 12, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}


}
