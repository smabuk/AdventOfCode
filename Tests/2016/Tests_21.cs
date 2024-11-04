﻿namespace AdventOfCode.Tests.Year2016;

public class Tests_21_Scrambled_Letters_and_Hash
{
	const int DAY = 21;

	[Theory]
	[InlineData("""
		swap position 4 with position 0
		swap letter d with letter b
		reverse positions 0 through 4
		rotate left 1 step
		move position 1 to position 4
		move position 3 to position 0
		rotate based on position of letter b
		rotate based on position of letter d
		""", "abcde", "decab")]
	[InlineData("rotate left 0 step", "abcde", "abcde")]
	[InlineData("rotate right 1 step", "abcde", "eabcd")]
	public void Part1(string input, string password, string expected)
	{
		string actual = SolutionRouter.SolveProblem(YEAR, DAY, PART1, input, password);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData("""
		swap position 4 with position 0
		swap letter d with letter b
		reverse positions 0 through 4
		rotate left 1 step
		move position 1 to position 4
		move position 3 to position 0
		rotate based on position of letter b
		rotate based on position of letter d
		""", "decab", "abcde")]
	[InlineData("swap position 4 with position 0", "ebcda", "abcde")]
	[InlineData("swap letter d with letter b", "edcba", "ebcda")]
	[InlineData("reverse positions 0 through 4", "abcde", "edcba")]
	[InlineData("rotate left 1 step", "bcdea", "abcde")]
	[InlineData("move position 1 to position 4", "bdeac", "bcdea")]
	[InlineData("move position 3 to position 0", "abdec", "bdeac")]
	[InlineData("rotate based on position of letter b", "ecabd", "abdec")]
	[InlineData("rotate based on position of letter d", "decab", "ecabd")]

	[InlineData("rotate right 1 step", "eabcd", "abcde")]
	public void Part2(string input, string scrambled, string expected)
	{
		string actual = SolutionRouter.SolveProblem(YEAR, DAY, PART2, input, scrambled);
		actual.ShouldBe(expected);
	}
}
