namespace AdventOfCode.Tests._2022;

public class Tests_09_Rope_Bridge {
	[Theory]
	[InlineData("""
		R 4
		U 4
		L 3
		D 1
		R 4
		D 1
		L 5
		R 2
		"""
		, 13)]
	public void Part1(string input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 9, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		R 5
		U 8
		L 8
		D 3
		R 17
		D 10
		L 25
		U 20
		"""
		, 36)]
	public void Part2(string input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 9, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
