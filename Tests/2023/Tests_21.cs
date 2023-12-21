namespace AdventOfCode.Tests.Year2023;

public class Tests_21_Step_Counter
{
	const int DAY = 21;

	private const string TEST_DATA = """
		...........
		.....###.#.
		.###.##..#.
		..#.#...#..
		....#.#....
		.##..S####.
		.##..#...#.
		.......##..
		.##.#.####.
		.##..##.##.
		...........
		""";

	[Theory]
	[InlineData(TEST_DATA, 16)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, 6), out int actual);
		actual.ShouldBe(expected);
	}
	[Theory]
	[InlineData(TEST_DATA,    6,       16)]
	[InlineData(TEST_DATA,   10,       50)]
	[InlineData(TEST_DATA,   50,     1594)]
	[InlineData(TEST_DATA,  100,     6536)]
	[InlineData(TEST_DATA, 1000,   668697)]
	[InlineData(TEST_DATA, 5000, 16733044)]
	public void Part2(string input, int steps, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input, steps), out int actual);
		actual.ShouldBe(expected);
	}
}
