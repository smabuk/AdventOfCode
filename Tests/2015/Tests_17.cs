using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2015 {
	public class Tests_17_No_Such_Thing_as_Too_Much {
		[Theory]
		[InlineData(new string[] {
			"20", "15", "10", "5", "5"
		}, 4)]
		public void Part1(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2015, 17, 1, input), out int actual);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(new string[] {
			"20", "15", "10", "5", "5"
		}, 999)]
		public void Part2(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2015, 17, 2, input), out int actual);
			Assert.Equal(expected, actual);
		}
	}
}
