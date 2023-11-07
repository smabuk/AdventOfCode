namespace AdventOfCode.Solutions._2018;

/// <summary>
/// Day 01: Chronal Calibration
/// https://adventofcode.com/2018/day/01
/// </summary>
[Description("Chronal Calibration")]
public sealed partial class Day01 {

	[Init]
	public static    void Init(string[] input, params object[]? _) => LoadInstructions(input);
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static IEnumerable<int> _frequencyChanges = [];

	private static void LoadInstructions(string[] input) {
		_frequencyChanges = input.Select(fc => fc.AsInt());
	}

	private static int Solution1(string[] input) {
		return _frequencyChanges.Sum();
	}

	private static int Solution2(string[] input) {

		int[] frequencyChanges = _frequencyChanges.ToArray();
		HashSet<int> results = [0];
		int index = 0;
		int result = 0;

		do {
			result += frequencyChanges[index];
			index = (index + 1) % frequencyChanges.Length;
		} while (results.Add(result));

		return result;
	}

}
