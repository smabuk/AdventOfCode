namespace AdventOfCode.Tests.Year2017;

public class Tests_03_Spiral_Memory
{
	const int DAY = 03;

	[Theory]
	[InlineData(1, 0)]
	[InlineData(12, 3)]
	[InlineData(23, 2)]
	[InlineData(25, 4)]
	[InlineData(1024, 31)]
	public void Part1(int input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input.ToString()), out int actual);
		actual.ShouldBe(expected);
	}
}
