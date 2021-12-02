namespace AdventOfCode.Tests.Year2019;

public class Tests_06_Universal_Orbit_Map {
	[Theory]
	[InlineData(new string[] {
		"COM)B",
		"B)C",
		"C)D",
		"D)E",
		"E)F",
		"B)G",
		"G)H",
		"D)I",
		"E)J",
		"J)K",
		"K)L"
		}, 42)]
	public void Part1(string[] input, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2019, 6, 1, input), out long actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] {
		"COM)B",
		"B)C",
		"C)D",
		"D)E",
		"E)F",
		"B)G",
		"G)H",
		"D)I",
		"E)J",
		"J)K",
		"K)L",
		"K)YOU",
		"I)SAN"
		}, 4)]
	public void Part2(string[] input, int expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2019, 6, 2, input), out long actual);
		Assert.Equal(expected, actual);
	}
}
