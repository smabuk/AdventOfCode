namespace AdventOfCode.Tests.Year2025;

[SupportTestOutput]
public partial class Tests_10_Factory
{
	const int DAY = 10;

	private const string TEST_DATA =
		"""
		[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
		[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}
		[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}
		""";

	[Theory]
	[InlineData(TEST_DATA, 7)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_DATA, 33)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
