using System.Text;

namespace AdventOfCode.Solutions._2018;

/// <summary>
/// Day 05: Alchemical Reduction
/// https://adventofcode.com/2018/day/05
/// </summary>
[Description("Alchemical Reduction")]
public sealed partial class Day05 {
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input)
	{
		return ReactPolymer(input[0]).Length;
	}

	private static int Solution2(string[] input)
	{
		string polymer = input[0];
		int bestLength = int.MaxValue;

		for (int i = 0; i < 26; i++) {
			int length = ReactPolymer(polymer
				.Replace(Convert.ToChar(i + 'a').ToString(), "")
				.Replace(Convert.ToChar(i + 'A').ToString(), "")
				).Length;
			if (length < bestLength) {
				bestLength = length;
			}
		}

		return bestLength;
	}

	private static string ReactPolymer(string polymer)
	{
		const int OFFSET = 'a' - 'A';
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

		return polymer;
	}
}
