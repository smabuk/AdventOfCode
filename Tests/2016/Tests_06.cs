namespace AdventOfCode.Tests.Year2016;

public class Tests_06_Signals_and_Noise
{
	const int DAY = 06;

	[Theory]
	[InlineData("""
		eedadn
		drvtee
		eandsr
		raavrd
		atevrs
		tsrnev
		sdttsa
		rasrtv
		nssdts
		ntnada
		svetve
		tesnvt
		vntsnd
		vrdear
		dvrsen
		enarar
		""", "easter")]
	public void Part1(string input, string expected)
	{
		SolutionRouter.SolveProblem(YEAR, DAY, PART1, input)
			.ShouldBe(expected);
	}
}
