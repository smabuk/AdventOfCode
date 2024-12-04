namespace AdventOfCode.Tests.Year2024;

public class Tests_04_Ceres_Search
{
	const int DAY = 04;

	[Theory]
	[InlineData("""
		MMMSXXMASM
		MSAMXMSMSA
		AMXSXMAAMM
		MSAMASMSMX
		XMASAMXAMM
		XXAMMXXAMA
		SMSMSASXSS
		SAXAMASAAA
		MAMMMXMMMM
		MXMXAXMASX
		""", 18)]
	[InlineData("""
		XMASAMX.MM
		""", 2)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
