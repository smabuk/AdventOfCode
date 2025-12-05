using static AdventOfCode.Solutions.SolutionRouter;

namespace AdventOfCode.Tests.Year2025;

public partial class Tests_05_Cafeteria
{
	const int DAY = 05;

	private const string TEST_DATA =
		"""
		3-5
		10-14
		16-20
		12-18

		1
		5
		8
		11
		17
		32
		""";

	[Theory]
	[InlineData(TEST_DATA, 3)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_DATA, 14)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
