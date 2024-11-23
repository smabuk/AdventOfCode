namespace AdventOfCode.Tests.Year2017;

public class Tests_19_A_Series_of_Tubes(ITestOutputHelper testOutputHelper)
{
	const int DAY = 19;

	[Theory]
	[InlineData("""
		    |          
		    |  +--+    
		    A  |  C    
		F---|----E|--+ 
		    |  |  |  D 
		    +B-+  +--+ 
		""", "ABCDEF")]
	public void Part1(string input, string expected)
	{
		string actual = SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, new Action<string[], bool>(Callback));
		actual.ShouldBe(expected);
	}


	[Theory]
	[InlineData("""
		    |          
		    |  +--+    
		    A  |  C    
		F---|----E|--+ 
		    |  |  |  D 
		    +B-+  +--+ 
		""", 38)]
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
