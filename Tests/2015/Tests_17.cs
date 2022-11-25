namespace AdventOfCode.Tests._2015;

public class Tests_17_No_Such_Thing_as_Too_Much {
	[Theory]
	[InlineData(new string[] {
			"20", "15", "10", "5", "5"
		}, 25, 4)]
	public void Part1(string[] input, int noOfLiters, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2015, 17, 1, input, noOfLiters), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] {
			"20", "15", "10", "5", "5"
		}, 25, 3)]
	public void Part2(string[] input, int noOfLiters, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2015, 17, 2, input, noOfLiters), out int actual);
		Assert.Equal(expected, actual);
	}
}
