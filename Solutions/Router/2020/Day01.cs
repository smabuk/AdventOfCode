namespace AdventOfCode.Solutions.Year2020;

/// <summary>
/// Day 01: Report Repair
/// https://adventofcode.com/2020/day/1
/// </summary>
[Description("Report Repair")]
public static class Day01 {
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	public static long Solution1(string[] input) {
		List<int> expenseValues = new();
		foreach (string item in input) {
			expenseValues.Add(int.Parse(item));
		}
		FindSumsEqualTo(2020, expenseValues, 2, out List<int> entries);
		return entries.Aggregate(1, (product, entry) => product * entry);
	}

	public static long Solution2(string[] input) {
		List<int> expenseValues = new();
		foreach (string item in input) {
			expenseValues.Add(int.Parse(item));
		}
		FindSumsEqualTo(2020, expenseValues, 3, out List<int> entries);
		return entries.Aggregate(1, (product, entry) => product * entry);
	}

	public static bool FindSumsEqualTo(int value, IEnumerable<int> expenseValues, int noOfEntries, out List<int> foundEntries) {
		foundEntries = new();

		foreach (int entry in expenseValues) {
			if (noOfEntries > 2) {
				List<int> x = new List<int> { entry };
				if (FindSumsEqualTo(value - entry, expenseValues.Except(x).ToList(), noOfEntries - 1, out List<int> resultT)) {
					foundEntries.Add(entry);
					foundEntries.AddRange(resultT);
					return true;
				}
			} else {
				int matchValue = expenseValues.Where(e => e == value - entry).FirstOrDefault();
				if (matchValue != 0) {
					foundEntries.Add(entry);
					foundEntries.Add(matchValue);
					return true;
				}
			}
		}

		return false;
	}
}
