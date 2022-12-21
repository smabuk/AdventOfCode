namespace AdventOfCode.Tests._2022;

public class Tests_21_Monkey_Math {
	[Theory]
	[InlineData("""
		root: pppw + sjmn
		dbpl: 5
		cczh: sllz + lgvd
		zczc: 2
		ptdq: humn - dvpt
		dvpt: 3
		lfqf: 4
		humn: 5
		ljgn: 2
		sjmn: drzm * dbpl
		sllz: 4
		pppw: cczh / lfqf
		lgvd: ljgn * ptdq
		drzm: hmdt - zczc
		hmdt: 32
		"""
		, 152)]
	public void Part1(string input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 21, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		root: pppw + sjmn
		dbpl: 5
		cczh: sllz + lgvd
		zczc: 2
		ptdq: humn - dvpt
		dvpt: 3
		lfqf: 4
		humn: 5
		ljgn: 2
		sjmn: drzm * dbpl
		sllz: 4
		pppw: cczh / lfqf
		lgvd: ljgn * ptdq
		drzm: hmdt - zczc
		hmdt: 32
		"""
		, 301)]
	public void Part2(string input, long expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 21, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
