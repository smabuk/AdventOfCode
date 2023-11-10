using System.Text;

namespace AdventOfCode.Solutions._2018;

/// <summary>
/// Day 05: Alchemical Reduction
/// https://adventofcode.com/2018/day/05
/// </summary>
[Description("Alchemical Reduction")]
public sealed partial class Day05 {
	private const int OFFSET = 'a' - 'A';

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		string polymer = input[0];

		int currentLength;
		do {
			StringBuilder newPolymer = new();
			currentLength = polymer.Length;
			for (int i = 0; i < polymer.Length; i++) {
				if (i < polymer.Length - 1 && (polymer[i] + OFFSET == polymer[i + 1] || polymer[i] == polymer[i + 1] + OFFSET)) {
					i++;
				} else {
					_ = newPolymer.Append(polymer[i]);
				}
			}
			polymer = newPolymer.ToString();
		} while (polymer.Length != 0 && polymer.Length != currentLength);

		return polymer.Length;
	}

	private static string Solution2(string[] input) {
		//string inputLine = input[0];
		//List<string> inputs = input.ToList();
		return "** Solution not written yet **";
	}
}
