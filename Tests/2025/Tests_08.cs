namespace AdventOfCode.Tests.Year2025;

[SupportTestOutput]
public partial class Tests_08_Playground

{
	const int DAY = 08;

	private const string TEST_DATA =
		"""
		162,817,812
		57,618,57
		906,360,560
		592,479,940
		352,342,300
		466,668,158
		542,29,236
		431,825,988
		739,650,466
		52,470,668
		216,146,977
		819,987,18
		117,168,530
		805,96,715
		346,949,466
		970,615,88
		941,993,340
		862,61,35
		984,92,344
		425,690,689
		""";

	[Theory]
	[InlineData(TEST_DATA, 10, 40)]
	public void Part1(string input, int noOfPairs, int expected)
	{
		_ = int.TryParse(SolveProblem(YEAR, DAY, PART1, input, noOfPairs), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_DATA, 25272)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
