namespace AdventOfCode.Tests.Year2021;

public class Tests_25_Sea_Cucumber {
	[Theory]
	[InlineData(new string[] {
		"v...>>.vv>",
		".vv>>.vv..",
		">>.>v>...v",
		">>v>>.>.v.",
		"v>v.vv.v..",
		">.>>..v...",
		".vv..>.>v.",
		"v.v..>>v.v",
		"....v..v.>",
	}, 58)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2021, 25, 1, input, true), out int actual);
		Assert.Equal(expected, actual);
	}

}