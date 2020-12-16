using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2020 {
	public class Tests_16_Ticket_Translation {
		[Theory]
		[InlineData(new string[] {
			"class: 1-3 or 5-7",
			"row: 6-11 or 33-44",
			"seat: 13-40 or 45-50",
			"",
			"your ticket:",
			"7,1,14",
			"",
			"nearby tickets:",
			"7,3,47",
			"40,4,50",
			"55,2,20",
			"38,6,12",
		}, 71)]
		public void Part1(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2020, 16, 1, input), out int actual);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(new string[] {
			"class: 1-3 or 5-7",
			"row: 6-11 or 33-44",
			"seat: 13-40 or 45-50",
			"",
			"your ticket:",
			"7,1,14",
			"",
			"nearby tickets:",
			"7,3,47",
			"40,4,50",
			"55,2,20",
			"38,6,12",
		}, 9999)]
		public void Part2(string[] input, long expected) {
			_ = long.TryParse(SolutionRouter.SolveProblem(2020, 16, 2, input), out long actual);
			Assert.Equal(expected, actual);
		}

	}
}
