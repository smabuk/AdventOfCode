namespace AdventOfCode.Tests.Year2023;

public class Tests_10_Pipe_Maze {
	const int DAY = 10;

	[Theory]
	[InlineData("""
		.....
		.S-7.
		.|.|.
		.L-J.
		.....
		""", 4)]
	[InlineData("""
		..F7.
		.FJ|.
		SJ.L7
		|F--J
		LJ...
		""", 8)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	//[Theory]
	//[InlineData(TEST_DATA, 9999)]
	//public void Part2(string input, int expected)
	//{
	//	_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
	//	actual.ShouldBe(expected);
	//}
}
