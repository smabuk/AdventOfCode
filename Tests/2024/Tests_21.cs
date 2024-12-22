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
		""", 126384)]
	[InlineData("""029A""",  1_972)]
	[InlineData("""980A""", 58_800)]
	[InlineData("""179A""", 12_172)]
	[InlineData("""456A""", 29_184)]
	[InlineData("""379A""", 24_256)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, new Action<string[], bool>(Callback)), out int actual);
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
