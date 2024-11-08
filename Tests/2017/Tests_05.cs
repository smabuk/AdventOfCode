namespace AdventOfCode.Tests.Year2017;

public class Tests_05_A_Maze_of_Twisty_Trampolines_All_Alike
{
	const int DAY = 05;

	[Theory]
	[InlineData("""
		0
		3
		0
		1
		-3
		""", 5)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
