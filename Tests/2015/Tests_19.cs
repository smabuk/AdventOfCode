using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2015 {
	public class Tests_19_Medicine_for_Rudolph {
		[Theory]
		[InlineData(new string[] {
			"H => HO",
			"H => OH",
			"O => HH",
			"",
			"HOH"       
		}, 4)]
		[InlineData(new string[] {
			"H => HO",
			"H => OH",
			"O => HH",
			"",
			"HOHOHO"       
		}, 7)]
		public void Part1(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2015, 19, 1, input), out int actual);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(new string[] {
			"H => HO",
			"H => OH",
			"O => HH",
			"",
			"HOH"
		}, 3)]
		[InlineData(new string[] {
			"H => HO",
			"H => OH",
			"O => HH",
			"",
			"HOHOHO"
		}, 6)]
		public void Part2(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2015, 19, 2, input), out int actual);
			Assert.Equal(expected, actual);
		}
	}
}
