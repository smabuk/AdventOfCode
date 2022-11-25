namespace AdventOfCode.Tests.Year2019;

public class Tests_01_The_Tyranny_of_the_Rocket_Equation {
	[Theory]
	[InlineData(new string[] {
			"12"
		}, 2)]
	[InlineData(new string[] {
			"14"
		}, 2)]
	[InlineData(new string[] {
			"1969"
		}, 654)]
	[InlineData(new string[] {
			"100756"
		}, 33583)]
	[InlineData(new string[] {
			"12",
			"14",
			"1969",
			"100756"
		}, 34241)]
	public void Part1(string[] input, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2019, 1, 1, input), out long actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] {
			"14"
		}, 2)]
	[InlineData(new string[] {
			"1969"
		}, 966)]
	[InlineData(new string[] {
			"100756"
		}, 50346)]
	[InlineData(new string[] {
			"14",
			"1969",
			"100756"
		}, 51314)]
	public void Part2(string[] input, int expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2019, 1, 2, input), out long actual);
		Assert.Equal(expected, actual);
	}
}
