﻿namespace AdventOfCode.Solutions.Year2019;

/// <summary>
/// Day 04: Secure Container
/// https://adventofcode.com/2019/day/4
/// </summary>
public class Day04 {

	private static string Solution1(string[] input) {
		string[] inputs = input[0].Split("-");
		int start = int.Parse(inputs[0]);
		int end = int.Parse(inputs[1]);

		int count = 0;
		for (int i = start; i <= end; i++) {
			if (IsValidPassword(i)) {
				count++;
			}
		}

		return count.ToString();
	}

	private static bool IsValidPassword(int i) {
		bool adjacentDigits = false;
		char prevDigit = ' ';

		foreach (char digit in i.ToString()) {
			if (digit < prevDigit) {
				return false;
			} else if (prevDigit == digit) {
				adjacentDigits = true;
			}
			prevDigit = digit;
		}

		if (adjacentDigits) {
			return true;
		}

		return false;
	}

	private static string Solution2(string[] input) {
		string[] inputs = input[0].Split("-");
		int start = int.Parse(inputs[0]);
		int end = int.Parse(inputs[1]);

		int count = 0;
		for (int i = start; i <= end; i++) {
			if (IsValidPassword2(i)) {
				count++;
			}
		}

		return count.ToString();
	}

	private static bool IsValidPassword2(int i) {
		bool adjacentDigits = false;
		int adjacentDigitsCount = 1;
		char prevDigit = ' ';

		foreach (char digit in i.ToString()) {
			if (digit < prevDigit) {
				return false;
			} else if (prevDigit == digit) {
				adjacentDigitsCount++;
			} else {
				if (adjacentDigitsCount == 2) {
					adjacentDigits = true;
				}
				adjacentDigitsCount = 1;
			}
			prevDigit = digit;
		}

		if (adjacentDigits || adjacentDigitsCount == 2) {
			return true;
		}

		return false;
	}



	#region Problem initialisation
	public static string Part1(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution1(input).ToString();
	}
	public static string Part2(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution2(input).ToString();
	}
	#endregion

}