namespace AdventOfCode.Tests.Year2018;

public class Tests_03_Matter_How_You_Slice_It
{
	const int DAY = 3;

	[Theory]
	[InlineData((string[])([
		"#1 @ 1,3: 4x4",
		"#2 @ 3,1: 4x4",
		"#3 @ 5,5: 2x2",
		]), 4)]
	public void Part1(string[] input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData((string[])([
		"#1 @ 1,3: 4x4",
		"#2 @ 3,1: 4x4",
		"#3 @ 5,5: 2x2",
	]), 3)]
	public void Part2(string[] input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
