namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day XX: Title
/// https://adventofcode.com/2022/day/XX
/// </summary>
[Description("")]
public sealed partial class DayXX {

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
		//MatchCollection match = InputRegEx().Matches(input);
		Match match = InputRegEx().Match(input);
		if (match.Success) {
			return new(match.Groups["opts"].Value, int.Parse(match.Groups["number"].Value));
		}
		return null!;
	}

	[GeneratedRegex("""(?<opts>opt1|opt2|opt3) (?<number>[\+\-]\d+)""")]
	private static partial Regex InputRegEx();
}
