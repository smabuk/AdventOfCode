using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

using AdventOfCode.Solutions.Helpers;

namespace AdventOfCode.Solutions {
	/// <summary>
	/// Day 07: Handy Haversacks
	/// https://adventofcode.com/2020/day/7
	/// </summary>
	public class Solution_2020_07 {

		private static int Solution1(string[] input) {
			List<string> inputs = input.ToList();
			Dictionary<string, List<BagRuleDetail>?> rules = new();
			List<string> foundContainers = new();
			foreach (string rule in inputs) {

				string bag = rule[..rule.IndexOf(" bag")];
				if (rule.Contains("no other")) {
					rules.Add(bag, null);
					continue;
				}
				rules.Add(bag, GetBagRuleDetail(rule));
			}

			List<string> shinyGoldBagContainers = new();

			int count = 0;
			foreach (var item in rules) {
				string bag = item.Key;

				bool found = FindShinyGoldBag(bag, item.Value, rules, foundContainers);
				if (found) {
					count++;
				}

			}

			return foundContainers.Distinct().Count();
		}

		private static long Solution2(string[] input) {
			List<string> inputs = input.ToList();
			Dictionary<string, List<BagRuleDetail>?> rules = new();
			List<string> foundContainers = new();
			foreach (string rule in inputs) {
				string bag = rule[..rule.IndexOf(" bag")];
				if (rule.Contains("no other")) {
					rules.Add(bag, null);
					continue;
				}
				rules.Add(bag, GetBagRuleDetail(rule));
			}

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
					found =  true;
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
				long i = (CountBags(rules[innerbag.Bag], rules));
				if (i != 0) {
					noOfBags += (innerbag.Number * i);
				}
			}
			return noOfBags;
		}

		public static List<BagRuleDetail> GetBagRuleDetail(string rule) {

			List<int> numbers = new();
			Match match = Regex.Match(rule, @"\d+");
			_ = int.TryParse(match.Value, out int number);
			numbers.Add(number);
			while (match.Success) {
				match = match.NextMatch();
				_ = int.TryParse(match.Value, out number);
				if (match.Success) {
					numbers.Add(number);
				}
			}

			string rule1 = rule.Replace(" bags contain ", "|")
				.Replace(" bags, ", "|")
				.Replace(" bag, ", "|")
				.Replace(" bags.", "")
				.Replace(" bag.", "");

			string rule2 = new Regex(@"\d+ ").Replace(rule1, "");
			string[] bags = rule2.Split("|");
			List<BagRuleDetail> brds = new();
			int i = 0;
			foreach (string? bagname in bags[1..]) {
				brds.Add(new BagRuleDetail(bagname, numbers[i++]));
			}
			return brds;
		}



		public static string Part1(string[]? input) {
			if (input is null) { return "Error: No data provided"; }
			input = input.StripTrailingBlankLineOrDefault();
			return Solution1(input).ToString();
		}

		public static string Part2(string[]? input) {
			if (input is null) { return "Error: No data provided"; }
			input = input.StripTrailingBlankLineOrDefault();
			return Solution2(input).ToString();
		}

	}
	public record BagRuleDetail(string Bag, int Number);
	public record BagRules(string Bag, List<BagRuleDetail> Rules );


}
