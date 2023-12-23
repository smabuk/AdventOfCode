namespace AdventOfCode.Tests.Year2023;

public class Tests_20_Pulse_Propagation
{
	const int DAY = 20;

	[Theory]
	[InlineData("""
		broadcaster -> a, b, c
		%a -> b
		%b -> c
		%c -> inv
		&inv -> a
		""", 1, 32)]
	[InlineData("""
		broadcaster -> a, b, c
		%a -> b
		%b -> c
		%c -> inv
		&inv -> a
		""", 1000, 32_000_000)]
	[InlineData("""
		broadcaster -> a
		%a -> inv, con
		&inv -> b
		%b -> con
		&con -> output
		""", 1000, 11_687_500)]
	public void Part1(string input, int noOfButtonPushes, long expected)
	{
		_ = long.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, noOfButtonPushes), out long actual);
		actual.ShouldBe(expected);
	}
}
