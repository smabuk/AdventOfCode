namespace AdventOfCode.Tests.Year2017;

public class Tests_14_Disk_Defragmentation(ITestOutputHelper testOutputHelper)
{
	const int DAY = 14;

	[Theory]
	[InlineData("flqrgnkx", 8108)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData("flqrgnkx", 1242)]
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
