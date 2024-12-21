namespace AdventOfCode.Tests.Year2024;

public class Tests_21_Keypad_Conundrum(ITestOutputHelper testOutputHelper)
{
	const int DAY = 21;

	[Theory]
	[InlineData("""
		029A
		980A
		179A
		456A
		379A
		""", 3, 126384)]
	[InlineData("""029A""", 3, 1972)]
	public void Part1(string input, int noOfRobots, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, new Action<string[], bool>(Callback), noOfRobots), out int actual);
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
