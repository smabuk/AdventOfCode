using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2020 {
	public class Tests_16 {
		[Theory]
		[InlineData(new string[] {
			"0,3,6"
		}, 436)]
		public void Part1(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2020, 16, 1, input), out int actual);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(new string[] {
			"9,12,1,4,17,0,18"
		}, 1407)]
		public void Part2(string[] input, long expected) {
			_ = long.TryParse(SolutionRouter.SolveProblem(2020, 16, 2, input), out long actual);
			Assert.Equal(expected, actual);
		}

	}
}
