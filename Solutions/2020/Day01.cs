namespace AdventOfCode.Solutions.Year2020;

/// <summary>
/// https://adventofcode.com/2020/day/1
/// </summary>
public static class Day01 {
	public static long Part1(string[]? input = null) {
		input = input.StripTrailingBlankLineOrDefault();
		List<int> expenseValues = new();
		foreach (string item in input) {
			expenseValues.Add(int.Parse(item));
		}
		FindSumsEqualTo(2020, expenseValues, 2, out List<int> entries);
		return entries.Aggregate(1, (product, entry) => product * entry);
	}

	public static long Part2(string[]? input = null) {
		input = input.StripTrailingBlankLineOrDefault();
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
