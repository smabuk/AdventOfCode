namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 03: Binary Diagnostic
/// https://adventofcode.com/2021/day/3
/// </summary>
public class Day03 {

	private static string Solution1(string[] input) {
		List<string> diagnosticReport = input.ToList();

		string gammaRate = "";
		string epsilonRate = "";

		int noOfBits = diagnosticReport.First().Length;
		int noOfInputs = diagnosticReport.Count;

		for (int i = 0; i < noOfBits; i++) {
			char mostCommon = diagnosticReport.Select(s => s[i]).Count(bit => bit == '0') > (noOfInputs / 2) ? '0' : '1';
			gammaRate += mostCommon;
			epsilonRate += mostCommon == '0' ? '1' : '0';
		}

		int result = Convert.ToInt32(gammaRate, 2) * Convert.ToInt32(epsilonRate, 2);

		return result.ToString();
	}

	private static string Solution2(string[] input) {
		List<string> diagnosticReport = input.ToList();

		int noOfBits = diagnosticReport[0].Length;

		int count = 0;
		List<string> oxygenGeneratorRatings = diagnosticReport;
		List<string> co2ScrubberRatings = diagnosticReport;

		for (int i = 0; i < noOfBits; i++) {
			if (oxygenGeneratorRatings.Count > 1) {
				count = oxygenGeneratorRatings.Count;
				char mostCommon = oxygenGeneratorRatings.Select(s => s[i]).Count(s => s == '1') >= (count / 2.0) ? '1' : '0';
				oxygenGeneratorRatings = oxygenGeneratorRatings.Where(s => s[i] == mostCommon).ToList();
			}
			if (co2ScrubberRatings.Count > 1) {
				count = co2ScrubberRatings.Count;
				char leastCommon = co2ScrubberRatings.Select(s => s[i]).Count(s => s == '0') <= (count / 2.0) ? '0' : '1';
				co2ScrubberRatings = co2ScrubberRatings.Where(s => s[i] == leastCommon).ToList();
			}
		}

		int result = 
			Convert.ToInt32(oxygenGeneratorRatings.First(), 2)
			* Convert.ToInt32(co2ScrubberRatings.First(), 2);

		return result.ToString();
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
