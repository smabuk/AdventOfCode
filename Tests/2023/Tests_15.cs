namespace AdventOfCode.Tests.Year2023;

public class Tests_15_Lens_Library
{
	const int DAY = 15;

	private const string TEST_DATA = """
		rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7
		""";

	[Theory]
	[InlineData(TEST_DATA, 1320)]
	[InlineData("rn=1",      30)]
	[InlineData("cm-",      253)]
	[InlineData("qp=3",      97)]
	[InlineData("cm=2",      47)]
	[InlineData("qp-",       14)]
	[InlineData("pc=4",     180)]
	[InlineData("ot=9",       9)]
	[InlineData("ab=5",     197)]
	[InlineData("pc-",       48)]
	[InlineData("pc=6",     214)]
	[InlineData("ot=7",     231)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	//[Theory]
	//[InlineData(TEST_DATA, 9999)]
	//public void Part2(string input, int expected)
	//{
	//	_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
	//	actual.ShouldBe(expected);
	//}
}
