﻿namespace AdventOfCode.Tests.Year2023;

public class Tests_18_Lavaduct_Lagoon
{
	const int DAY = 18;

	private const string TEST_DATA = """
		R 6 (#70c710)
		D 5 (#0dc571)
		L 2 (#5713f0)
		D 2 (#d2c081)
		R 2 (#59c680)
		D 2 (#411b91)
		L 5 (#8ceee2)
		U 2 (#caa173)
		L 1 (#1b58a2)
		U 2 (#caa171)
		R 2 (#7807d2)
		U 3 (#a77fa3)
		L 2 (#015232)
		U 2 (#7a21e3)
		""";

	[Theory]
	[InlineData(TEST_DATA, 62)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_DATA, 952_408_144_115)]
	public void Part2(string input, long expected)
	{
		_ = long.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out long actual);
		actual.ShouldBe(expected);
	}
}
