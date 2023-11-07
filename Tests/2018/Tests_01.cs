namespace AdventOfCode.Tests.Year2018;

public class Tests_01_Chronal_Calibration {

	const int DAY = 1;

	[Theory]
	[InlineData((string[])(["+1", "-2", "+3", "+1"]),  3)]
	[InlineData((string[])(["+1", "+1", "+1"])      ,  3)]
	[InlineData((string[])(["+1", "+1", "-2"])      ,  0)]
	[InlineData((string[])(["-1", "-2", "-3"])      , -6)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData((string[])(["+1", "-2", "+3", "+1"])      ,  2)]
	[InlineData((string[])(["+1", "-1"])                  ,  0)]
	[InlineData((string[])(["+3", "+3", "+4", "-2", "-4"]), 10)]
	[InlineData((string[])(["-6", "+3", "+8", "+5", "-6"]),  5)]
	[InlineData((string[])(["+7", "+7", "-2", "-7", "-4"]), 14)]
	public void Part2(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
