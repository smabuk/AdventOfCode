namespace AdventOfCode.Tests.Year2021;

public class Tests_23_Amphipod {
	[Theory]
	[InlineData(new string[] {
		"#############",
		"#...........#",
		"###B#C#B#D###",
		"  #A#D#C#A#  ",
		"  #########  ",
	}, 12521)]
	public void Part1(string[] input, int expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2021, 23, 1, input, true), out long actual);
		Assert.Equal(expected, actual);
	}

	[Theory(Skip = "This problem needs more thought")]
	[InlineData(new string[] {
		"#############",
		"#...........#",
		"###B#C#B#D###",
		"  #A#D#C#A#  ",
		"  #########  ",
	}, 44169)]
	public void Part2(string[] input, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2021, 23, 2, input), out long actual);
		Assert.Equal(expected, actual);
	}
}