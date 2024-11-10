namespace AdventOfCode.Tests.Year2017;

public class Tests_13_Packet_Scanners(ITestOutputHelper testOutputHelper)
{
	const int DAY = 13;

	[Theory]
	[InlineData("""
		0: 3
		1: 2
		4: 4
		6: 4
		""", 24)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, new Action<string[], bool>(Callback)), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData("""
		0: 3
		1: 2
		4: 4
		6: 4
		""", 10)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
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
