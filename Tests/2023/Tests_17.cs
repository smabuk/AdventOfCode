﻿namespace AdventOfCode.Tests.Year2023;

public class Tests_17_Clumsy_Crucible
{
	const int DAY = 17;

	private const string TEST_DATA = """
		2413432311323
		3215453535623
		3255245654254
		3446585845452
		4546657867536
		1438598798454
		4457876987766
		3637877979653
		4654967986887
		4564679986453
		1224686865563
		2546548887735
		4322674655533
		""";

	[Theory]
	[InlineData(TEST_DATA, 102)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData(TEST_DATA, 94)]
	[InlineData("""
		111111111111
		999999999991
		999999999991
		999999999991
		999999999991
		""", 71)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
