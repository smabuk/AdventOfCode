namespace AdventOfCode.Tests.Year2018;

public class Tests_02_Inventory_Management_System
{
	const int DAY = 2;

	[Theory]
	[InlineData((string[])([
		"abcdef",
		"bababc",
		"abbcde",
		"abcccd",
		"aabcdd",
		"abcdee",
		"ababab",
		]), 12)]
	public void Part1(string[] input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData((string[])([
		"abcde",
		"fghij",
		"klmno",
		"pqrst",
		"fguij",
		"axcye",
		"wvxyz",
	]), "fgij")]
	public void Part2(string[] input, string expected)
	{
		string actual = SolutionRouter.SolveProblem(YEAR, DAY, PART2, input);
		Assert.Equal(expected, actual);
	}
}
