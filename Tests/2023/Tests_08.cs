namespace AdventOfCode.Tests.Year2023;

public class Tests_08_Haunted_Wasteland {
	const int DAY = 8;

	private const string TEST_DATA = """
		LLR

		AAA = (BBB, BBB)
		BBB = (AAA, ZZZ)
		ZZZ = (ZZZ, ZZZ)
		""";

	[Theory]
	[InlineData("""
		RL

		AAA = (BBB, CCC)
		BBB = (DDD, EEE)
		CCC = (ZZZ, GGG)
		DDD = (DDD, DDD)
		EEE = (EEE, EEE)
		GGG = (GGG, GGG)
		ZZZ = (ZZZ, ZZZ)
		""", 2)]
	[InlineData("""
		LLR

		AAA = (BBB, BBB)
		BBB = (AAA, ZZZ)
		ZZZ = (ZZZ, ZZZ)
		""", 6)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}
}
