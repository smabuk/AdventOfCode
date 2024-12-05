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
	[InlineData("75,47,61,53,29", "lookup", 61)]
	[InlineData("97,61,53,29,13", "lookup", 53)]
	[InlineData("75,29,13"      , "lookup", 29)]
	[InlineData("75,97,47,61,53", "lookup",  0)]
	[InlineData("61,13,29"      , "lookup",  0)]
	[InlineData("97,13,75,29,47", "lookup",  0)]
	[InlineData("""
		75,47,61,53,29
		97,61,53,29,13
		75,29,13
		75,97,47,61,53
		61,13,29
		97,13,75,29,47
		""", "lookup", 143)]
	[InlineData("75,47,61,53,29", "sort", 61)]
	[InlineData("97,61,53,29,13", "sort", 53)]
	[InlineData("75,29,13"      , "sort", 29)]
	[InlineData("75,97,47,61,53", "sort",  0)]
	[InlineData("61,13,29"      , "sort",  0)]
	[InlineData("97,13,75,29,47", "sort",  0)]
	[InlineData("""
		75,47,61,53,29
		97,61,53,29,13
		75,29,13
		75,97,47,61,53
		61,13,29
		97,13,75,29,47
		""", "sort", 143)]
	public void Part1(string input, string method, int expected)
	{
		string fullInput = TEST_INPUT_BASE + input;
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, fullInput, method), out int actual);
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
	public void Part2_Using_Lookup(string input, int expected)
	{
		string fullInput = TEST_INPUT_BASE + input;
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, fullInput, "lookup"), out int actual);
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
	public void Part2_Using_Sort(string input, int expected)
	{
		string fullInput = TEST_INPUT_BASE + input;
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, fullInput, "sort"), out int actual);
		actual.ShouldBe(expected);
	}
}
