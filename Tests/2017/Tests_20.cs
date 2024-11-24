namespace AdventOfCode.Tests.Year2017;

public class Tests_20_Particle_Swarm
{
	const int DAY = 20;

	[Theory]
	[InlineData("""
		p=< 4,0,0>, v=< 0,0,0>, a=<-2,0,0>
		p=< 3,0,0>, v=< 2,0,0>, a=<-1,0,0>
		""", 1)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData("""
		p=<-6,0,0>, v=< 3,0,0>, a=< 0,0,0>
		p=<-4,0,0>, v=< 2,0,0>, a=< 0,0,0>
		p=<-2,0,0>, v=< 1,0,0>, a=< 0,0,0>
		p=< 3,0,0>, v=<-1,0,0>, a=< 0,0,0>
		""", 1)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
