namespace AdventOfCode.Tests.Year2018;

public class Tests_13_Mine_Cart_Madness
{
	const int DAY = 13;

	[Theory]
	[InlineData("""
		/->-\        
		|   |  /----\
		| /-+--+-\  |
		| | |  | v  |
		\-+-/  \-+--/
		  \------/   
		""", 7, 3)]
	public void Part1(string input, int expectedX, int expectedY)
	{
		string actual = SolutionRouter.SolveProblem(YEAR, DAY, PART1, input);
		string expected = $"{expectedX},{expectedY}";
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		/>-<\  
		|   |  
		| /<+-\
		| | | v
		\>+</ |
		  |   ^
		  \<->/
		""", 6, 4)]
	public void Part2(string input, int expectedX, int expectedY)
	{
		string actual = SolutionRouter.SolveProblem(YEAR, DAY, PART2, input);
		string expected = $"{expectedX},{expectedY}";
		Assert.Equal(expected, actual);
	}

}
