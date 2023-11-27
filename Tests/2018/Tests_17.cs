namespace AdventOfCode.Tests.Year2018;

public class Tests_17_Reservoir_Research(ITestOutputHelper testOutputHelper)
{
	const int DAY = 17;

	[Theory]
	[InlineData("""
		x=495, y=2..7
		y=7, x=495..501
		x=501, y=3..7
		x=498, y=2..4
		x=506, y=1..2
		x=498, y=10..13
		x=504, y=10..13
		y=13, x=498..504
		""", 57)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, new Action<string[], bool>(Callback)), out int actual);
		Assert.Equal(expected, actual);
	}

	//[Theory]
	//[InlineData("""
	//	x=495, y=2..7
	//	y=7, x=495..501
	//	x=501, y=3..7
	//	x=498, y=2..4
	//	x=506, y=1..2
	//	x=498, y=10..13
	//	x=504, y=10..13
	//	y=13, x=498..504
	//	x=495, y=2..7
	//	y=7, x=495..501
	//	x=501, y=3..7
	//	x=498, y=2..4
	//	x=506, y=1..2
	//	x=498, y=10..13
	//	x=504, y=10..13
	//	y=13, x=498..504
	//	""", 999999)]
	//public void Part2(string input, int expected)
	//{
	//	_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input, new Action<string[], bool>(Callback)), out int actual);
	//	Assert.Equal(expected, actual);
	//}


	private void Callback(string[] lines, bool _)
	{
		if (lines is null or []) {
			return;
		}

		testOutputHelper.WriteLine(string.Join(Environment.NewLine, lines));
	}

}
