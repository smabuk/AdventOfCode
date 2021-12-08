namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 08: Seven Segment Search
/// https://adventofcode.com/2021/day/XX
/// </summary>
[Description("Seven Segment Search")]
public class Day08 {

	record FourDigitDisplay(List<string> SignalPatterns, List<string> OutputValues) {

		public int GetFourDigitDisplay() {
			List<string> signalPatterns = SignalPatterns;

			Dictionary<string, int>? numbers = signalPatterns
				.ToDictionary(sp => sp, _ => -1);
			string[] patterns = new string[10];
			
			SetPatternNumber(signalPatterns.Where(sp => sp.Length == 2).Single(), 1);
			SetPatternNumber(signalPatterns.Where(sp => sp.Length == 3).Single(), 7);
			SetPatternNumber(signalPatterns.Where(sp => sp.Length == 4).Single(), 4);
			SetPatternNumber(signalPatterns.Where(sp => sp.Length == 7).Single(), 8);
			signalPatterns.RemoveAll(s => patterns.Contains(s));

			foreach (string signalPattern in signalPatterns.Where(sp => sp.Length == 6)) {
				if (signalPattern.ContainsAllWires(patterns[4])) {
					SetPatternNumber(signalPattern, 9);
				} else if (signalPattern.ContainsAllWires(patterns[7])) {
					SetPatternNumber(signalPattern, 0);
				} else {
					SetPatternNumber(signalPattern, 6);
				}
			}
			signalPatterns.RemoveAll(s => patterns.Contains(s));

			foreach (string signalPattern in signalPatterns) {
				if (signalPattern.ContainsAllWires(patterns[1])) {
					SetPatternNumber(signalPattern, 3);
				} else if (patterns[6].ContainsAllWires(signalPattern)) {
					SetPatternNumber(signalPattern, 5);
				} else {
					SetPatternNumber(signalPattern, 2);
				}
			}

			return int.Parse(String.Join("", OutputValues.Select(o => numbers[o])));




			void SetPatternNumber(string signalPattern, int number) {
				patterns[number] = signalPattern;
				numbers[signalPattern] = number;
			}
		}

		public int OneFourSevenEightTotal => 
			OutputValues.Count(o => new int[] { 2, 3, 4, 7 }.Contains(o.Length));

	}
	private static int Solution1(string[] input) {
		List<FourDigitDisplay> fourDigitDisplays = input.Select(i => ParseLine(i)).ToList();

		return fourDigitDisplays.Sum(f => f.OneFourSevenEightTotal);
	}

	private static int Solution2(string[] input) {
		List<FourDigitDisplay> fourDigitDisplays = input.Select(i => ParseLine(i)).ToList();

		return fourDigitDisplays.Sum(fdd => fdd.GetFourDigitDisplay());
	}

	private static FourDigitDisplay ParseLine(string input) {

		List<string> signalPatterns = input
			.Split(" | ")
			.First()
			.Split(" ")
			.Select(s => new string(s.OrderBy(c => c).ToArray()))
			.ToList();
		List<string> outputValues = input
			.Split(" | ")
			.Last()
			.Split(" ")
			.Select(s => new string(s.OrderBy(c => c).ToArray()))
			.ToList();
		return new FourDigitDisplay(signalPatterns, outputValues);
	}





	#region Problem initialisation
	public static string Part1(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution1(input).ToString();
	}
	public static string Part2(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution2(input).ToString();
	}
	#endregion

}

internal static class Day08_StringExtensions {
	internal static bool ContainsAllWires(this string longer, string shorter) {
		foreach (char c in shorter) {
			if (longer.Contains(c) == false) {
				return false;
			}
		}
		return true;
	}
}