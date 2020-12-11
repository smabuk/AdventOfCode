using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2020 {
	public class Tests_10_Adapter_Array {
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
		}, 127)]
		public void Part1(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2020, 10, 1, input), out int actual);
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
		[InlineData(new string[] {
			"28",
			"33",
			"18",
			"42",
			"31",
			"14",
			"46",
			"20",
			"48",
			"47",
			"24",
			"23",
			"49",
			"45",
			"19",
			"38",
			"39",
			"11",
			"1",
			"32",
			"25",
			"35",
			"8",
			"17",
			"7",
			"9",
			"4",
			"2",
			"34",
			"10",
			"3"
		}, 19208)]
		public void Part2(string[] input, long expected) {
			_ = long.TryParse(SolutionRouter.SolveProblem(2020, 10, 2, input), out long actual);
			Assert.Equal(expected, actual);
		}

	}
}
