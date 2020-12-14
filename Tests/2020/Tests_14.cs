using AdventOfCode.Solutions;

using Xunit;

namespace AdventOfCode.Tests.Year2020 {
	public class Tests_14_Docking_Data {
		[Theory]
		[InlineData(new string[] {
			"mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X",
			"mem[8] = 11",
			"mem[7] = 101",
			"mem[8] = 0"        
		}, 165)]
		public void Part1(string[] input, int expected) {
			_ = int.TryParse(SolutionRouter.SolveProblem(2020, 14, 1, input), out int actual);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(new string[] {
			"1004098",
			"13, 17"
		}, 169)]
		public void Part2(string[] input, long expected) {
			_ = long.TryParse(SolutionRouter.SolveProblem(2020, 14, 2, input), out long actual);
			Assert.Equal(expected, actual);
		}

	}
}
