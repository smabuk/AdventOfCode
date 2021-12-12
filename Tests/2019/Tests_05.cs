namespace AdventOfCode.Tests.Year2019;

public class Tests_05_Sunny_with_a_Chance_of_Asteroids {
	[Theory]
	[InlineData(new string[] { "3,0,4,0,99" }, new int[] { 1 }, 1)]
	public void Part1(string[] program, int[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2019, 5, 1, program, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] { "3,9,8,9,10,9,4,9,99,-1,8" }, new int[] { 8 }, 1)]
	[InlineData(new string[] { "3,9,8,9,10,9,4,9,99,-1,8" }, new int[] { 4 }, 0)]
	[InlineData(new string[] { "3,3,1108,-1,8,3,4,3,99" }, new int[] { 8 }, 1)]
	[InlineData(new string[] { "3,3,1108,-1,8,3,4,3,99" }, new int[] { 4 }, 0)]
	[InlineData(new string[] { "3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9" }, new int[] { 0 }, 0)]
	[InlineData(new string[] { "3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9" }, new int[] { 9 }, 1)]
	[InlineData(new string[] { "3,3,1105,-1,9,1101,0,0,12,4,12,99,1" }, new int[] { 0 }, 0)]
	[InlineData(new string[] { "3,3,1105,-1,9,1101,0,0,12,4,12,99,1" }, new int[] { 9 }, 1)]
	[InlineData(new string[] { "3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99" },
		new int[] { 4 }, 999)]
	[InlineData(new string[] { "3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99" },
		new int[] { 8 }, 1000)]
	[InlineData(new string[] { "3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99" },
		new int[] { 9 }, 1001)]
	public void Part2(string[] program, int[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2019, 5, 2, program, input), out int actual);
		Assert.Equal(expected, actual);
	}

}
