using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2015 {
	public class Tests_13_Knights_of_the_Dinner_Table {
		[Theory]
		[InlineData(new string[] {
			"Alice would gain 54 happiness units by sitting next to Bob.",
			"Alice would lose 79 happiness units by sitting next to Carol.",
			"Alice would lose 2 happiness units by sitting next to David.",
			"Bob would gain 83 happiness units by sitting next to Alice.",
			"Bob would lose 7 happiness units by sitting next to Carol.",
			"Bob would lose 63 happiness units by sitting next to David.",
			"Carol would lose 62 happiness units by sitting next to Alice.",
			"Carol would gain 60 happiness units by sitting next to Bob.",
			"Carol would gain 55 happiness units by sitting next to David.",
			"David would gain 46 happiness units by sitting next to Alice.",
			"David would lose 7 happiness units by sitting next to Bob.",
			"David would gain 41 happiness units by sitting next to Carol.",
		}, 330)]
		[InlineData(new string[] {
			"Bob would gain 83 happiness units by sitting next to Alice.",
			"Bob would lose 7 happiness units by sitting next to Carol.",
			"Bob would lose 63 happiness units by sitting next to David.",
			"Alice would gain 54 happiness units by sitting next to Bob.",
			"Alice would lose 79 happiness units by sitting next to Carol.",
			"Alice would lose 2 happiness units by sitting next to David.",
			"Carol would lose 62 happiness units by sitting next to Alice.",
			"Carol would gain 60 happiness units by sitting next to Bob.",
			"Carol would gain 55 happiness units by sitting next to David.",
			"David would gain 46 happiness units by sitting next to Alice.",
			"David would lose 7 happiness units by sitting next to Bob.",
			"David would gain 41 happiness units by sitting next to Carol.",
		}, 330)]
		public void Part1(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2015, 13, 1, input), out int actual);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(new string[] {
			"Alice would gain 54 happiness units by sitting next to Bob.",
			"Alice would lose 79 happiness units by sitting next to Carol.",
			"Alice would lose 2 happiness units by sitting next to David.",
			"Bob would gain 83 happiness units by sitting next to Alice.",
			"Bob would lose 7 happiness units by sitting next to Carol.",
			"Bob would lose 63 happiness units by sitting next to David.",
			"Carol would lose 62 happiness units by sitting next to Alice.",
			"Carol would gain 60 happiness units by sitting next to Bob.",
			"Carol would gain 55 happiness units by sitting next to David.",
			"David would gain 46 happiness units by sitting next to Alice.",
			"David would lose 7 happiness units by sitting next to Bob.",
			"David would gain 41 happiness units by sitting next to Carol.",
		}, 286)]
		public void Part2(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2015, 13, 2, input), out int actual);
			Assert.Equal(expected, actual);
		}


	}
}
