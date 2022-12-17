namespace AdventOfCode.Tests._2022;

public class Tests_17_Pyroclastic_Flow {
	[Theory]
	[InlineData("""
		>>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>
		"""
		, 3068)]
	public void Part1(string input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 17, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		>>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>
		"""
		, 9999)]
	public void Part2(string input, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2022, 17, 2, input), out long actual);
		Assert.Equal(expected, actual);
	}
}
