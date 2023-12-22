namespace AdventOfCode.Tests.Year2023;

public class Tests_22_Sand_Slabs
{
	const int DAY = 22;

	private const string TEST_DATA = """
		1,0,1~1,2,1
		0,0,2~2,0,2
		0,2,3~2,2,3
		0,0,4~0,2,4
		2,0,5~2,2,5
		0,1,6~2,1,6
		1,1,8~1,1,9
		""";

	[Theory]
	[InlineData(TEST_DATA, 5)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_DATA, 7)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
