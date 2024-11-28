namespace AdventOfCode.Tests.Year2017;

public class Tests_11_Hex_Ed
{
	const int DAY = 11;

	[Theory]
	[InlineData("ne,ne,ne", 3)]
	[InlineData("ne,ne,sw,sw", 0)]
	[InlineData("ne,ne,s,s", 2)]
	[InlineData("se,sw,se,sw,sw", 3)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
