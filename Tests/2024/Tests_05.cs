namespace AdventOfCode.Tests.Year2024;

public class Tests_05_Print_Queue
{
	const int DAY = 05;

	private const string TEST_INPUT_BASE = """
		47|53
		97|13
		97|61
		97|47
		75|29
		61|13
		75|53
		29|13
		97|29
		53|29
		61|53
		97|53
		61|29
		47|13
		75|47
		97|75
		47|61
		75|61
		47|29
		75|13
		53|13


		""";

	[Theory]
	[InlineData("75,47,61,53,29", 61)]
	[InlineData("97,61,53,29,13", 53)]
	[InlineData("75,29,13",       29)]
	[InlineData("75,97,47,61,53",  0)]
	[InlineData("61,13,29",        0)]
	[InlineData("97,13,75,29,47",  0)]
	[InlineData("""
		75,47,61,53,29
		97,61,53,29,13
		75,29,13
		75,97,47,61,53
		61,13,29
		97,13,75,29,47
		""", 143)]
	public void Part1(string input, int expected)
	{
		string fullInput = TEST_INPUT_BASE + input;
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, fullInput), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData("""
		75,47,61,53,29
		97,61,53,29,13
		75,29,13
		75,97,47,61,53
		61,13,29
		97,13,75,29,47
		""", 123)]
	public void Part2(string input, int expected)
	{
		string fullInput = TEST_INPUT_BASE + input;
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, fullInput), out int actual);
		actual.ShouldBe(expected);
	}
}
