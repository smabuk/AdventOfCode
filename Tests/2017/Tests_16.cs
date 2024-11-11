namespace AdventOfCode.Tests.Year2017;

public class Tests_16_Permutation_Promenade
{
	const int DAY = 16;

	[Theory]
	[InlineData("""s1,x3/4,pe/b""", "abcde", "baedc")]
	public void Part1(string input, string programs,string expected)
	{
		string actual = SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, programs);
		actual.ShouldBe(expected);
	}
}
