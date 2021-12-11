namespace AdventOfCode.Tests.Year2021;

public class Tests_11_Dumbo_Octopus {
	[Theory]
	[InlineData(new string[] {
		"11111",
		"19991",
		"19191",
		"19991",
		"11111"
	}, 2, 9)]
	[InlineData(new string[] {
		"5483143223",
		"2745854711",
		"5264556173",
		"6141336146",
		"6357385478",
		"4167524645",
		"2176841721",
		"6882881134",
		"4846848554",
		"5283751526"
	}, 10, 204)]
	[InlineData(new string[] {
		"5483143223",
		"2745854711",
		"5264556173",
		"6141336146",
		"6357385478",
		"4167524645",
		"2176841721",
		"6882881134",
		"4846848554",
		"5283751526"
	}, 100, 1656)]
	public void Part1(string[] input, int steps, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2021, 11, 1, input, steps), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] {
		"5483143223",
		"2745854711",
		"5264556173",
		"6141336146",
		"6357385478",
		"4167524645",
		"2176841721",
		"6882881134",
		"4846848554",
		"5283751526"
	}, 195)]
	public void Part2(string[] input, long expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2021, 11, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
