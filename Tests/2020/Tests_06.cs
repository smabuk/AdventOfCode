namespace AdventOfCode.Tests.Year2020;

public class Tests_06 {
	[Theory]
	[InlineData(new string[] {
			"abc",
			"",
			"a",
			"b",
			"c",
			"",
			"ab",
			"ac",
			"",
			"a",
			"a",
			"a",
			"a",
			"",
			"b"
		}, 11)]
	public void Part1_Custom_Customs(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2020, 6, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] {
			"abc",
			"",
			"a",
			"b",
			"c",
			"",
			"ab",
			"ac",
			"",
			"a",
			"a",
			"a",
			"a",
			"",
			"b"
		}, 6)]
	public void Part2_Custom_Customs(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2020, 6, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}

}
