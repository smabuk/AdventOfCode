namespace AdventOfCode.Tests._2015;

public class Tests_08_Matchsticks {
	[Theory]
	[InlineData(new string[] { @"""""" }, 2)]
	[InlineData(new string[] { @"""abc""" }, 2)]
	[InlineData(new string[] { @"""aaa\""aaa""" }, 3)]
	[InlineData(new string[] { @"""\x27""" }, 5)]
	[InlineData(new string[] {
			@"""""",
			@"""abc""",
			@"""aaa\""aaa""",
			@"""\x27"""
		}, 12)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2015, 8, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] { @"""""" }, 4)]
	[InlineData(new string[] { @"""abc""" }, 4)]
	[InlineData(new string[] { @"""aaa\""aaa""" }, 6)]
	[InlineData(new string[] { @"""\x27""" }, 5)]
	[InlineData(new string[] {
			@"""""",
			@"""abc""",
			@"""aaa\""aaa""",
			@"""\x27"""
		}, 19)]
	public void Part2(string[] input, int expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2015, 8, 2, input), out long actual);
		Assert.Equal(expected, actual);
	}

}
