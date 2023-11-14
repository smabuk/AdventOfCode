namespace AdventOfCode.Tests.Year2018;

public class Tests_12_Subterranean_Sustainability
{
	const int DAY = 12;

	[Theory]
	[InlineData("""
		initial state: #..#.#..##......###...###

		...## => #
		..#.. => #
		.#... => #
		.#.#. => #
		.#.## => #
		.##.. => #
		.#### => #
		#.#.# => #
		#.### => #
		##.#. => #
		##.## => #
		###.. => #
		###.# => #
		####. => #
		"""
		, 325)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
