namespace AdventOfCode.Tests.Year2024;

public class Tests_18_RAM_Run(ITestOutputHelper testOutputHelper)
{
	const int DAY = 18;

	[Theory]
	[InlineData("""
		5,4
		4,2
		4,5
		3,0
		2,1
		6,3
		2,4
		1,5
		0,6
		3,3
		2,6
		5,1
		1,2
		5,5
		2,5
		6,5
		1,4
		0,4
		6,4
		1,1
		6,1
		1,0
		0,5
		1,6
		2,0
		""", 7, 12, 22)]
	public void Part1(string input, int gridSize, int bytes, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, new Action<string[], bool>(Callback), gridSize, bytes), out int actual);
		actual.ShouldBe(expected);
	}


	[Theory]
	[InlineData("""
		5,4
		4,2
		4,5
		3,0
		2,1
		6,3
		2,4
		1,5
		0,6
		3,3
		2,6
		5,1
		1,2
		5,5
		2,5
		6,5
		1,4
		0,4
		6,4
		1,1
		6,1
		1,0
		0,5
		1,6
		2,0
		""", 7, 12, "6,1")]
	public void Part2(string input, int gridSize, int bytes, string expected)
	{
		string actual = SolutionRouter.SolveProblem(YEAR, DAY, PART2, input, new Action<string[], bool>(Callback), gridSize, bytes);
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
