namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 01: Trebuchet?!
/// https://adventofcode.com/2023/day/01
/// </summary>
[Description("Trebuchet?!")]
public sealed partial class Day01 {

	public static string Part1(string[] input, params object[]? args) => Solution(input, 1, args);
	public static string Part2(string[] input, params object[]? args) => Solution(input, 2, args);

	/// <summary>
	/// This solution supports 2 ways of solving the problem:
	///		Regex
	///		Linq
	/// </summary>
	private static string Solution(string[] input, int partNo, object[]? args)
	{
		return GetArgument(args, 1, "regex").ToLowerInvariant() switch
		{
			"regex" => Solution_Using_Regex(input, partNo).ToString(),
			"linq"  => Solution_Using_Linq(input, partNo).ToString(),
			_       => "** Solution not written yet **",
		};
	}

	private static int Solution_Using_Linq(string[] input, int partNo)  => input.Sum(i => GetCalibrationValue_Using_Linq(i, partNo));
	private static int Solution_Using_Regex(string[] input, int partNo) => input.Sum(i => GetCalibrationValue_Using_Regex(i, partNo));

	private static readonly Dictionary<string, int> VALID_MATCHES = new()
		{
			{ "1",     1 },
			{ "2",     2 },
			{ "3",     3 },
			{ "4",     4 },
			{ "5",     5 },
			{ "6",     6 },
			{ "7",     7 },
			{ "8",     8 },
			{ "9",     9 },
			// Use these matches as well for Part2
			{ "one",   1 },
			{ "two",   2 },
			{ "three", 3 },
			{ "four",  4 },
			{ "five",  5 },
			{ "six",   6 },
			{ "seven", 7 },
			{ "eight", 8 },
			{ "nine",  9 },
		};

	private static int GetCalibrationValue_Using_Linq(string input, int partNo)
	{
		const int NO_MATCH = -1;

		IEnumerable<(int Value, int FirstPosition, int LastPosition)> matches = VALID_MATCHES
			.Take(partNo * 9)
			.Select(vm => (vm.Value, input.IndexOf(vm.Key), input.LastIndexOf(vm.Key)));

		int firstDigit = matches
			.Where(m => m.FirstPosition != NO_MATCH)
			.MinBy(m => m.FirstPosition)
			.Value;

		int lastDigit = matches
			.Where(m => m.LastPosition != NO_MATCH)
			.MaxBy(m => m.LastPosition)
			.Value;

		return (firstDigit * 10) + lastDigit;
	}

	private static int GetCalibrationValue_Using_Regex(string input, int partNo)
	{
		int firstDigit;
		int lastDigit;

		if (partNo == 1) {
			firstDigit = VALID_MATCHES[FirstDigitRegex1().Match(input).Value];
			lastDigit  = VALID_MATCHES[LastDigitRegex1().Match(input).Value];
		} else {
			firstDigit = VALID_MATCHES[FirstDigitRegex2().Match(input).Value];
			lastDigit  = VALID_MATCHES[LastDigitRegex2().Match(input).Value];
		}
		return (firstDigit * 10) + lastDigit;
	}

	// language=regex
	private const string REGEX_PART1 = @"(\d)";
	[GeneratedRegex(REGEX_PART1)]
	private static partial Regex FirstDigitRegex1();

	[GeneratedRegex(REGEX_PART1, RegexOptions.RightToLeft)]
	private static partial Regex LastDigitRegex1();

	// language=regex
	private const string REGEX_PART2 = @"(\d|one|two|three|four|five|six|seven|eight|nine)";
	[GeneratedRegex(REGEX_PART2)]
	private static partial Regex FirstDigitRegex2();

	[GeneratedRegex(REGEX_PART2, RegexOptions.RightToLeft)]
	private static partial Regex LastDigitRegex2();
}
