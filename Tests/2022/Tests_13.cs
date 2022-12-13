namespace AdventOfCode.Tests._2022;

public class Tests_13_Distress_Signal {
	[Theory]
	[InlineData("""
		[1,1,3,1,1]
		[1,1,5,1,1]

		[[1],[2,3,4]]
		[[1],4]

		[9]
		[[8,7,6]]

		[[4,4],4,4]
		[[4,4],4,4,4]

		[7,7,7,7]
		[7,7,7]

		[]
		[3]

		[[[]]]
		[[]]

		[1,[2,[3,[4,[5,6,7]]]],8,9]
		[1,[2,[3,[4,[5,6,0]]]],8,9]
		"""
		, 13)]
	[InlineData("""
		[[[]]]
		[[]]
		"""
		, 0)]
	[InlineData("""
		[[]]
		[[[]]]
		"""
		, 1)]
	[InlineData("""
		[[],[9,4]]
		[[],[[5,4,1],7],[[],4],[],[3,1,[[1],3,4,2]]]
		"""
		, 0)]
	public void Part1(string input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 13, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		[1,1,3,1,1]
		[1,1,5,1,1]

		[[1],[2,3,4]]
		[[1],4]

		[9]
		[[8,7,6]]

		[[4,4],4,4]
		[[4,4],4,4,4]

		[7,7,7,7]
		[7,7,7]

		[]
		[3]

		[[[]]]
		[[]]

		[1,[2,[3,[4,[5,6,7]]]],8,9]
		[1,[2,[3,[4,[5,6,0]]]],8,9]
		"""
		, 1, 140)]
	public void Part2(string input, int noOfRounds, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2022, 13, 2, input, noOfRounds), out long actual);
		Assert.Equal(expected, actual);
	}
}
