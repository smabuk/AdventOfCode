namespace AdventOfCode.Tests.Year2025;

[SupportTestOutput]
public partial class Tests_11_Reactor
{
	const int DAY = 11;

	private const string TEST_DATA =
		"""
		aaa: you hhh
		you: bbb ccc
		bbb: ddd eee
		ccc: ddd eee fff
		ddd: ggg
		eee: out
		fff: out
		ggg: out
		hhh: ccc fff iii
		iii: out
		""";

	[Theory]
	[InlineData(TEST_DATA, 5)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
