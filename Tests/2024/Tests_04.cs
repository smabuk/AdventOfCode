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
	[InlineData("""XMASAMX.MM""", 2)] // overlapping result
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData("""
		.M.S......
		..A..MSMS.
		.M.S.MAA..
		..A.ASMSM.
		.M.S.M....
		..........
		S.S.S.S.S.
		.A.A.A.A..
		M.M.M.M.M.
		..........
		""", 9)]
	[InlineData("""
		M.S
		.A.
		M.S
		""", 1)]
	[InlineData("""
		.M.
		MAS
		.S.
		""", 0)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
