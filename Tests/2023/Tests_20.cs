namespace AdventOfCode.Tests.Year2023;

public class Tests_20_Pulse_Propagation
{
	const int DAY = 20;

	private const string TEST_DATA = """
		broadcaster -> a, b, c
		%a -> b
		%b -> c
		%c -> inv
		&inv -> a
		""";

	[Theory]
	[InlineData(TEST_DATA, 32_000_000)]
	[InlineData("""
		broadcaster -> a
		%a -> inv, con
		&inv -> b
		%b -> con
		&con -> output
		""", 11_687_500)]
	public void Part1(string input, long expected)
	{
		_ = long.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out long actual);
		actual.ShouldBe(expected);
	}
}
