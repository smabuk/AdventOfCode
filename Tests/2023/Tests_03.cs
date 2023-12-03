namespace AdventOfCode.Tests.Year2023;

public class Tests_03_Gear_Ratios
{
	const int DAY = 3;

	[Theory]
	[InlineData("""
		467..114..
		...*......
		..35..633.
		......#...
		617*......
		.....+.58.
		..592.....
		......755.
		...$.*....
		.664.598..
		""", 4361)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
