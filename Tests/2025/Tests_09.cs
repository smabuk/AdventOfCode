namespace AdventOfCode.Tests.Year2025;

[SupportTestOutput]
public partial class Tests_09_Movie_Theater
{
	const int DAY = 09;

	private const string TEST_DATA =
		"""
		7,1
		11,1
		11,7
		9,7
		9,5
		2,5
		2,3
		7,3
		""";

	[Theory]
	[InlineData(TEST_DATA, 50)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_DATA, 24)]
	public void Part2(string input, long expected)
	{
		_ = long.TryParse(SolveProblem(YEAR, DAY, PART2, input), out long actual);
		actual.ShouldBe(expected);
	}

}
