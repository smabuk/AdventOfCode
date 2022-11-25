namespace AdventOfCode.Tests.Year2015;

public class Tests_10_Elves_Look_Elves_Say {
	[Theory]
	[InlineData(new string[] { "1" }, 82350)]
	[InlineData(new string[] { "11" }, 107312)]
	[InlineData(new string[] { "21" }, 139984)]
	[InlineData(new string[] { "1211" }, 182376)]
	[InlineData(new string[] { "11121" }, 210920)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2015, 10, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] { "1" }, 1166642)]
	[InlineData(new string[] { "11" }, 1520986)]
	[InlineData(new string[] { "21" }, 1982710)]
	[InlineData(new string[] { "1211" }, 2584304)]
	[InlineData(new string[] { "11121" }, 2990094)]
	public void Part2(string[] input, int expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2015, 10, 2, input), out long actual);
		Assert.Equal(expected, actual);
	}

}
