namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 24: Arithmetic Logic Unit
/// https://adventofcode.com/2021/day/24
/// </summary>
[Description("Arithmetic Logic Unit")]
public class Day24 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	record RecordType(string Name, int Value);

	private static string Solution1(string[] input) {
		//string inputLine = input[0];
		//List<string> inputs = input.ToList();
		List<RecordType> instructions = input.Select(i => ParseLine(i)).ToList();
		return "** Solution not written yet **";
	}

	private static string Solution2(string[] input) {
		//string inputLine = input[0];
		//List<string> inputs = input.ToList();
		List<RecordType> instructions = input.Select(i => ParseLine(i)).ToList();
		return "** Solution not written yet **";
	}

	private static RecordType ParseLine(string input) {
		//MatchCollection match = Regex.Matches(input, @"(opt1|opt2|opt3) ([\+\-]\d+)");
		Match match = Regex.Match(input, @"(opt1|opt2|opt3) ([\+\-]\d+)");
		if (match.Success) {
			return new(match.Groups[1].Value, int.Parse(match.Groups[2].Value));
		}
		return null!;
	}
}
