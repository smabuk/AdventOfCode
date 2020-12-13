using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2020 {
	public class Tests_13_Shuttle_Search {
		[Theory]
		[InlineData(new string[] {
			"939",
			"7,13,x,x,59,x,31,19"
		}, 295)]
		[InlineData(new string[] {
			"1004098",
			"23,x,x,x,x,x,x,x,x,x,x,x,x,41,x,x,x,x,x,x,x,x,x,509,x,x,x,x,x,x,x,x,x,x,x,x,13,17,x,x,x,x,x,x,x,x,x,x,x,x,x,x,29,x,401,x,x,x,x,x,37,x,x,x,x,x,x,x,x,x,x,x,x,19"
		}, 2406)]
		public void Part1(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2020, 13, 1, input), out int actual);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(new string[] {
			"17,x,13,19"
		}, 3417)]
		[InlineData(new string[] {
			"7,13,x,x,59,x,31,19"
		}, 1068781)]
		[InlineData(new string[] {
			"67,7,59,61"
		}, 754018)]
		[InlineData(new string[] {
			"67,7,x,59,61"
		}, 1261476)]
		[InlineData(new string[] {
			"1789,37,47,1889"
		}, 1202161486)]
		[InlineData(new string[] {
			"67,x,7,59,61"
		}, 779210)]
		[InlineData(new string[] {
			"23,x,x,x,x,x,x,x,x,x,x,x,x,41,x,x,x,x,x,x,x,x,x,509,x,x,x,x,x,x,x,x,x,x,x,x,13,17,x,x,x,x,x,x,x,x,x,x,x,x,x,x,29,x,401,x,x,x,x,x,37,x,x,x,x,x,x,x,x,x,x,x,x,19"
		}, 0)]
		public void Part2(string[] input, long expected) {
			_ = long.TryParse(SolutionRouter.SolveProblem(2020, 13, 2, input), out long actual);
			Assert.Equal(expected, actual);
		}

	}
}
