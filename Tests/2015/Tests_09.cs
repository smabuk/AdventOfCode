namespace AdventOfCode.Tests._2015;

public class Tests_09_All_in_a_Single_Night {
	[Theory]
	[InlineData(new string[] {
			@"London to Dublin = 464",
			@"London to Belfast = 518",
			@"Dublin to Belfast = 141"
		}, 605)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2015, 9, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] {
			@"London to Dublin = 464",
			@"London to Belfast = 518",
			@"Dublin to Belfast = 141"
		}, 982)]
	public void Part2(string[] input, int expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2015, 9, 2, input), out long actual);
		Assert.Equal(expected, actual);
	}

}
