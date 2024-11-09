namespace AdventOfCode.Tests.Year2017;

public class Tests_08_I_Heard_You_Like_Registers
{
	const int DAY = 08;

	[Theory]
	[InlineData("""
		b inc 5 if a > 1
		a inc 1 if b < 5
		c dec -10 if a >= 1
		c inc -20 if c == 10
		""", 1)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
