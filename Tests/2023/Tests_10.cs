namespace AdventOfCode.Tests.Year2023;

public class Tests_10_Pipe_Maze(ITestOutputHelper testOutputHelper)
{
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
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, new Action<string[], bool>(Callback)), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData("""
		...........
		.S-------7.
		.|F-----7|.
		.||.....||.
		.||.....||.
		.|L-7.F-J|.
		.|..|.|..|.
		.L--J.L--J.
		...........
		""", 4)]
	[InlineData("""
		..........
		.S------7.
		.|F----7|.
		.||....||.
		.||....||.
		.|L-7F-J|.
		.|..||..|.
		.L--JL--J.
		..........
		""", 4)]
	[InlineData("""
		.F----7F7F7F7F-7....
		.|F--7||||||||FJ....
		.||.FJ||||||||L7....
		FJL7L7LJLJ||LJ.L-7..
		L--J.L7...LJS7F-7L7.
		....F-J..F7FJ|L7L7L7
		....L7.F7||L7|.L7L7|
		.....|FJLJ|FJ|F7|.LJ
		....FJL-7.||.||||...
		....L---J.LJ.LJLJ...
		""", 8)]
	[InlineData("""
		FF7FSF7F7F7F7F7F---7
		L|LJ||||||||||||F--J
		FL-7LJLJ||||||LJL-77
		F--JF--7||LJLJ7F7FJ-
		L---JF-JLJ.||-FJLJJ7
		|F|F-JF---7F7-L7L|7|
		|FFJF7L7F-JF7|JL---7
		7-L-JL7||F7|L7F-7F7|
		L.L7LFJ|||||FJL7||LJ
		L7JLJL-JLJLJL--JLJ.L
		""", 10)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input, new Action<string[], bool>(Callback)), out int actual);
		actual.ShouldBe(expected);
	}

	private void Callback(string[] lines, bool _)
	{
		if (lines is null or []) {
			return;
		}

		testOutputHelper.WriteLine(string.Join(Environment.NewLine, lines));
	}

}
