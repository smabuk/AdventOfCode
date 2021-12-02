using AdventOfCode.Solutions.Year2019;
namespace AdventOfCode.Tests.Year2019;

public class Tests_05_Sunny_with_a_Chance_of_Asteroids {
	[Theory]
	[InlineData(new string[] { "3,0,4,0,99" }, new int[] { 1 }, 1)]
	public void Part1(string[] program, int[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2019, 5, 1, program, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new int[] { 1002, 4, 3, 4, 33 }, new int[] { 1 }, new int[] { 1002, 4, 3, 4, 99 })]
	public void IntcodeComputerTests(int[] program, int[] input, int[] expected) {
		int[] result = IntcodeComputer.ExecuteIntcodeProgram(program, input, out int[] actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] { 
		"1,0,0,3,1,1,2,3,1,3,4,3,1,5,0,3,2,1,10,19,1,19,6,23,2,23,13,27,1,27,5,31,2,31,10,35,1,9,35,39,1,39,9,43,2,9,43,47,1,5,47,51,2,13,51,55,1,55,9,59,2,6,59,63,1,63,5,67,1,10,67,71,1,71,10,75,2,75,13,79,2,79,13,83,1,5,83,87,1,87,6,91,2,91,13,95,1,5,95,99,1,99,2,103,1,103,6,0,99,2,14,0,0" },
		6577)]
	public void Part2(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2019, 5, 2, input, true), out int actual);
		Assert.Equal(expected, actual);
	}

}
