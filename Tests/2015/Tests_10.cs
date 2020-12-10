using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2015 {
	public class Tests_10_Elves_Look_Elves_Say {
		[Theory]
		[InlineData(new string[] { "1" }, 2)]
		[InlineData(new string[] { "11" }, 2)]
		[InlineData(new string[] { "21" }, 2)]
		[InlineData(new string[] { "1211" }, 4)]
		[InlineData(new string[] { "11121" }, 5)]
		public void Part1(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2015, 10, 1, input), out int actual);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(new string[] { "1" }, 2)]
		[InlineData(new string[] { "11" }, 2)]
		[InlineData(new string[] { "21" }, 2)]
		[InlineData(new string[] { "1211" }, 4)]
		[InlineData(new string[] { "11121" }, 5)]
		public void Part2(string[] input, int expected) {
			_ = long.TryParse(SolutionRouter.SolveProblem(2015, 10, 2, input), out long actual);
			Assert.Equal(expected, actual);
		}

	}
}
