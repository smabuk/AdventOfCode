namespace AdventOfCode.Tests._2015;

public class Tests_18_Like_a_GIF_For_Your_Yard {
	[Theory]
	[InlineData(new string[] {
			".#.#.#",
			"...##.",
			"#....#",
			"..#...",
			"#.#..#",
			"####.."
		}, 4, 4)]
	public void Part1(string[] input, int noOfIterations, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2015, 18, 1, input, noOfIterations), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] {
			".#.#.#",
			"...##.",
			"#....#",
			"..#...",
			"#.#..#",
			"####.."
		}, 5, 17)]
	public void Part2(string[] input, int noOfIterations, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2015, 18, 2, input, noOfIterations), out int actual);
		Assert.Equal(expected, actual);
	}
}
