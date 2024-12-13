﻿namespace AdventOfCode.Tests.Year2024;

public class Tests_13_Claw_Contraption
{
	const int DAY = 13;

	[Theory]
	[InlineData("""
		Button A: X+94, Y+34
		Button B: X+22, Y+67
		Prize: X=8400, Y=5400
		""", 280)]
	[InlineData("""
		Button A: X+17, Y+86
		Button B: X+84, Y+37
		Prize: X=7870, Y=6450
		""", 200)]
	[InlineData("""
		Button A: X+94, Y+34
		Button B: X+22, Y+67
		Prize: X=8400, Y=5400

		Button A: X+26, Y+66
		Button B: X+67, Y+21
		Prize: X=12748, Y=12176

		Button A: X+17, Y+86
		Button B: X+84, Y+37
		Prize: X=7870, Y=6450

		Button A: X+69, Y+23
		Button B: X+27, Y+71
		Prize: X=18641, Y=10279
		""", 480)]
	public void Part1(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART1, input), out int actual);
		actual.ShouldBe(expected);
	}

	[Theory]
	[InlineData("""
		Button A: X+94, Y+34
		Button B: X+22, Y+67
		Prize: X=8400, Y=5400
		""", 280)]
	[InlineData("""
		Button A: X+17, Y+86
		Button B: X+84, Y+37
		Prize: X=7870, Y=6450
		""", 200)]
	[InlineData("""
		Button A: X+94, Y+34
		Button B: X+22, Y+67
		Prize: X=8400, Y=5400

		Button A: X+26, Y+66
		Button B: X+67, Y+21
		Prize: X=12748, Y=12176

		Button A: X+17, Y+86
		Button B: X+84, Y+37
		Prize: X=7870, Y=6450

		Button A: X+69, Y+23
		Button B: X+27, Y+71
		Prize: X=18641, Y=10279
		""", 480)]
	public void Part2(string input, int expected)
	{
		_ = int.TryParse(SolutionRouter.SolveProblem(YEAR, DAY, PART2, input, true), out int actual);
		actual.ShouldBe(expected);
	}
}
