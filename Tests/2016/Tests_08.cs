namespace AdventOfCode.Tests.Year2016;

public class Tests_08_Two_Factor_Authentication(ITestOutputHelper testOutputHelper)
{
	const int DAY = 08;

	[Theory]
	[InlineData("""
		rect 3x2
		rotate column x=1 by 1
		rotate row y=0 by 4
		rotate column x=1 by 1
		""", 6)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, new Action<string[], bool>(Callback), 7, 3), out int actual);
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
