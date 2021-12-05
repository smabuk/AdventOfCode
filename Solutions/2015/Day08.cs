namespace AdventOfCode.Solutions.Year2015;

/// <summary>
/// Day 08: Matchsticks
/// https://adventofcode.com/2015/day/8
/// </summary>
[Description("Matchsticks")]
public class Day08 {

	private static int Solution1(string[] input) {
		List<string> inputs = input.ToList();

		int TotalChars = inputs.Sum(input => input.Length);
		return TotalChars - inputs.Sum(input => CountCharsInMemorySneakily(input));
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

	private static int Solution2(string[] input) {
		List<string> inputs = input.ToList();

		int TotalChars = inputs.Sum(input => input.Length);
		return inputs.Sum(input => CountExtraCharsEncoded(input)) - TotalChars;
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





	#region Problem initialisation
	/// <summary>
	/// Sets up the inputs for Part1 of the problem and calls Solution1
	/// </summary>
	/// <param name="input"></param>
	/// Array of strings
	/// <param name="args"></param>
	/// Optional extra parameters that may be required as input to the problem
	/// <returns></returns>
	public static string Part1(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		// int arg1 = GetArgument(args, 1, 25);
		input = input.StripTrailingBlankLineOrDefault();
		return Solution1(input).ToString();
	}
	/// <summary>
	/// Sets up the inputs for Part2 of the problem and calls Solution2
	/// </summary>
	/// <param name="input"></param>
	/// Array of strings
	/// <param name="args"></param>
	/// Optional extra parameters that may be required as input to the problem
	/// <returns></returns>
	public static string Part2(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		// int arg1 = GetArgument(args, 1, 25);
		input = input.StripTrailingBlankLineOrDefault();
		return Solution2(input).ToString();
	}
	#endregion

}
