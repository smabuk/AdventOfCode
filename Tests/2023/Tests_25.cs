namespace AdventOfCode.Tests.Year2023;

public class Tests_25_Snowverload
{
	const int DAY = 25;

	private const string TEST_DATA = """
		jqt: rhn xhk nvd
		rsh: frs pzl lsr
		xhk: hfx
		cmg: qnr nvd lhk bvb
		rhn: xhk bvb hfx
		bvb: xhk hfx
		pzl: lsr hfx nvd
		qnr: nvd
		ntq: jqt hfx bvb xhk
		nvd: lhk
		lsr: lhk
		rzs: qnr cmg lsr rsh
		frs: qnr lhk lsr
		""";

	[Theory]
	[InlineData(TEST_DATA, 54)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
