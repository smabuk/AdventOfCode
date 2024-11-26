namespace AdventOfCode.Tests.Year2017;

public class Tests_22_Sporifica_Virus
{
	const int DAY = 22;

	[Theory]
	[InlineData("""
		..#
		#..
		...
		""", 70, 41)]
	[InlineData("""
		..#
		#..
		...
		""", 10_000, 5587)]
	public void Part1(string input, int noOfBursts, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, noOfBursts), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData("""
		..#
		#..
		...
		""", 100, 26)]
	[InlineData("""
		..#
		#..
		...
		""", 10_000_000, 2511944)]
	public void Part2(string input, int noOfBursts, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input, noOfBursts), out int actual);
		actual.ShouldBe(expected);
	}

}
