using static AdventOfCode.Solutions.Year2021.Day22;
using static AdventOfCode.Solutions.Year2021.Day24;

namespace AdventOfCode.Tests.Year2021;

public class Tests_24_Arithmetic_Logic_Unit {
	[Theory]
	[InlineData(new string[] {
		"inp x",
		"mul x -1",
	}, new int[] { 3 }, 9999)]
	public void Part1(string[] input, int[] startValue, int expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2021, 23, 1, input, startValue), out long actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] {
		"inp x",
		"mul x -1",
	}, 44169)]
	public void Part2(string[] input, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2021, 23, 2, input), out long actual);
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void RealData_Part1() {
		ALU alu = new();
		string[] inputs = new[] {
			"inb w",
			"mul x 0",
			"add x z",
			"mod x 26",
			"div z 1",
			"add x 12",
			"eql x w",
			"eql x 0",
			"mul y 0",
			"add y 25",
			"mul y x",
			"add y 1",
			"mul z y",
			"mul y 0",
			"add y w",
			"add y 4",
			"mul y x",
			"add z y",
		};
		List<Instruction> instructions = ALU.Parse(inputs).ToList();
		int[] digits = new[] { 1 };
		bool actual = alu.ExecuteInstructions(instructions, digits);
		Assert.Equal(true, actual);
	}
}