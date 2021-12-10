namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 10: 
/// https://adventofcode.com/2021/day/10
/// </summary>
[Description("")]
public class Day10 {

	static readonly char[] OPEN_CHARS = "([{<".ToCharArray();

	private static long Solution1(string[] input) {
		return input
			.Select(line => FindFirstIllegalCharacterValue(line))
			.Sum();
	}

	private static long FindFirstIllegalCharacterValue(string line) {
		Stack<char> openChars = new();

		foreach (char character in line) {
			if (OPEN_CHARS.Contains(character)) {
				openChars.Push(character);
			} else {
				(char expectedChar, int value) = character switch {
					')' => ('(', 3),
					']' => ('[', 57),
					'}' => ('{', 1197),
					'>' => ('<', 25137),
					_ => throw new NotImplementedException(),
				};
				if (expectedChar != openChars.Pop()) {
					return value;
				}
			}
		}

		return 0;
	}

	private static long Solution2(string[] input) {
		return input
			.Where(line => FindFirstIllegalCharacterValue(line) == 0)
			.Select(line => FindClosingCharacterValues(line))
			.Median();
	}

	private static long FindClosingCharacterValues(string line) {
		Stack<char> openChars = new();

		foreach (char character in line) {
			if (OPEN_CHARS.Contains(character)) {
				openChars.Push(character);
			} else {
				openChars.Pop();
			}
		}

		long total = 0;
		while (openChars.Count > 0) {
			long score = openChars.Pop() switch {
				'(' => 1,
				'[' => 2,
				'{' => 3,
				'<' => 4,
				_ => throw new NotImplementedException()
			};
			total = 5 * total + score;
		}

		return total;
	}




	#region Problem initialisation
	public static string Part1(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		// int arg1 = GetArgument(args, 1, 25);
		input = input.StripTrailingBlankLineOrDefault();
		return Solution1(input).ToString();
	}
	public static string Part2(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		// int arg1 = GetArgument(args, 1, 25);
		input = input.StripTrailingBlankLineOrDefault();
		return Solution2(input).ToString();
	}
	#endregion

}
