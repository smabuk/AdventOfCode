namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 19: Linen Layout
/// https://adventofcode.com/2024/day/19
/// </summary>
[Description("Linen Layout")]
public static partial class Day19 {

	private static List<string> _towelPatterns  = [];
	private static List<string> _desiredDesigns = [];

	[Init]
	public static void LoadTowels(string[] input)
	{
		_towelPatterns  = [.. input[0].TrimmedSplit(',')];
		_desiredDesigns = [.. input[2..]];
	}

	public static int Part1(string[] _)
		=> _desiredDesigns
		.Where(design => design.IsPossible(_towelPatterns))
		.Count();

	public static bool IsPossible(this string design, List<string> patterns)
	{
		if (design is "") {
			return true;
		}

		foreach (string pattern in patterns.Where(design.StartsWith)) {
			if (design[pattern.Length..].IsPossible(patterns)) {
				return true;
			}
		}

		return false;
	}

	public static string Part2(string[] input, params object[]? args) => NO_SOLUTION_WRITTEN_MESSAGE;
}
