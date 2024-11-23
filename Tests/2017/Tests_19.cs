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


	private void Callback(string[] lines, bool _)
	{
		if (lines is null or []) {
			return;
		}

		testOutputHelper.WriteLine(string.Join(Environment.NewLine, lines));
	}

}
