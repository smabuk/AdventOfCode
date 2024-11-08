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

	[Theory]
	[InlineData(4, 5)]
	[InlineData(11, 23)]
	[InlineData(59, 122)]
	[InlineData(330, 351)]
	[InlineData(747, 806)]
	public void Part2(int input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input.ToString()), out int actual);
		actual.ShouldBe(expected);
	}
}
