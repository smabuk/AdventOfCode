namespace AdventOfCode.Tests.Year2023;

public class Tests_16_The_Floor_Will_Be_Lava
{
	const int DAY = 16;

	private const string TEST_DATA = """
		.|...\....
		|.-.\.....
		.....|-...
		........|.
		..........
		.........\
		..../.\\..
		.-.-/..|..
		.|....-|.\
		..//.|....
		""";

	[Theory]
	[InlineData(TEST_DATA, 46)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_DATA, 51)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}

}
