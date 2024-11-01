namespace AdventOfCode.Tests.Year2016;

public class Tests_07_Internet_Protocol_Version_7
{
	const int DAY = 07;

	[Theory]
	[InlineData("abba[mnop]qrst", 1)]
	[InlineData("abcd[bddb]xyyx", 0)]
	[InlineData("aaaa[qwer]tyui", 0)]
	[InlineData("ioxxoj[asdfgh]zxcvbn", 1)]
	[InlineData("""
		abba[mnop]qrst
		abcd[bddb]xyyx
		aaaa[qwer]tyui
		ioxxoj[asdfgh]zxcvbn
		""", 2)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
