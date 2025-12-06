namespace AdventOfCode.Tests.Year2025;

public class Tests_02_Gift_Shop
{
	const int DAY = 02;

	private const string TEST_DATA = "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124";

	[Theory]
	[InlineData(TEST_DATA, 1227775554)]
	public void Part1(string input, long expected)
	{
		_ = long.TryParse(SolveProblem(YEAR, DAY, PART1, input), out long actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_DATA, 4174379265)]
	public void Part2(string input, long expected)
	{
		_ = long.TryParse(SolveProblem(YEAR, DAY, PART2, input), out long actual);
		actual.ShouldBe(expected);
	}
}
