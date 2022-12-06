namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 06: Tuning Trouble
/// https://adventofcode.com/2022/day/6
/// </summary>
[Description("Tuning Trouble")]
public sealed partial class Day06 {

	public static string Part1(string input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string input, params object[]? _) => Solution2(input).ToString();

	private record struct RecordType(string Name, int Value);

	private static int Solution1(string input) {
		//string inputLine = input[0];
		//List<string> inputs = input.ToList();
		//List<RecordType> instructions = input.Select(i => ParseLine(i)).ToList();
		int startOfPacketMarker = 0;
		for (int i = 0; i < input.Length - 4; i++) {
			string marker = input[i..(i + 4)];
			if (marker.ToCharArray().Distinct().Count() == 4) {
				startOfPacketMarker = i + 4;
				break;
			}
		}
		return startOfPacketMarker;
	}

	private static string Solution2(string input) {
		//string inputLine = input[0];
		//List<string> inputs = input.ToList();
		//List<RecordType> instructions = input.Select(i => ParseLine(i)).ToList();
		return "** Solution not written yet **";
	}

	private static RecordType ParseLine(string input) {
		//MatchCollection match = InputRegEx().Matches(input);
		Match match = InputRegEx().Match(input);
		if (match.Success) {
			return new(match.Groups["opts"].Value, int.Parse(match.Groups["number"].Value));
		}
		return new();
	}

	[GeneratedRegex("""(?<opts>opt1|opt2|opt3) (?<number>[\+\-]\d+)""")]
	private static partial Regex InputRegEx();
}
