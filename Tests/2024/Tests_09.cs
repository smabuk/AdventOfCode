namespace AdventOfCode.Tests.Year2024;

public class Tests_09_Disk_Fragmenter
{
	const int DAY = 09;

	private const string TEST_DATA = """
		2333133121414131402
		""";

	[Theory]
	[InlineData(TEST_DATA, 1928)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_DATA, 2858)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
