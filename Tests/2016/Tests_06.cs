namespace AdventOfCode.Tests.Year2016;

public class Tests_06_Signals_and_Noise
{
	const int DAY = 06;

	private const string TEST_INPUT = """
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
		""";

	[Theory]
	[InlineData(TEST_INPUT, "easter")]
	public void Part1(string input, string expected)
	{
		SolutionRouter.SolveProblem(YEAR, DAY, PART1, input)
			.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_INPUT, "advent")]
	public void Part2(string input, string expected)
	{
		SolutionRouter.SolveProblem(YEAR, DAY, PART2, input)
			.ShouldBe(expected);
	}
}
