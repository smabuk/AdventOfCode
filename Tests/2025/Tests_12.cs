namespace AdventOfCode.Tests.Year2025;

[SupportTestOutput]
public partial class Tests_12_Christmas_Tree_Farm
{
	const int DAY = 12;

	private const string TEST_DATA =
		"""
		0:
		###
		##.
		##.

		1:
		###
		##.
		.##

		2:
		.##
		###
		##.

		3:
		##.
		###
		##.

		4:
		###
		#..
		###

		5:
		###
		.#.
		###

		4x4: 0 0 0 0 2 0
		12x5: 1 0 1 0 2 2
		12x5: 1 0 1 0 3 2
		""";

	[Theory]
	[InlineData(TEST_DATA, 2)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
