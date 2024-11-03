namespace AdventOfCode.Tests.Year2016;

public class Tests_12_Leonardos_Monorail
{
	const int DAY = 12;

	[Theory]
	[InlineData("""
		cpy 41 a
		inc a
		inc a
		dec a
		jnz a 2
		dec a
		""", 42)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
