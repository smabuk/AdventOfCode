namespace AdventOfCode.Solutions._2018;

/// <summary>
/// Day 02: Inventory Management System
/// https://adventofcode.com/2018/day/02
/// </summary>
[Description("Inventory Management System")]
public sealed partial class Day02 {

	[Init]
	public static    void Init(string[] input, params object[]? _) => LoadInstructions(input);
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static IEnumerable<string> _boxIds = [];

	private static void LoadInstructions(string[] input) {
		_boxIds = input;
	}

	private static int Solution1(string[] input) {
		int noContaining2Letters = 0;
		int noContaining3Letters = 0;
		foreach (string boxId in _boxIds) {
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
		return "** Solution not written yet **";
	}
}
