namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 08: Seven Segment Search
/// https://adventofcode.com/2021/day/XX
/// </summary>
[Description("Seven Segment Search")]
public class Day08 {

	record FourDigitDisplay(List<string> SignalPatterns, List<string> OutputValues) {

		public int GetFourDigitDisplay() {
			string[] patterns = new string[10];
			Dictionary<string, int> numbers = new();

			List<string> signalPatterns = SignalPatterns;

			foreach (string sp in signalPatterns) {
				numbers[sp] = -1;
			}

			foreach (string signalPattern in signalPatterns) {
				if (signalPattern.Length == 2) {
					patterns[1] = signalPattern;
					numbers[signalPattern] = 1;
				} else if (signalPattern.Length == 3) {
					patterns[7] = signalPattern;
					numbers[signalPattern] = 7;
				} else if (signalPattern.Length == 4) {
					patterns[4] = signalPattern;
					numbers[signalPattern] = 4;
				} else if (signalPattern.Length == 7) {
					patterns[8] = signalPattern;
					numbers[signalPattern] = 8;
				}
			}

			signalPatterns.RemoveAll(s => patterns.Contains(s));
			foreach (string signalPattern in signalPatterns) {
				if (signalPattern.Length == 6 && ContainsAllWires(signalPattern, patterns[4])) {
					patterns[9] = signalPattern;
					numbers[signalPattern] = 9;
				} else if (signalPattern.Length == 6 && ContainsAllWires(signalPattern, patterns[7])) {
					patterns[0] = signalPattern;
					numbers[signalPattern] = 0;
				} else if (signalPattern.Length == 6) {
					patterns[6] = signalPattern;
					numbers[signalPattern] = 6;
				}
			}

			signalPatterns.RemoveAll(s => patterns.Contains(s));
			foreach (string signalPattern in signalPatterns) {
				if (signalPattern.Length == 5 && ContainsAllWires(signalPattern, patterns[1])) {
					patterns[3] = signalPattern;
					numbers[signalPattern] = 3;
				} else if (signalPattern.Length == 5 && ContainsAllWires(patterns[6],signalPattern)) {
					patterns[5] = signalPattern;
					numbers[signalPattern] = 5;
				} else if (signalPattern.Length == 5) {
					patterns[2] = signalPattern;
					numbers[signalPattern] = 2;
				}
			}

			int multiplier = 1000;
			int value = 0;
			foreach (string output in OutputValues) {
				value += numbers[output] * multiplier;
				multiplier /= 10;
			}

			return value;
		}

		private bool ContainsAllWires(string larger, string smaller) {
			foreach (char c in smaller)
			{
				if (larger.Contains(c) == false) {
					return false;
				}
			}
			return true;
		}

		public int OneFourSevenEightTotal => OutputValues
			.Count(o => o.Length == 2 || o.Length == 4 || o.Length == 3 || o.Length == 7);

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
