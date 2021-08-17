namespace AdventOfCode.Tests.Year2015;

public class Tests_01_Not_Quite_Lisp {
	[Theory]
	[InlineData(new string[] { "(())" }, 0)]
	[InlineData(new string[] { "()()" }, 0)]
	[InlineData(new string[] { "(((" }, 3)]
	[InlineData(new string[] { "(()(()(" }, 3)]
	[InlineData(new string[] { "())" }, -1)]
	[InlineData(new string[] { "))(" }, -1)]
	[InlineData(new string[] { ")))" }, -3)]
	[InlineData(new string[] { ")())())" }, -3)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2015, 01, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] { ")" }, 1)]
	[InlineData(new string[] { "()())" }, 5)]
	public void Part2(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2015, 01, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
