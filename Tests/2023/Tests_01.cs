namespace AdventOfCode.Tests.Year20123;

public class Tests_01_Trebuchet
{

	const int DAY = 1;

	[Theory]
	[InlineData("""
		1abc2
		pqr3stu8vwx
		a1b2c3d4e5f
		treb7uchet
		""", 142)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		two1nine
		eightwothree
		abcone2threexyz
		xtwone3four
		4nineeightseven2
		zoneight234
		7pqrstsixteen
		""", 281)]
	[InlineData("""
		zoneight
		""", 18)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
