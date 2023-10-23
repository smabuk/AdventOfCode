namespace AdventOfCode.Solutions._2020;

/// <summary>
/// Day 07: Handy Haversacks
/// https://adventofcode.com/2020/day/7
/// </summary>
[Description("Handy Haversacks")]
public class Day07 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	public record BagRuleDetail(string Bag, int Number);
	public record BagRules(string Bag, List<BagRuleDetail> Rules);

	private static int Solution1(string[] input) {
		List<string> inputs = input.ToList();

		Dictionary<string, List<BagRuleDetail>?> rules = GetAllBagRules(inputs);

		List<string> foundContainers = [];
		foreach (var item in rules) {
			_ = FindShinyGoldBag(item.Key, item.Value, rules, foundContainers);
		}

		return foundContainers.Distinct().Count();
	}

	private static long Solution2(string[] input) {
		List<string> inputs = input.ToList();

		Dictionary<string, List<BagRuleDetail>?> rules = GetAllBagRules(inputs);

		return CountBags(rules["shiny gold"], rules) - 1;
	}

	private static bool FindShinyGoldBag(string bag, List<BagRuleDetail>? bagrules, Dictionary<string, List<BagRuleDetail>?> rules, List<string> foundContainers) {
		if (bagrules is null) {
			return false;
		}

		bool found = false;
		foreach (var innerbag in bagrules) {
			if (innerbag.Bag == "shiny gold") {
				foundContainers.Add(bag);
				found = true;
			}
			if (FindShinyGoldBag(innerbag.Bag, rules[innerbag.Bag], rules, foundContainers)) {
				foundContainers.Add(bag);
				found = true;
			}
		}
		return found;
	}

	private static long CountBags(List<BagRuleDetail>? bagrules, Dictionary<string, List<BagRuleDetail>?> rules) {
		if (bagrules is null) {
			return 1;
		}

		long noOfBags = 1;
		foreach (var innerbag in bagrules) {
			long i = CountBags(rules[innerbag.Bag], rules);
			if (i != 0) {
				noOfBags += innerbag.Number * i;
			}
		}
		return noOfBags;
	}

	public static Dictionary<string, List<BagRuleDetail>?> GetAllBagRules(IEnumerable<string> inputs) {
		Dictionary<string, List<BagRuleDetail>?> rules = [];

		foreach (string rule in inputs) {
			string bag = rule[..rule.IndexOf(" bag")];
			if (rule.Contains("no other")) {
				rules.Add(bag, null);
				continue;
			}
			rules.Add(bag, GetBagRuleDetail(rule));
		}
		return rules;
	}

	public static List<BagRuleDetail> GetBagRuleDetail(string rule) {
		MatchCollection? bagDetailRules = Regex.Matches(rule, @"(\d +)([a-z]+ [a-z]+) bag");
		return bagDetailRules.Select(brd => new BagRuleDetail(brd.Groups[2].Value, int.Parse(brd.Groups[1].Value))).ToList();
	}
}
