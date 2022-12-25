namespace AdventOfCode.Tests._2022;

public class Tests_25_Full_of_Hot_Air {
	[Theory]
	[InlineData("""
		1=-0-2
		12111 
		 2=0= 
		   21 
		 2=01 
		  111 
		20012 
		  112 
		1=-1= 
		 1-12 
		   12 
		   1= 
		  122 
		"""
		, "2=-1=0")]
	public void Part1(string input, string expected) {
		string actual = SolutionRouter.SolveProblem(2022, 25, 1, input);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		b
		"""
		, 9999)]
	public void Part2(string input, long expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 25, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
