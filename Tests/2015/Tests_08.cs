using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2015 {
	public class Tests_08_Matchsticks {
		[Theory]
		[InlineData(new string[] { @"""""" }, 2)]
		[InlineData(new string[] { @"""abc""" }, 2)]
		[InlineData(new string[] { @"""aaa\""aaa""" }, 3)]
		[InlineData(new string[] { @"""\x27""" }, 5)]
		[InlineData(new string[] {
			@"""""",
			@"""abc""",
			@"""aaa\""aaa""",
			@"""\x27"""
		}, 12)]
		public void Part1(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2015, 8, 1, input), out int actual);
			Assert.Equal(expected, actual);
		}

		[Theory (Skip ="This problem needs more thought")]
		[InlineData(new string[] {
		"123 -> x",
		"456 -> y",
		"x AND y -> d",
		"x OR y -> e",
		"x LSHIFT 2 -> f",
		"y RSHIFT 2 -> g",
		"NOT x -> h",
		"NOT y -> i",
		},  8)]
		public void Part2(string[] input, int expected) {
			_ = long.TryParse(SolutionRouter.SolveProblem(2015, 8, 2, input), out long actual);
			Assert.Equal(expected, actual);
		}

	}
}
