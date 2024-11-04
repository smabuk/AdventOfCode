namespace AdventOfCode.Tests.Year2016;

public class Tests_20_Firewall_Rules
{
	const int DAY = 20;
	private const string TEST_INPUT = """
		5-8
		0-2
		4-7
		""";

	[Theory]
	[InlineData(TEST_INPUT, 3)]
	public void Part1(string input, uint expected)
	{
		_ = uint.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out uint actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_INPUT, 4294967296 - 8)]
	public void Part2(string input, uint expected)
	{
		_ = uint.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out uint actual);
		actual.ShouldBe(expected);
	}
}
