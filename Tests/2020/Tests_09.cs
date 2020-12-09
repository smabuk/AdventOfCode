using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2020 {
	public class Tests_09_Encoding_Error {
		[Theory]
		[InlineData(new string[] {
			"35",
			"20",
			"15",
			"25",
			"47",
			"40",
			"62",
			"55",
			"65",
			"95",
			"102",
			"117",
			"150",
			"182",
			"127",
			"219",
			"299",
			"277",
			"309",
			"576"
		}, 127)]
		public void Part1(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2020, 9, 1, input), out int actual);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(new string[] {
"35",
"20",
"15",
"25",
"47",
"40",
"62",
"55",
"65",
"95",
"102",
"117",
"150",
"182",
"127",
"219",
"299",
"277",
"309",
"576"
	  }, 8)]
		public void Part2(string[] input, long expected) {
			_ = long.TryParse(SolutionRouter.SolveProblem(2020, 9, 2, input), out long actual);
			Assert.Equal(expected, actual);
		}

	}
}
