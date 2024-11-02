namespace AdventOfCode.Tests.Year2016;

public class Tests_09_Explosives_in_Cyberspace
{
	const int DAY = 09;

	[Theory]
	[InlineData("ADVENT", 6)]
	[InlineData("A(1x5)BC", 7)]
	[InlineData("(3x3)XYZ", 9)]
	[InlineData("A(2x2)BCD(2x2)EFG", 11)]
	[InlineData("X(8x2)(3x3)ABCY", 18)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData("(3x3)XYZ", 9)]
	[InlineData("X(8x2)(3x3)ABCY", 20)]
	[InlineData("(27x12)(20x12)(13x14)(7x10)(1x12)A", 241920)]
	[InlineData("(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN", 445)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
