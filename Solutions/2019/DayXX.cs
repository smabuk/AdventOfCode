namespace AdventOfCode.Solutions._2019;

/// <summary>
/// Day XX: Title
/// https://adventofcode.com/2019/day/XX
/// </summary>
[Description("")]
public class DayXX {

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
