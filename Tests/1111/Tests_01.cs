namespace AdventOfCode.Tests.Year1111;

//[SupportTestOutput]
public partial class Tests_GenerateIParsable
{
	const int DAY = 01;

	private const string TEST_DATA = """
		68
		30
		48
		5
		60
		55
		1
		99
		14
		82
		""";

	[Theory]
	[InlineData(TEST_DATA, 50)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
