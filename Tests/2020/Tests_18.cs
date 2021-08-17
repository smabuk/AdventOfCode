namespace AdventOfCode.Tests.Year2020;

public class Tests_18_Operation_Order {
	[Theory]
	[InlineData(new string[] {
			"1 + 2 * 3 + 4 * 5 + 6"
		}, 71)]
	[InlineData(new string[] {
			"1 + (2 * 3) + (4 * (5 + 6))"
		}, 51)]
	[InlineData(new string[] {
			"2 * 3 + (4 * 5)"
		}, 26)]
	[InlineData(new string[] {
			"5 + (8 * 3 + 9 + 3 * 4 * 3)"
		}, 437)]
	[InlineData(new string[] {
			"5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))"
		}, 12240)]
	[InlineData(new string[] {
			"((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2"
		}, 13632)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2020, 18, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] {
			"1 + 2 * 3 + 4 * 5 + 6"
		}, 231)]
	[InlineData(new string[] {
			"1 + (2 * 3) + (4 * (5 + 6))"
		}, 51)]
	[InlineData(new string[] {
			"2 * 3 + (4 * 5)"
		}, 46)]
	[InlineData(new string[] {
			"5 + (8 * 3 + 9 + 3 * 4 * 3)"
		}, 1445)]
	[InlineData(new string[] {
			"5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))"
		}, 669060)]
	[InlineData(new string[] {
			"((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2"
		}, 23340)]
	public void Part2(string[] input, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2020, 18, 2, input), out long actual);
		Assert.Equal(expected, actual);
	}

}
