namespace AdventOfCode.Tests._2015;

public class Tests_25_It_Let_It_Snow {
	[Theory]
	[InlineData("To continue, please consult the code grid in the manual.  Enter the code at row 2, column 1."
		, 31916031)]
	[InlineData("To continue, please consult the code grid in the manual.  Enter the code at row 5, column 6."
		, 31663883)]
	public void Part1(string input, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2015, 25, 1, input), out long actual);
		Assert.Equal(expected, actual);
	}
}
