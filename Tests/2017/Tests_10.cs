namespace AdventOfCode.Tests.Year2017;

public class Tests_10_Knot_Hash
{
	const int DAY = 10;

	[Theory]
	[InlineData("""3, 4, 1, 5""", 5, 12)]
	public void Part1(string input, int listSize,  int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, listSize), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData("", "a2582a3a0e66e6e86e3812dcb672a272")]
	[InlineData("AoC 2017", "33efeb34ea91902bb2f59c9920caa6cd")]
	[InlineData("1,2,3", "3efbe78a8d82f29979031a4aa0b16a9d")]
	[InlineData("1,2,4", "63960835bcdc130f0b66d7ff4f6a5a8e")]
	public void Part2(string input, string expected)
	{
		string actual = SolutionRouter.SolveProblem(YEAR, DAY, PART2, input);
		actual.ShouldBe(expected);
	}
}
