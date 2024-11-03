namespace AdventOfCode.Tests.Year2016;

public class Tests_11_Radioisotope_Thermoelectric_Generators(ITestOutputHelper testOutputHelper)
{
	const int DAY = 11;

	[Theory]
	[InlineData("""
		The first floor contains a hydrogen-compatible microchip and a lithium-compatible microchip.
		The second floor contains a hydrogen generator.
		The third floor contains a lithium generator.
		The fourth floor contains nothing relevant.
		""", 11)]
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
