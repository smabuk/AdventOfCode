﻿namespace AdventOfCode.Tests._2022;

public class Tests_02_Rock_Paper_Scissors {
	[Theory]
	[InlineData("""
		A Y
		B X
		C Z
		"""
		, 15)]
	public void Part1(string input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 2, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		A Y
		B X
		C Z
		"""
		, 12)]
	public void Part2(string input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 2, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
