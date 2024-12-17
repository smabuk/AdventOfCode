namespace AdventOfCode.Tests.Year2024;

public class Tests_17_Chronospatial_Computer(ITestOutputHelper testOutputHelper)
{
	const int DAY = 17;

	[Theory]
	[InlineData("""
		Register A: 0
		Register B: 0
		Register C: 9

		Program: 2,6
		""", "")] // Reg B 1
	[InlineData("""
		Register A: 0
		Register B: 29
		Register C: 0

		Program: 1,7
		""", "")] // Reg B = 26
	[InlineData("""
		Register A: 0
		Register B: 2024
		Register C: 43690

		Program: 4,0
		""", "")] // Reg B = 44354
	[InlineData("""
		Register A: 10
		Register B: 0
		Register C: 0

		Program: 5,0,5,1,5,4
		""", "0,1,2")]
	[InlineData("""
		Register A: 2024
		Register B: 0
		Register C: 0

		Program: 0,1,5,4,3,0
		""", "4,2,5,6,7,7,7,7,3,1,0")]
	[InlineData("""
		Register A: 729
		Register B: 0
		Register C: 0

		Program: 0,1,5,4,3,0
		""", "4,6,3,5,6,3,5,2,1,0")]
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
