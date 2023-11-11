namespace AdventOfCode.Tests.Year2018;

public class Tests_07_The_Sum_of_Its_Parts
{
	const int DAY = 7;

	[Theory]
	[InlineData("""
		Step C must be finished before step A can begin.
		Step C must be finished before step F can begin.
		Step A must be finished before step B can begin.
		Step A must be finished before step D can begin.
		Step B must be finished before step E can begin.
		Step D must be finished before step E can begin.
		Step F must be finished before step E can begin.
		"""
		, "CABDFE")]
	public void Part1(string input, string expected)
	{
		string actual = SolutionRouter.SolveProblem(YEAR, DAY, PART1, input);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		Step C must be finished before step A can begin.
		Step C must be finished before step F can begin.
		Step A must be finished before step B can begin.
		Step A must be finished before step D can begin.
		Step B must be finished before step E can begin.
		Step D must be finished before step E can begin.
		Step F must be finished before step E can begin.
		"""
		, 9999)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input, 32), out int actual);
		Assert.Equal(expected, actual);
	}
}
