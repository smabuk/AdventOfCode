namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 01: Trebuchet
/// https://adventofcode.com/2023/day/01
/// </summary>
[Description("Trebuchet")]
public sealed partial class Day01 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();


	private static int Solution1(string[] input) => input.Select(ParseLine).Sum();

	private static int Solution2(string[] input) => input.Select(ParseLinePart2).Sum();

	private static int ParseLine(string input)
	{
		MatchCollection match = InputRegEx().Matches(input);
		return (int.Parse(match[0].Value) * 10) + int.Parse(match[^1].Value);
	}

	private static int ParseLinePart2(string input)
	{
		string[] validMatches = [
			"one",
			"two",
			"three",
			"four",
			"five",
			"six",
			"seven",
			"eight",
			"nine",
			"1",
			"2",
			"3",
			"4",
			"5",
			"6",
			"7",
			"8",
			"9",
			];
	
		int firstValue = GetValue(validMatches
			.Select(s => (Match: s, Position: input.IndexOf(s)))
			.Where(x => x.Position != -1)
			.OrderBy(x => x.Position)
			.First()
			.Match);

		int lastValue = GetValue(
			validMatches
			.Select(s => (Match: s, Position: input.LastIndexOf(s)))
			.OrderByDescending(x => x.Position)
			.First()
			.Match);

		return (firstValue * 10) + lastValue;
	}

	private static int GetValue(string value)
	{
		return value switch
		{
			"one"   => 1,
			"two"   => 2,
			"three" => 3,
			"four"  => 4,
			"five"  => 5,
			"six"   => 6,
			"seven" => 7,
			"eight" => 8,
			"nine"  => 9,
			_ => int.Parse(value),
		};
	}

	[GeneratedRegex("""(?<number>[\d])""")]
	private static partial Regex InputRegEx();
}
