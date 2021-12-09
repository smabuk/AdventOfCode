namespace AdventOfCode.Tests.Year2021;

public class Tests_09_Smoke_Basin {
	[Theory]
	[InlineData(new string[] {
		"2199943210",
		"3987894921",
		"9856789892",
		"8767896789",
		"9899965678",
	}, 15)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2021, 9, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] {
		"2199943210",
		"3987894921",
		"9856789892",
		"8767896789",
		"9899965678",
	}, 1134)]
	public void Part2(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2021, 9, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
