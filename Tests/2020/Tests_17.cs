using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2020 {
	public class Tests_17_Conway_Cubes {
		[Theory]
		[InlineData(new string[] {
			".#.",
			"..#",
			"###"       
		}, 112)]
		[InlineData(new string[] {
			"######.#",
			"##.###.#",
			"#.###.##",
			"..#..###",
			"##.#.#.#",
			"##...##.",
			"#.#.##.#",
			".###.###"
		}, 348)]
		public void Part1(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2020, 17, 1, input), out int actual);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(new string[] {
			".#.",
			"..#",
			"###"
		}, 848)]
		[InlineData(new string[] {
			"######.#",
			"##.###.#",
			"#.###.##",
			"..#..###",
			"##.#.#.#",
			"##...##.",
			"#.#.##.#",
			".###.###"
		}, 2236)]
		public void Part2(string[] input, long expected) {
			_ = long.TryParse(SolutionRouter.SolveProblem(2020, 17, 2, input), out long actual);
			Assert.Equal(expected, actual);
		}

	}
}
