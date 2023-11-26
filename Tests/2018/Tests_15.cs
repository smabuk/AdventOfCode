using Xunit.Abstractions;

namespace AdventOfCode.Tests.Year2018;

public class Tests_15_Beverage_Bandits(ITestOutputHelper testOutputHelper)
{
	const int DAY = 15;

	[Theory]
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
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, new Action<string[], bool>(Callback)), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		#######
		#.G...#
		#...EG#
		#.#.#G#
		#..G#E#
		#.....#
		#######
		""", 4988)]
	[InlineData("""
		#######
		#E..EG#
		#.#G.E#
		#E.##E#
		#G..#.#
		#..E#.#
		#######
		""", 31284)]
	[InlineData("""
		#######
		#E.G#.#
		#.#G..#
		#G.#.G#
		#G..#.#
		#...E.#
		#######
		""", 3478)]
	[InlineData("""
		#######
		#.E...#
		#.#..G#
		#.###.#
		#E#G#G#
		#...#G#
		#######
		""", 6474)]
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
		""", 1140)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input, new Action<string[], bool>(Callback)), out int actual);
		Assert.Equal(expected, actual);
	}


	private void Callback(string[] lines, bool _)
	{
		if (lines is null or []) {
			return;
		}

		testOutputHelper.WriteLine(string.Join(Environment.NewLine, lines));
	}

}
