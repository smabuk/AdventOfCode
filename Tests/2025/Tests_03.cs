namespace AdventOfCode.Tests.Year2025;

public class Tests_03_Lobby
{
	const int DAY = 03;

	private const string TEST_DATA =
		"""
		987654321111111
		811111111111119
		234234234234278
		818181911112111
		""";


	[Theory]
	[InlineData("987654321111111", 98)]
	[InlineData("811111111111119", 89)]
	[InlineData("234234234234278", 78)]
	[InlineData("818181911112111", 92)]
	[InlineData(TEST_DATA, 357)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData("987654321111111", 987654321111)]
	[InlineData("811111111111119", 811111111119)]
	[InlineData("234234234234278", 434234234278)]
	[InlineData("818181911112111", 888911112111)]
	[InlineData(TEST_DATA, 3121910778619)]
	public void Part2(string input, long expected)
	{
		_ = long.TryParse(SolveProblem(YEAR, DAY, PART2, input), out long actual);
		actual.ShouldBe(expected);
	}
}
