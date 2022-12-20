namespace AdventOfCode.Tests._2022;

public class Tests_20_Grove_Positioning_System {
	[Theory]
	[InlineData("""
		1
		2
		-3
		3
		-2
		0
		4
		"""
		, 3)]
	[InlineData("""
		-1
		7
		1
		23
		0
		-12
		-14
		"""
		, 31)]
	public void Part1(string input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 20, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		1
		2
		-3
		3
		-2
		0
		4
		"""
		, 9999)]
	public void Part2(string input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 20, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
