using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2015 {
	public class Tests_07_Some_Assembly_Required {
		[Theory]
		[InlineData(new string[] {
			"123 -> x",
			"456 -> y",
			"x AND y -> d",
			"x OR y -> e",
			"x LSHIFT 2 -> f",
			"y RSHIFT 2 -> g",
			"NOT x -> h",
			"NOT y -> i",
		}, 5)]
		public void Part1(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2015, 7, 1, input), out int actual);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(new string[] {
		"123 -> x",
		"456 -> y",
		"x AND y -> d",
		"x OR y -> e",
		"x LSHIFT 2 -> f",
		"y RSHIFT 2 -> g",
		"NOT x -> h",
		"NOT y -> i",
		}, 8)]
		public void Part2(string[] input, long expected) {
			_ = long.TryParse(SolutionRouter.SolveProblem(2015, 7, 2, input), out long actual);
			Assert.Equal(expected, actual);
		}

	}
}
