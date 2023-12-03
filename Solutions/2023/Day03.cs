﻿namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 03: Gear Ratios
/// https://adventofcode.com/2023/day/03
/// </summary>
[Description("Gear Ratios")]
public sealed partial class Day03 {

	//[Init]
	//public static    void Init(string[] input, params object[]? _) => LoadInstructions(input);
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		List<int> partNos = [];

		char[,] engine = string.Join("", input).To2dArray<char>(input[0].Length);
		int maxX = input[0].Length - 1;
		int maxY = input.Length - 1;
		for (int row = 0; row < input.Length; row++) {
			string engineLine = input[row].Replace('.', ' ');
			MatchCollection possibleParts = Regex.Matches(engineLine, @"(?<number>\d+)");
			foreach (Match number in possibleParts) {
				bool foundPart = false;
				for (int y = Math.Max(row - 1, 0); y <= Math.Min(row + 1, maxY); y++) {
					for (int x = Math.Max(number.Index - 1, 0); x < Math.Min(number.Index + number.Length + 1, maxX); x++) {
						char cell = engine[x, y];
						if (!char.IsDigit(cell) && cell is not '.') {
							partNos.Add(number.Value.AsInt());
							foundPart = true;
							break;
						}
					}
					if (foundPart) {
						break;
					}
				}
			}
		}

		return partNos.Sum();
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}
}
