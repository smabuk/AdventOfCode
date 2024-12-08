namespace AdventOfCode.Tests.Year2024;

public class Tests_08_Resonant_Collinearity(ITestOutputHelper testOutputHelper)
{
	const int DAY = 08;

	private const string TEST_DATA = """
		............
		........0...
		.....0......
		.......0....
		....0.......
		......A.....
		............
		............
		........A...
		.........A..
		............
		............
		""";

	[Theory]
	[InlineData(TEST_DATA, 14)]
	public async Task Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, new Action<string[], bool>(Callback)), out int actual);
		actual.ShouldBe(expected);
		await Task.Delay(500);
	}

	[Theory]
	[InlineData(TEST_DATA, 34)]
	public async Task Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input, new Action<string[], bool>(Callback)), out int actual);
		actual.ShouldBe(expected);
		await Task.Delay(500);
	}


	private void Callback(string[] lines, bool _)
	{
		if (lines is null or []) {
			return;
		}

		testOutputHelper.WriteLine(string.Join(Environment.NewLine, lines));
	}
}
