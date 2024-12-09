namespace AdventOfCode.Tests.Year2024;

public class Tests_09_Disk_Fragmenter(ITestOutputHelper testOutputHelper)
{
	const int DAY = 09;

	private const string TEST_DATA = """
		2333133121414131402
		""";

	[Theory]
	[InlineData(TEST_DATA, 1928)]
	public async Task Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, new Action<string[], bool>(Callback)), out int actual);
		actual.ShouldBe(expected);
		await Task.Delay(500);
	}

	[Theory]
	[InlineData(TEST_DATA, 2858)]
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
