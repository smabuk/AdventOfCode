namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 06: Tuning Trouble
/// https://adventofcode.com/2022/day/6
/// </summary>
[Description("Tuning Trouble")]
public sealed partial class Day06 {

	public static string Part1(string input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string input) => GetMessageStart(input, 4);
	private static int Solution2(string input) => GetMessageStart(input, 14);

	private static int GetMessageStart(string input, int messageSize) {
		for (int i = 0; i < input.Length - messageSize; i++) {
			if (input[i..(i + messageSize)].Distinct().Count() == messageSize) {
				return i + messageSize;
			}
		}
		throw new Exception("No answer found");
	}
}
