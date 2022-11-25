namespace AdventOfCode.Solutions.Year2020;

/// <summary>
/// Day 2: Password Philosophy
/// https://adventofcode.com/2020/day/2
/// </summary>
[Description("Password Philosophy")]
public static class Day02 {
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	public static long Solution1(string[] input) {
		return CountValidPasswords_Part1(input);
	}

	public static long Solution2(string[] input) {
		return CountValidPasswords_Part2(input);
	}

	public static long CountValidPasswords_Part1(string[] passwordList) {
		int validPasswordCount = 0;

		foreach (string entry in passwordList) {
			string[] components = entry.Split(" ");
			string[] minandmax = components[0].Split("-");
			_ = int.TryParse(minandmax[0], out int min);
			_ = int.TryParse(minandmax[1], out int max);
			char letter = components[1][0];
			string password = components[2];
			int countOfLetter = password.Count(c => c == letter);
			if (min <= countOfLetter && countOfLetter <= max) {
				validPasswordCount++;
			}
		}
		return validPasswordCount;
	}

	public static long CountValidPasswords_Part2(string[] passwordList) {
		int validPasswordCount = 0;

		foreach (string entry in passwordList) {
			string[] components = entry.Split(" ");
			string[] positions = components[0].Split("-");
			_ = int.TryParse(positions[0], out int pos1);
			_ = int.TryParse(positions[1], out int pos2);
			char letter = components[1][0];
			char letterPos1 = components[2][pos1 - 1];
			char letterPos2 = components[2][pos2 - 1];

			if (letterPos1 != letterPos2 && (letterPos1 == letter || letterPos2 == letter)) {
				validPasswordCount++;
			}
		}
		return validPasswordCount;
	}

}
