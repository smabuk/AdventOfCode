namespace AdventOfCode.Tests.Year2025;

public class Tests_01_Secret_Entrance(ITestOutputHelper testOutputHelper)
{
	const int DAY = 01;

	private const string TEST_DATA = """
		L68
		L30
		R48
		L5
		R60
		L55
		L1
		L99
		R14
		L82
		""";

	[Theory]
	[InlineData(TEST_DATA, 3)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, new Action<string[], bool>(Callback)), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_DATA, 6)]
	[InlineData("R1000", 10)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input, new Action<string[], bool>(Callback)), out int actual);
		actual.ShouldBe(expected);
	}

	private void Callback(string[] lines, bool _)
	{
		if (lines is null or []) {
			return;
		}

		testOutputHelper.WriteLine(string.Join(Environment.NewLine, lines));
	}

}
