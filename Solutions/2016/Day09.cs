using System.Text;
using static AdventOfCode.Solutions._2016.Day09Constants;

namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 09: Explosives in Cyberspace
/// https://adventofcode.com/2016/day/09
/// </summary>
[Description("Explosives in Cyberspace")]
public sealed partial class Day09 {

	public static string Part1(string[] input) => Solution1(input).ToString();
	public static string Part2(string[] input) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		string compressed = input[0];
		StringBuilder uncompressed = new();

		for (int i = 0; i < compressed.Length; i++) {
			char current = compressed[i];
			if (current == OPEN_BRACKET) {
				int closeBracketIndex = compressed[i..].IndexOf(CLOSE_BRACKET) + i;
				string marker = compressed[(i + 1)..closeBracketIndex];
				string[] tokens = marker.Split('x');
				int chars = tokens[0].As<int>();
				int repeat = tokens[1].As<int>();
				for (int j = 0; j < repeat; j++) {
					_ = uncompressed.Append(compressed[(closeBracketIndex + 1)..(closeBracketIndex + 1 + chars)]);
				}
				i = closeBracketIndex + chars;
			} else {
				_ = uncompressed.Append(current);
			}
		}

		return uncompressed.Length;
	}

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class Day09Constants
{
	public const char OPEN_BRACKET = '(';
	public const char CLOSE_BRACKET = ')';
}
