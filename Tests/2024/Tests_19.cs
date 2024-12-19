namespace AdventOfCode.Tests.Year2024;

public class Tests_19_Linen_Layout
{
	const int DAY = 19;

	[Theory]
	[InlineData("""
		r, wr, b, g, bwu, rb, gb, br

		brwrr
		bggr
		gbbr
		rrbgbr
		ubwu
		bwurrg
		brgr
		bbrgwb
		""", 6)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData("""
		r, wr, b, g, bwu, rb, gb, br

		brwrr
		bggr
		gbbr
		rrbgbr
		ubwu
		bwurrg
		brgr
		bbrgwb
		""", 16)]
	[InlineData("""
		r, wr, b, g, bwu, rb, gb, br

		ubwu
		bbrgwb
		""", 0)]
	[InlineData("""
		r, wr, b, g, bwu, rb, gb, br

		brwrr
		""", 2)]
	[InlineData("""
		r, wr, b, g, bwu, rb, gb, br

		bggr
		""", 1)]
	[InlineData("""
		r, wr, b, g, bwu, rb, gb, br

		gbbr
		""", 4)]
	[InlineData("""
		r, wr, b, g, bwu, rb, gb, br

		rrbgbr
		""", 6)]
	[InlineData("""
		r, wr, b, g, bwu, rb, gb, br

		bwurrg
		""", 1)]
	[InlineData("""
		r, wr, b, g, bwu, rb, gb, br

		brgr
		""", 2)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
