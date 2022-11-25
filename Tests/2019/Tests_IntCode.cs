using AdventOfCode.Solutions._2019;
namespace AdventOfCode.Tests._2019;

public class Tests_IntCode {
	[Theory]
	[InlineData(new int[] { 1002, 4, 3, 4, 33 }, new int[] { 1 }, new int[] { 1002, 4, 3, 4, 99 })]
	public void IntcodeComputerTests(int[] program, int[] input, int[] expected) {
		int[] actual = IntcodeComputer.ExecuteIntcodeProgram(program, input, out int[] _);
		Assert.Equal(expected, actual);
	}
}
