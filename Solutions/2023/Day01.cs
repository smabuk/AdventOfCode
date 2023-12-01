namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 01: Trebuchet?!
/// https://adventofcode.com/2023/day/01
/// </summary>
[Description("Trebuchet?!")]
public sealed partial class Day01 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();


	private static int Solution1(string[] input) => input.Sum(i => GetCalibrationValue(i, 1));
	private static int Solution2(string[] input) => input.Sum(i => GetCalibrationValue(i, 2));


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

	private static int GetCalibrationValue(string input, int partNo)
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
}
