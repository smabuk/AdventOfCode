namespace AdventOfCode.Solutions._2015;

/// <summary>
/// Day 13: Knights of the Dinner Table
/// https://adventofcode.com/2015/day/13
/// </summary>
[Description("Knights of the Dinner Table")]
public class Day13 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		Dictionary<(string, string), int> lookup = new();
		List<KeyValuePair<(string, string), int>>? inputs = input.Select(i => ParseLine(i)).ToList();
		List<string> guests = new();

		foreach (var item in inputs) {
			if (guests.Contains(item.Key.Item1) == false) {
				guests.Add(item.Key.Item1);
			}
			lookup.Add(item.Key, item.Value);
		}
		int sum = guests.Permute().Select(t => CalculateHappinessIncrease(t, lookup)).Max();

		return sum;
	}

	private static int Solution2(string[] input) {
		Dictionary<(string, string), int> lookup = new();
		List<KeyValuePair<(string, string), int>>? inputs = input.Select(i => ParseLine(i)).ToList();
		List<string> guests = new();

		foreach (var item in inputs) {
			if (guests.Contains(item.Key.Item1) == false) {
				guests.Add(item.Key.Item1);
			}
			lookup.Add(item.Key, item.Value);
		}

		string myName = "ME";
		foreach (string guest in guests) {
			lookup.Add((guest, myName), 0);
			lookup.Add((myName, guest), 0);
		}
		guests.Add(myName);

		int sum = guests.Permute().Select(t => CalculateHappinessIncrease(t, lookup)).Max();

		return sum;
	}

	public static int CalculateHappinessIncrease(IEnumerable<string> table, Dictionary<(string, string), int> lookup) {
		int sum = lookup[(table.Last(), table.First())];
		sum += table.Zip(table.Skip(1), (g1, g2) => (g1, g2)).Sum(g => lookup[g]);

		IEnumerable<string> reverseTable = table.Reverse();
		sum += lookup[(reverseTable.Last(), reverseTable.First())];
		sum += reverseTable.Zip(reverseTable.Skip(1), (g1, g2) => (g1, g2)).Sum(g => lookup[g]);

		return sum;
	}

	private static KeyValuePair<(string, string), int> ParseLine(string input) {
		Match match = Regex.Match(input, @"(\w+) would (gain|lose) (\d+) happiness units by sitting next to (\w+)");
		if (match.Success) {
			int hapUnits = match.Groups[2].Value == "gain"
				? int.Parse(match.Groups[3].Value)
				: int.Parse($"-{match.Groups[3].Value}");
			return new KeyValuePair<(string, string), int>((
				match.Groups[1].Value, match.Groups[4].Value), hapUnits);
		}
		return new KeyValuePair<(string, string), int>();
	}
}
