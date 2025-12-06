namespace AdventOfCode.Tests.Year2025;

public partial class Tests_06_Trash_Compactor
{
	const int DAY = 06;

	private const string TEST_DATA =
		"""
		123 328  51 64.
		 45 64  387 23.
		  6 98  215 314
		*   +   *   +
		""";

	[Theory]
	[InlineData(TEST_DATA, 4277556)]
	public void Part1(string input, int expected)
	{
		input = input.Replace('.', ' ');
		_ = int.TryParse(SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_DATA, 3263827)]
	public void Part2(string input, int expected)
	{
		input = input.Replace('.', ' ');
		_ = int.TryParse(SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
