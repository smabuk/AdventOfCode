namespace AdventOfCode.Tests.Year2016;

public class Tests_01_Bathroom_Security
{
	const int DAY = 02;

	private const string TEST_INPUT = """
		ULL
		RRDDD
		LURDL
		UUUUD
		""";

	[Theory]
	[InlineData(TEST_INPUT, "1985")]
	public void Part1(string input, string expected)
	{
		string actual  = SolutionRouter.SolveProblem(YEAR, DAY, PART1, input);
		actual.ShouldBe(expected);
	}

	//[Theory]
	//[InlineData(TEST_INPUT, "9999")]
	//public void Part2(string input, string expected)
	//{
	//	string actual = SolutionRouter.SolveProblem(YEAR, DAY, PART2, input);
	//	actual.ShouldBe(expected);
	//}
}
