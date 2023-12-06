namespace AdventOfCode.Tests.Year2023;

public class Tests_06_Wait_For_It {
	const int DAY = 6;

	private const string TEST_DATA = """
		Time:      7  15   30
		Distance:  9  40  200
		""";

	[Theory]
	[InlineData(TEST_DATA, 288)]
	public void Part(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_DATA, 71503)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_DATA, 71503)]
	public void Part2_Using_BinaryChop(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input, "chop"), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_DATA, 71503)]
	public void Part2_Using_BruteForce(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input, "force"), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_DATA, 71503)]
	public void Part2_Using_Maths(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input, "maths"), out int actual);
		actual.ShouldBe(expected);
	}
}
