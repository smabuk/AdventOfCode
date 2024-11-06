﻿namespace AdventOfCode.Tests.Year2017;

public class Tests_02_Corruption_Checksum
{
	const int DAY = 02;

	[Theory]
	[InlineData("""
		5	1	9	5
		7	5	3
		2	4	6	8
		""", 18)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData("""
		5	 9	 2	 8
		9	 4	 7	 3
		3	 8	 6	 5
		""", 9)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input), out int actual);
		actual.ShouldBe(expected);
	}
}
