namespace AdventOfCode.Tests.Year2024;

public class Tests_05_Print_Queue
{
	const int DAY = 05;

	private const string TEST_INPUT = """
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

		75,47,61,53,29
		97,61,53,29,13
		75,29,13
		75,97,47,61,53
		61,13,29
		97,13,75,29,47
		""";

	[Theory]
	[InlineData(TEST_INPUT, 143)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_INPUT, 123)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
