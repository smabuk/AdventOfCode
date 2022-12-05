namespace AdventOfCode.Tests._2022;

public class Tests_05_Supply_Stacks {
	[Theory]
	[InlineData("""
		    [D]    
		[N] [C]    
		[Z] [M] [P]
		 1   2   3 

		move 1 from 2 to 1
		move 3 from 1 to 3
		move 2 from 2 to 1
		move 1 from 1 to 2
		"""
		, "CMZ")]
	public void Part1(string input, string expected) {
		string actual = SolutionRouter.SolveProblem(2022, 5, 1, input);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		    [D]    
		[N] [C]    
		[Z] [M] [P]
		 1   2   3 

		move 1 from 2 to 1
		move 3 from 1 to 3
		move 2 from 2 to 1
		move 1 from 1 to 2
		"""
		, "MCD")]
	public void Part2(string input, string expected) {
		string actual = SolutionRouter.SolveProblem(2022, 5, 2, input);
		Assert.Equal(expected, actual);
	}
}
