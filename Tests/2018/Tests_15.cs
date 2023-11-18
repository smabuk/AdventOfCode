namespace AdventOfCode.Tests.Year2018;

public class Tests_15_Beverage_Bandits
{
	const int DAY = 15;

	[Theory(Skip="WIP: skipping to work on days I enjoy better.")]
	[InlineData("""
		#######
		#.G...#
		#...EG#
		#.#.#G#
		#..G#E#
		#.....#
		#######
		""", 27730)]
	[InlineData("""
		#######
		#G..#E#
		#E#E.E#
		#G.##.#
		#...#E#
		#...E.#
		#######
		""", 36334)]
	[InlineData("""
		#######
		#E..EG#
		#.#G.E#
		#E.##E#
		#G..#.#
		#..E#.#
		#######
		""", 39514)]
	[InlineData("""
		#######
		#E.G#.#
		#.#G..#
		#G.#.G#
		#G..#.#
		#...E.#
		#######
		""", 27755)]
	[InlineData("""
		#######
		#.E...#
		#.#..G#
		#.###.#
		#E#G#G#
		#...#G#
		#######
		""", 28944)]
	[InlineData("""
		#########
		#G......#
		#.E.#...#
		#..##..G#
		#...##..#
		#...#...#
		#.G...G.#
		#.....G.#
		#########
		""", 18740)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory(Skip = "WIP: skipping to work on days I enjoy better.")]
	[InlineData("""
		#######
		#.G...#
		#...EG#
		#.#.#G#
		#..G#E#
		#.....#
		#######
		""", 9999)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		Assert.Equal(expected, actual);
	}

}
