﻿using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2020 {
	public class Tests_11_Seating_System {
		[Theory]
		[InlineData(new string[] {
			"L.LL.LL.LL",
			"LLLLLLL.LL",
			"L.L.L..L..",
			"LLLL.LL.LL",
			"L.LL.LL.LL",
			"L.LLLLL.LL",
			"..L.L.....",
			"LLLLLLLLLL",
			"L.LLLLLL.L",
			"L.LLLLL.LL"
		}, 37)]
		public void Part1(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2020, 11, 1, input), out int actual);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(new string[] {
			"16",
			"10",
			"15",
			"5",
			"1",
			"11",
			"7",
			"19",
			"6",
			"12",
			"4"
		}, 8)]
		public void Part2(string[] input, long expected) {
			_ = long.TryParse(SolutionRouter.SolveProblem(2020, 11, 2, input), out long actual);
			Assert.Equal(expected, actual);
		}

	}
}
