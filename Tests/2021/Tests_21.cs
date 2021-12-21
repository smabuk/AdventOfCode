namespace AdventOfCode.Tests.Year2021;

public class Tests_21_Dirac_Dice {
	[Theory]
	[InlineData(new string[] {
		"Player 1 starting position: 4",
		"Player 2 starting position: 8",
	}, 739785)]
	public void Part1(string[] input, int expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2021, 21, 1, input), out long actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] {
		"Player 1 starting position: 4",
		"Player 2 starting position: 8",
	}, 739785)]
	public void Part2(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2021, 20, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
