namespace AdventOfCode.Tests._2015;

public class Tests_04_The_Ideal_Stocking_Stuffer {
	[Theory]
	[InlineData(new string[] { "abcdef" }, 609043)]
	[InlineData(new string[] { "pqrstuv" }, 1048970)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2015, 04, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

}
