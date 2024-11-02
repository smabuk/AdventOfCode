namespace AdventOfCode.Tests.Year2016;

public class Tests_04_Security_Through_Obscurity
{
	const int DAY = 04;

	[Theory]
	[InlineData("aaaaa-bbb-z-y-x-123[abxyz]", 123)]
	[InlineData("a-b-c-d-e-f-g-h-987[abcde]", 987)]
	[InlineData("not-a-real-room-404[oarel]", 404)]
	[InlineData("totally-real-room-200[decoy]", 0)]
	[InlineData("""
		aaaaa-bbb-z-y-x-123[abxyz]
		a-b-c-d-e-f-g-h-987[abcde]
		not-a-real-room-404[oarel]
		totally-real-room-200[decoy]
		""", 1514)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData("qzmt-zixmtkozy-ivhz", 343, "very encrypted name")]
	public void Part2(string encrypted, int sectorId, string expected)
	{
		Solutions._2016.Day04.Room
			.Decrypt(encrypted, sectorId)
			.ShouldBe(expected);
	}
}
