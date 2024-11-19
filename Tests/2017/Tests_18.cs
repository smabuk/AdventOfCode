namespace AdventOfCode.Tests.Year2017;

public class Tests_18_Duet
{
	const int DAY = 18;

	[Theory]
	[InlineData("""
		set a 1
		add a 2
		mul a a
		mod a 5
		snd a
		set a 0
		rcv a
		jgz a -1
		set a 1
		jgz a -2
		""", 4)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData("""
		snd 1
		snd 2
		snd p
		rcv a
		rcv b
		rcv c
		rcv d
		""", 3)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
