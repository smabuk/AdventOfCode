namespace AdventOfCode.Tests.Year2024;

public class Tests_07_Bridge_Repair
{
	const int DAY = 07;

	private const string TEST_DATA = """
		190: 10 19
		3267: 81 40 27
		83: 17 5
		156: 15 6
		7290: 6 8 6 15
		161011: 16 10 13
		192: 17 8 14
		21037: 9 7 18 13
		292: 11 6 16 20
		""";

	[Theory]
	[InlineData(TEST_DATA, 3749)]
	[InlineData("190: 10 19", 190)]
	[InlineData("3267: 81 40 27", 3267)]
	[InlineData("292: 11 6 16 20", 292)]
	[InlineData("161011: 16 10 13", 0)]
	[InlineData("21037: 9 7 18 13", 0)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
