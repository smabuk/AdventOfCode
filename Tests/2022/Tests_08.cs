namespace AdventOfCode.Tests._2022;

public class Tests_08_Treetop_Tree_House {
	[Theory]
	[InlineData("""
		30373
		25512
		65332
		33549
		35390
		"""
		, 21)]
	public void Part1(string input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 8, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		30373
		25512
		65332
		33549
		35390
		"""
		, 8)]
	public void Part2(string input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 8, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
