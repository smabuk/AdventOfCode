namespace AdventOfCode.Tests.Year2016;

public class Tests_23_Safe_Cracking
{
	const int DAY = 23;

	[Theory]
	[InlineData("""
		cpy 2 a
		tgl a
		tgl a
		tgl a
		cpy 1 a
		dec a
		dec a
		""", 3)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
