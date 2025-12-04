namespace AdventOfCode.Tests.Year2025;

[SupportTestOutput]
public partial class Tests_04_Printing_Department
{
	const int DAY = 04;

	private const string TEST_DATA =
		"""
		..@@.@@@@.
		@@@.@.@.@@
		@@@@@.@.@@
		@.@@@@..@.
		@@.@@@@.@@
		.@@@@@@@.@
		.@.@.@.@@@
		@.@@@.@@@@
		.@@@@@@@@.
		@.@.@@@.@.
		""";

	[Theory]
	[InlineData(TEST_DATA, 13)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
