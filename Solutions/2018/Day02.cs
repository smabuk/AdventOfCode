namespace AdventOfCode.Solutions._2018;

/// <summary>
/// Day 02: Inventory Management System
/// https://adventofcode.com/2018/day/02
/// </summary>
[Description("Inventory Management System")]
public sealed partial class Day02 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		int noContaining2Letters = 0;
		int noContaining3Letters = 0;
		foreach (string boxId in input) {
			if (boxId.GroupBy(x => x).Any(g => g.Count() == 2)) {
				noContaining2Letters++;
			}
			if (boxId.GroupBy(x => x).Any(g => g.Count() == 3)) {
				noContaining3Letters++;
			}
		}
		return noContaining2Letters * noContaining3Letters;
	}

	private static string Solution2(string[] input) {
		for (int i = 0; i < input.Count(); i++) {
			string? boxId = input
				.Select(boxId => boxId.Remove(i, 1))
				.GroupBy(boxId => boxId)
				.Where(g => g.Count() == 2)
				.SingleOrDefault()
				?.Key;
			if (boxId is not null) {
				return boxId;
			}
		}

		return "** No Solution Found **";
	}
}
