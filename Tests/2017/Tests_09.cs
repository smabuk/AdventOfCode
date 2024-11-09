namespace AdventOfCode.Tests.Year2017;

public class Tests_09_Stream_Processing
{
	const int DAY = 09;

	[Theory]
	[InlineData("""{}""", 1)]
	[InlineData("""{{{}}}""", 6)]
	[InlineData("""{{},{}}""", 5)]
	[InlineData("""{{{},{},{{}}}}""", 16)]
	[InlineData("""{<a>,<a>,<a>,<a>}""", 1)]
	[InlineData("""{{<ab>},{<ab>},{<ab>},{<ab>}}""", 9)]
	[InlineData("""{{<!!>},{<!!>},{<!!>},{<!!>}}""", 9)]
	[InlineData("""{{<a!>},{<a!>},{<a!>},{<ab>}}""", 3)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
