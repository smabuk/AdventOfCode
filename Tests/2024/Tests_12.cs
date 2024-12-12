namespace AdventOfCode.Tests.Year2024;

public class Tests_12_Garden_Groups
{
	const int DAY = 12;

	[Theory]
	[InlineData("""
		AAAA
		BBCD
		BBCC
		EEEC
		""", 140)]
	[InlineData("""
		OOOOO
		OXOXO
		OOOOO
		OXOXO
		OOOOO
		""", 772)]
	[InlineData("""
		RRRRIICCFF
		RRRRIICCCF
		VVRRRCCFFF
		VVRCCCJFFF
		VVVVCJJCFE
		VVIVCCJJEE
		VVIIICJJEE
		MIIIIIJJEE
		MIIISIJEEE
		MMMISSJEEE
		""", 1930)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
