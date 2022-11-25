namespace AdventOfCode.Tests.Year2020;

public class Tests_05_Binary_Boarding {
	[Theory]
	[InlineData(new string[] { "BFFFBBFRRR" }, 567)]
	[InlineData(new string[] { "FFFBBBFRRR" }, 119)]
	[InlineData(new string[] { "BBFFBBFRLL" }, 820)]
	[InlineData(new string[] { "BBFFBBFRRL" }, 822)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2020, 5, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Part2_Binary_Boarding() {
		string[]? input = Helpers.GetInputData(2020, 5);
		int actual = 0;
		if (input is not null) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2020, 5, 2, input), out actual);
		}
		Assert.Equal(671, actual);
	}
}
