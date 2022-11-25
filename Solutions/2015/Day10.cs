using System.Text;

namespace AdventOfCode.Solutions.Year2015;

/// <summary>
/// Day 10: Elves Look, Elves Say
/// https://adventofcode.com/2015/day/10
/// </summary>
[Description("Elves Look, Elves Say")]
public class Day10 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static long Solution1(string[] input) {
		string inputString = input[0];

		string newString = LookAndSay(inputString);
		for (int i = 0; i < 39; i++) {
			newString = LookAndSay(newString);
		}
		return newString.Length;
	}

	private static long Solution2(string[] input) {
		string inputString = input[0];

		string newString = LookAndSay(inputString);
		for (int i = 0; i < 49; i++) {
			newString = LookAndSay(newString);
		}
		return newString.Length;
	}
	private static string LookAndSay(string inputString) {
		char current = inputString[0];
		char c;
		StringBuilder sb = new();
		int jump = 0;
		for (int index = 0; index < inputString.Length; index += jump) {
			c = inputString[index];
			if (index + 3 <= inputString.Length && c == inputString[index + 1] && c == inputString[index + 2]) {
				sb.Append('3');
				sb.Append(c);
				jump = 3;
			} else if (index + 2 <= inputString.Length && c == inputString[index + 1]) {
				sb.Append('2');
				sb.Append(c);
				jump = 2;
			} else if (index + 1 <= inputString.Length) {
				sb.Append('1');
				sb.Append(c);
				jump = 1;
			}
		}
		return sb.ToString();
	}
}
