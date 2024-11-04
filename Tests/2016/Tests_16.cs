namespace AdventOfCode.Tests.Year2016;

public class Tests_16_Dragon_Checksum
{
	const int DAY = 16;

	[Theory]
	[InlineData("110010110100", 12, "100")]
	[InlineData("10000", 20, "01100")]
	public void Part1(string input, int length, string expected)
	{
		string actual = SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, length);
		actual.ShouldBe(expected);
	}
}
