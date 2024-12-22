namespace AdventOfCode.Tests.Year2024;

public class Tests_22_Monkey_Market(ITestOutputHelper testOutputHelper)
{
	const int DAY = 22;

	[Theory]
	[InlineData("""
		1
		10
		100
		2024
		""", 2000, 37327623)]
	[InlineData(   "1", 2000, 8685429)]
	[InlineData(  "10", 2000, 4700978)]
	[InlineData( "100", 2000, 15273692)]
	[InlineData("2024", 2000, 8667524)]
	[InlineData("123", 1, 15887950)]
	[InlineData("123", 2, 16495136)]
	[InlineData("123", 10, 5908254)]
	public void Part1(string input, int iterations, long expected)
	{
		_ = long.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, new Action<string[], bool>(Callback), iterations), out long actual);
		actual.ShouldBe(expected);
	}


	[Theory]
	[InlineData("""
		1
		2
		3
		2024
		""", 2000, 23)]
	//[InlineData(   "1", 2000, 8685429)]
	//[InlineData(  "10", 2000, 4700978)]
	//[InlineData( "100", 2000, 15273692)]
	//[InlineData("2024", 2000, 8667524)]
	[InlineData("123", 10, 6)]
	public void Part2(string input, int iterations, long expected)
	{
		_ = long.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input, new Action<string[], bool>(Callback), iterations), out long actual);
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
