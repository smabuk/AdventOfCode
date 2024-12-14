namespace AdventOfCode.Tests.Year2024;

public class Tests_14_Restroom_Redoubt(ITestOutputHelper testOutputHelper)
{
	const int DAY = 14;

	[Theory]
	[InlineData("""
		p=0,4 v=3,-3
		p=6,3 v=-1,-3
		p=10,3 v=-1,2
		p=2,0 v=2,-1
		p=0,0 v=1,3
		p=3,0 v=-2,-2
		p=7,6 v=-1,-3
		p=3,0 v=-1,-2
		p=9,3 v=2,3
		p=7,3 v=-1,2
		p=2,4 v=2,-3
		p=9,5 v=-3,-3
		""", 11, 7, 12)]
	[InlineData("""
		p=2,4 v=2,-3
		""", 11, 7, 0)]
	public async Task Part1(string input, int width, int height, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, new Action<string[], bool>(Callback), width, height), out int actual);
		actual.ShouldBe(expected);
		await Task.Delay(200); // Allow time to visualise
	}




	private void Callback(string[] lines, bool _)
	{
		if (lines is null or []) {
			return;
		}

		testOutputHelper.WriteLine(string.Join(Environment.NewLine, lines));
	}

}
