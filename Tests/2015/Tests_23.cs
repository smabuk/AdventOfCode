namespace AdventOfCode.Tests._2015;

public class Tests_23_Opening_the_Turing_Lock {
	[Theory]
	[InlineData("""
		inc a
		jio a, +2
		tpl a
		inc a
		"""
		,"a" , 2)]
	[InlineData("""
		jio a, +19
		inc a
		tpl a
		inc a
		tpl a
		inc a
		tpl a
		tpl a
		inc a
		inc a
		tpl a
		tpl a
		inc a
		inc a
		tpl a
		inc a
		inc a
		tpl a
		jmp +23
		tpl a
		tpl a
		inc a
		inc a
		tpl a
		inc a
		inc a
		tpl a
		inc a
		tpl a
		inc a
		tpl a
		inc a
		tpl a
		inc a
		inc a
		tpl a
		inc a
		inc a
		tpl a
		tpl a
		inc a
		jio a, +8
		inc b
		jie a, +4
		tpl a
		inc a
		jmp +2
		hlf a
		jmp -7
		"""
		, "b", 184)]
	public void Part1(string input, string register, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2015, 23, 1, input, register), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		inc a
		jio a, +2
		tpl a
		inc a
		"""
		, 2)]
	public void Part2(string input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2015, 23, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
