using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2015 {
	public class Tests_21_RPG_Simulator_20XX {
		[Theory]
		[InlineData(new string[] {
			"Hit Points: 12",
			"Damage: 7",
			"Armor: 2"
		}, 8)]
		[InlineData(new string[] {
			"Hit Points: 104",
			"Damage: 8",
			"Armor: 1"
		}, 78)]
		public void Part1(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2015, 21, 1, input), out int actual);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(new string[] {
			"Hit Points: 12",
			"Damage: 7",
			"Armor: 2"
		}, 9999)]
		[InlineData(new string[] {
			"Hit Points: 104",
			"Damage: 8",
			"Armor: 1"
		}, 9999)]
		public void Part2(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2015, 21, 2, input), out int actual);
			Assert.Equal(expected, actual);
		}
	}
}
