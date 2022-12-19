namespace AdventOfCode.Tests._2022;

public class Tests_19_Not_Enough_Minerals {
	[Theory]
	[InlineData("""
		Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.
		Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.
		"""
		, 33)]
	public void Part1(string input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 19, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory(Skip = "Takes too long to run")]
	[InlineData("""
		Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.
		Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.
		"""
		, 56 * 62)]
	public void Part2(string input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 19, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
