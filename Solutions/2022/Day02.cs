namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 02: Rock Paper Scissors
/// https://adventofcode.com/2022/day/2
/// </summary>
[Description("Rock Paper Scissors")]
public partial class Day02 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	record Round(string OpponentChoice, string MyChoice) {
		public int MyChoiceScore => MyChoice switch {
			"X" => 1,
			"Y" => 2,
			"Z" => 3,
			_ => 0,
		};

		public int MyResultScore {
			get {
				if (OpponentChoice == "A" && MyChoice == "X"
					|| OpponentChoice == "B" && MyChoice == "Y"
					|| OpponentChoice == "C" && MyChoice == "Z") {
					return MyChoiceScore + 3;
				} else if (OpponentChoice == "A" && MyChoice == "Y"
					|| OpponentChoice == "B" && MyChoice == "Z"
					|| OpponentChoice == "C" && MyChoice == "X") {
					return MyChoiceScore + 6;
				}
				return MyChoiceScore;
			}
		}
	};

	private static int Solution1(string[] input) {
		List<Round> round = input.Select(i => ParseLine(i)).ToList();
		int totalScore = round.Sum(x => x.MyResultScore);
		return totalScore;
	}

	private static string Solution2(string[] input) {
		//string inputLine = input[0];
		//List<string> inputs = input.ToList();
		List<Round> instructions = input.Select(i => ParseLine(i)).ToList();
		return "** Solution not written yet **";
	}

	private static Round ParseLine(string input) {
		//MatchCollection match = InputRegEx().Matches(input);
		Match match = InputRegEx().Match(input);
		if (match.Success) {
			return new(match.Groups["opp"].Value, match.Groups["me"].Value);
		}
		return null!;
	}

	[GeneratedRegex("""(?<opp>A|B|C) (?<me>\D)""")]
	private static partial Regex InputRegEx();
}
