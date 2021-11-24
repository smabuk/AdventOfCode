namespace AdventOfCode.Tests.Year2021;

public class Tests_01_xxxxxxxxx {
	[Theory(Skip ="Not yet available")]
	[InlineData(new string[] {
			"Hit Points: 12",
			"Damage: 7",
			"Armor: 2"
		}, 8)]
	[InlineData(new string[] {
			"Hit Points: 104",
			"Damage: 8",
			"Armor: 1"
		}, 78)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2021, 1, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory(Skip = "Not yet available")]
	[InlineData(new string[] {
			"Hit Points: 104",
			"Damage: 8",
			"Armor: 1"
		}, 148)]
	public void Part2(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2021, 1, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
