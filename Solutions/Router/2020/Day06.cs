namespace AdventOfCode.Solutions.Year2020;

/// <summary>
/// Day 06: Custom Customs
/// https://adventofcode.com/2020/day/6
/// </summary>
[Description("Custom Customs")]
public class Day06 {
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		List<string> inputs = input.ToList();
		inputs.Add("");

		int totalCount = 0;
		string groupQuestions = "";
		foreach (string passenger in inputs) {
			if (string.IsNullOrWhiteSpace(passenger)) {
				totalCount += groupQuestions.Length;
				groupQuestions = "";
				continue;
			}
			foreach (char question in passenger) {
				if (groupQuestions.Contains(question) == false) {
					groupQuestions += question;
				}
			}
		}

		return totalCount;
	}

	private static int Solution2(string[] input) {
		List<string> inputs = input.ToList();
		inputs.Add("");

		int totalCount = 0;
		string groupQuestions = "";
		bool firstPerson = true;
		foreach (string passenger in inputs) {
			if (string.IsNullOrWhiteSpace(passenger)) {
				totalCount += groupQuestions.Length;
				firstPerson = true;
				groupQuestions = "";
				continue;
			}
			if (firstPerson) {
				groupQuestions = passenger;
			} else {
				foreach (char question in groupQuestions) {
					if (passenger.Contains(question) == false) {
						groupQuestions = groupQuestions.Replace(question.ToString(), "");
					}
				}
			}
			firstPerson = false;
		}

		return totalCount;
	}
}
