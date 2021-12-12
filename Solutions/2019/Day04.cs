namespace AdventOfCode.Solutions.Year2019;

/// <summary>
/// Day 04: Secure Container
/// https://adventofcode.com/2019/day/4
/// </summary>
[Description("Secure Container")]
public class Day04 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static string Solution1(string[] input) {
		(int start, int end) = ParseInputs(input);

		int count = 0;
		for (int i = start; i <= end; i++) {
			if (IsValidPassword(i)) {
				count++;
			}
		}

		return count.ToString();
	}

	private static string Solution2(string[] input) {
		(int start, int end) = ParseInputs(input);

		int count = 0;
		for (int i = start; i <= end; i++) {
			if (IsValidPassword2(i)) {
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
	private static (int start, int end) ParseInputs(string[] input) {
		string[] inputs = input[0].Split("-");
		int start = int.Parse(inputs[0]);
		int end = int.Parse(inputs[1]);
		return (start, end);
	}
}
