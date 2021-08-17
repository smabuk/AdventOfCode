namespace AdventOfCode.Tests.Year2020;

public class Tests_15_Rambunctious_Recitation {
	[Theory]
	[InlineData(new string[] {
			"0,3,6"
		}, 436)]
	[InlineData(new string[] {
			"1,3,2"
		}, 1)]
	[InlineData(new string[] {
			"2,1,3"
		}, 10)]
	[InlineData(new string[] {
			"1,2,3"
		}, 27)]
	[InlineData(new string[] {
			"2,3,1"
		}, 78)]
	[InlineData(new string[] {
			"3,2,1"
		}, 438)]
	[InlineData(new string[] {
			"3,1,2"
		}, 1836)]
	[InlineData(new string[] {
			"9,12,1,4,17,0,18"
		}, 610)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2020, 15, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory(Skip = "Solution is too slow - takes about 15s per test")]
	[InlineData(new string[] {
			"0,3,6"
		}, 175594)]
	[InlineData(new string[] {
			"1,3,2"
		}, 2578)]
	[InlineData(new string[] {
			"2,1,3"
		}, 3544142)]
	[InlineData(new string[] {
			"1,2,3"
		}, 261214)]
	[InlineData(new string[] {
			"2,3,1"
		}, 6895259)]
	[InlineData(new string[] {
			"3,2,1"
		}, 18)]
	[InlineData(new string[] {
			"3,1,2"
		}, 362)]
	[InlineData(new string[] {
			"9,12,1,4,17,0,18"
		}, 1407)]
	public void Part2(string[] input, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2020, 15, 2, input), out long actual);
		Assert.Equal(expected, actual);
	}

}
