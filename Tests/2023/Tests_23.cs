namespace AdventOfCode.Tests.Year2023;

public class Tests_23_A_Long_Indexes
{
	const int DAY = 23;

	private const string TEST_DATA = """
		#.#####################
		#.......#########...###
		#######.#########.#.###
		###.....#.>.>.###.#.###
		###v#####.#v#.###.#.###
		###.>...#.#.#.....#...#
		###v###.#.#.#########.#
		###...#.#.#.......#...#
		#####.#.#.#######.#.###
		#.....#.#.#.......#...#
		#.#####.#.#.#########v#
		#.#...#...#...###...>.#
		#.#.#v#######v###.###v#
		#...#.>.#...>.>.#.###.#
		#####v#.#.###v#.#.###.#
		#.....#...#...#.#.#...#
		#.#########.###.#.#.###
		#...###...#...#...#.###
		###.###.#.###v#####v###
		#...#...#.#.>.>.#.>.###
		#.###.###.#.###.#.#v###
		#.....###...###...#...#
		#####################.#
		""";

	[Theory]
	[InlineData(TEST_DATA, 94)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_DATA, 154)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
