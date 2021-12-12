namespace AdventOfCode.Tests.Year2021;

public class Tests_13_Transparent_Origami {
	[Theory]
	[InlineData(new string[] {
		"6,10",
		"0,14",
		"9,10",
		"0,3",
		"10,4",
		"4,11",
		"6,0",
		"6,12",
		"4,1",
		"0,13",
		"10,12",
		"3,4",
		"3,0",
		"8,4",
		"1,10",
		"2,14",
		"8,10",
		"9,0",
		"",
		"fold along y=7",
		"fold along x=5",
	}, 1, 17)]
	[InlineData(new string[] {
		"6,10",
		"0,14",
		"9,10",
		"0,3",
		"10,4",
		"4,11",
		"6,0",
		"6,12",
		"4,1",
		"0,13",
		"10,12",
		"3,4",
		"3,0",
		"8,4",
		"1,10",
		"2,14",
		"8,10",
		"9,0",
		"",
		"fold along y=7",
		"fold along x=5",
	}, 2, 16)]
	public void Part1(string[] input,int folds, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2021, 13, 1, input, folds), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] {
		"6,10",
		"0,14",
		"9,10",
		"0,3",
		"10,4",
		"4,11",
		"6,0",
		"6,12",
		"4,1",
		"0,13",
		"10,12",
		"3,4",
		"3,0",
		"8,4",
		"1,10",
		"2,14",
		"8,10",
		"9,0",
		"",
		"fold along y=7",
		"fold along x=5",
	}, @"
#####
#   #
#   #
#   #
#####")]
	public void Part2(string[] input, string expected) {
		string actual = SolutionRouter.SolveProblem(2021, 13, 2, input);
		Assert.Equal(expected, actual);
	}
}
