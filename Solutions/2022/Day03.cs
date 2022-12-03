namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 03: Rucksack Reorganization
/// https://adventofcode.com/2022/day/3
/// </summary>
[Description("Rucksack Reorganization")]
public sealed partial class Day03 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	record RecordType(string Name, int Value);

	private static int Solution1(string[] input) {
		int prioritiesSum = 0;

		for (int i = 0; i < input.Length; i++) {
			int compartmentSize = input[i].Length / 2;
			ReadOnlySpan<char> compartment1 = input[i][..compartmentSize].AsSpan();
			ReadOnlySpan<char> compartment2 = input[i][compartmentSize..].AsSpan();
			for (int j = 0; j < compartmentSize; j++) {
				char item = compartment1[j];
				if (compartment2.Contains(item)) {
					prioritiesSum += GetPriority(item);
					break;
				}
			}
		}
		return prioritiesSum;
	}


	private static int Solution2(string[] input) {
		const int GroupSize = 3;
		int prioritiesSum = 0;

		for (int i = 0; i < input.Length; i+=GroupSize) {
			int compartmentSize = input[i].Length / 2;
			ReadOnlySpan<char> group1 = input[i].AsSpan();
			ReadOnlySpan<char> group2 = input[i+1].AsSpan();
			ReadOnlySpan<char> group3 = input[i+2].AsSpan();
			foreach (char item in group1) {
				if (group2.Contains(item) && group3.Contains(item)) {
					prioritiesSum += GetPriority(item);
					break;
				}
			}
		}
		return prioritiesSum;
	}

	private static int GetPriority(char item) => (item < 'a') ? item - 64 + 26 : item - 96;
}
