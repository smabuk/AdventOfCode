namespace AdventOfCode.Solutions._2015;

/// <summary>
/// Day 08: Matchsticks
/// https://adventofcode.com/2015/day/8
/// </summary>
[Description("Matchsticks")]
public class Day08 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		List<string> inputs = input.ToList();

		int TotalChars = inputs.Sum(input => input.Length);
		return TotalChars - inputs.Sum(input => CountCharsInMemorySneakily(input));
	}

	private static int Solution2(string[] input) {
		List<string> inputs = input.ToList();

		int TotalChars = inputs.Sum(input => input.Length);
		return inputs.Sum(input => CountExtraCharsEncoded(input)) - TotalChars;
	}

	private static int CountCharsInMemorySneakily(string input) {
		// Don't replace with what they suggest as that will cause further replacements
		const string REPLACE_CHAR = "_";
		string strT = input[1..^1];
		Regex regex = new(@"(\\x[a-fA-F0-9]{2})");
		strT = regex.Replace(strT, REPLACE_CHAR);
		strT = strT
			.Replace(@"\\", REPLACE_CHAR)
			.Replace(@"\""", REPLACE_CHAR);
		return strT.Length;
	}

	private static int CountExtraCharsEncoded(string input) {
		// Don't replace with what they suggest as that will cause further replacements
		const string ENCODE_CHAR = @"__";
		string strT = input;
		strT = strT
			.Replace(@"\", ENCODE_CHAR)
			.Replace(@"""", ENCODE_CHAR);
		return strT.Length + 2; // (the quotes on either end)
	}
}
