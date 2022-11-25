namespace AdventOfCode.Tests._2015;

public class Tests_20_Infinite_Elves_and_Infinite_Houses {
	[Theory]
	[InlineData(new string[] {
			"30"
		}, 2)]
	[InlineData(new string[] {
			"85"
		}, 6)]
	[InlineData(new string[] {
			"140"
		}, 8)]
	[InlineData(new string[] {
			"36000000"
		}, 831600)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2015, 20, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] {
			"30"
		}, 2)]
	[InlineData(new string[] {
			"43"
		}, 3)]
	[InlineData(new string[] {
			"45"
		}, 4)]
	[InlineData(new string[] {
			"36000000"
		}, 884520)]
	public void Part2(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2015, 20, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
