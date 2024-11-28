namespace AdventOfCode.Tests.Year2017;

public class Tests_25_The_Halting_Problem
{
	const int DAY = 25;

	[Theory]
	[InlineData("""
		Begin in state A.
		Perform a diagnostic checksum after 6 steps.

		In state A:
		  If the current value is 0:
		    - Write the value 1.
		    - Move one slot to the right.
		    - Continue with state B.
		  If the current value is 1:
		    - Write the value 0.
		    - Move one slot to the left.
		    - Continue with state B.

		In state B:
		  If the current value is 0:
		    - Write the value 1.
		    - Move one slot to the left.
		    - Continue with state A.
		  If the current value is 1:
		    - Write the value 1.
		    - Move one slot to the right.
		    - Continue with state A.
		""", 3)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

}
