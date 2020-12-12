using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2015 {
	public class Tests_15_Science_for_Hungry_People { 
		[Theory]
		[InlineData(new string[] {
			"Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8",
			"Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3"
		}, 62842880)]
		public void Part1(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2015, 15, 1, input), out int actual);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(new string[] {
			"Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8",
			"Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3"
		}, 286)]
		public void Part2(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2015, 15, 2, input), out int actual);
			Assert.Equal(expected, actual);
		}


	}
}
