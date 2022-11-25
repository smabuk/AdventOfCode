namespace AdventOfCode.Tests.Year2015;

public class Tests_07_Some_Assembly_Required {
	[Theory]
	[InlineData(new string[] {
			"123 -> x",
			"456 -> y",
			"x AND y -> d",
			"x OR y -> e",
			"x LSHIFT 2 -> f",
			"y RSHIFT 2 -> g",
			"NOT x -> h",
			"NOT y -> i",
		}, "e", 507)]
	public void Part1(string[] input, string start, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2015, 7, 1, input, start), out int actual);
		Assert.Equal(expected, actual);
	}
}
