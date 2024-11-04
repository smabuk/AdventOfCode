namespace AdventOfCode.Tests.Year2016;

public class Tests_18_Like_a_Rogue(ITestOutputHelper testOutputHelper)
{
	const int DAY = 18;

	[Theory]
	[InlineData("..^^.", 3, 6)]
	[InlineData(".^^.^.^^^^", 10, 38)]
	public void Part1(string input, int noOfRows, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, new Action<string[], bool>(Callback), noOfRows), out int actual);
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
