using System.Text;

namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 14: Extended Polymerization
/// https://adventofcode.com/2021/day/14
/// </summary>
[Description("Extended Polymerization")]
public class Day14 {

	public static string Part1(string[] input, params object[]? args) {
		int steps = GetArgument(args, argumentNumber: 1, defaultResult: 10);
		return Solution1(input, steps).ToString();
	}
	public static string Part2(string[] input, params object[]? args) {
		int steps = GetArgument(args, argumentNumber: 1, defaultResult: 40);
		return Solution2(input, steps).ToString();
	}

	record PairInsertionRule(string Pair, char Value) {
		public string First => $"{Pair[0]}{Value}";
		public string Second => $"{Value}{Pair[1]}";
	};

	private static int Solution1(string[] input, int steps) {
		string polymerTemplate = input[0];
		Dictionary<string, PairInsertionRule> rules = input[2..]
			.Select(i => ParseLine(i))
			.ToDictionary(r => r.Pair, r => r);

		string polymer = polymerTemplate;
		for (int step = 0; step < steps; step++) {
			polymer = ProcessPairs(polymer, rules);
		}

		int max = polymer
			.GroupBy(p => p)
			.Select(g => new { Polymer = g, Count = g.Count() })
			.MaxBy(p => p.Count)!
			.Count;

		int min = polymer
			.GroupBy(p => p)
			.Select(g => new { Polymer = g, Count = g.Count() })
			.MinBy(p => p.Count)!
			.Count;

		return max - min;
	}

	private static long Solution2(string[] input, int steps) {
		string polymerTemplate = input[0];
		Dictionary<string, PairInsertionRule> rules = input[2..]
			.Select(i => ParseLine(i))
			.ToDictionary(r => r.Pair, r => r);

		Dictionary<char, long> polymers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
			.Select(i => i)
			.ToDictionary(x => x, x => 0L);

		Dictionary<string, long> polymerPairs = input[2..]
			.Select(i => ParseLine(i))
			.ToDictionary(r => r.Pair, r => 0L);

		for (int i = 0; i < polymerTemplate.Length; i++) {
			polymers[polymerTemplate[i]]++;
			if (i + 1 < polymerTemplate.Length && rules.TryGetValue(polymerTemplate[i..(i + 2)], out PairInsertionRule? rule)) {
				polymerPairs[rule.Pair]++;
			}
		}

		string polymer = polymerTemplate;
		for (int step = 0; step < steps; step++) {
			Dictionary<string, long> newPolymerPairs = new(polymerPairs);
			foreach (KeyValuePair<string, PairInsertionRule> ruleKVP in rules) {
				var rule = ruleKVP.Value;
				if (polymerPairs.TryGetValue(rule.Pair, out long count)) {
					newPolymerPairs[rule.Pair] = newPolymerPairs[rule.Pair] - count;
					newPolymerPairs[rule.First] = newPolymerPairs[rule.First] + count;
					newPolymerPairs[rule.Second] = newPolymerPairs[rule.Second] + count;
					polymers[rule.Value] = polymers[rule.Value] + count;
				}
			}
			polymerPairs = new(newPolymerPairs);
		}

		long max = polymers
			.MaxBy(p => p.Value)
			.Value;

		long min = polymers
			.Where(p => p.Value > 0)
			.MinBy(p => p.Value)
			.Value;

		return max - min;
	}

	private static string ProcessPairs(string polymer, Dictionary<string, PairInsertionRule> rules) {
		StringBuilder newPolymer = new();

		for (int i = 0; i < polymer.Length; i++) {
			newPolymer.Append(polymer[i]);
			if (i + 1 < polymer.Length && rules.TryGetValue(polymer[i..(i+2)], out PairInsertionRule? rule)) {
				newPolymer.Append(rule.Value);
			}
		}

		return newPolymer.ToString();
	}

	private static PairInsertionRule ParseLine(string input) {
		Match match = Regex.Match(input, @"(\w*) -> (\w)");
		if (match.Success) {
			return new(match.Groups[1].Value, match.Groups[2].Value[0]);
		}
		return null!;
	}
}
