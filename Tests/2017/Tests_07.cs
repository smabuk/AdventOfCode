namespace AdventOfCode.Tests.Year2017;

public class Tests_07_Recursive_Circus
{
	const int DAY = 07;

	[Theory]
	[InlineData("""
		pbga (66)
		xhth (57)
		ebii (61)
		havc (66)
		ktlj (57)
		fwft (72) -> ktlj, cntj, xhth
		qoyq (66)
		padx (45) -> pbga, havc, qoyq
		tknk (41) -> ugml, padx, fwft
		jptl (61)
		ugml (68) -> gyxo, ebii, jptl
		gyxo (61)
		cntj (57)
		""", "tknk")]
	public void Part1(string input, string expected)
	{
		string actual = SolutionRouter.SolveProblem(YEAR, DAY, PART1, input);
		actual.ShouldBe(expected);
	}
}
