﻿namespace AdventOfCode.Tests._2022;

public class Tests_22_Monkey_Map {
	[Theory]
	[InlineData("""
		        ...#
		        .#..
		        #...
		        ....
		...#.......#
		........#...
		..#....#....
		..........#.
		        ...#....
		        .....#..
		        .#......
		        ......#.

		10R5L5R10L4R5L5
		"""
		, 6032)]
	public void Part1(string input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 22, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""

		"""
		, 9999)]
	public void Part2(string input, long expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 22, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
