using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AdventOfCode.Solutions.Helpers;

using static AdventOfCode.Solutions.Helpers.ArgumentHelpers;

namespace AdventOfCode.Solutions.Year2015 {
	/// <summary>
	/// Day 19: Medicine for Rudolph
	/// https://adventofcode.com/2015/day/19
	/// </summary>
	public class Day19 {

		record Replacement(string From, string To);

		private static int Solution1(string[] input) {
			List<Replacement> replacements = input[..^2].Select(i => ParseLine(i)).ToList();
			string startingMolecule = input[^1];

			List<string> molecules = new();

			foreach (var r in replacements) {
				int i = 0;
				do {
					i = startingMolecule.IndexOf(r.From, i);
					if (i >= 0) {
						molecules.Add($"{startingMolecule[..i]}{r.To}{startingMolecule[(i + r.From.Length)..]}");
						i++;
					}
				} while (i >=0);
			}

			return molecules.Distinct().Count();
		}

		private static string Solution2(string[] input) {
			//string inputLine = input[0];
			//List<string> inputs = input.ToList();
			List<Replacement> instructions = input.Select(i => ParseLine(i)).ToList();
			return "** Solution not written yet **";
		}

		private static Replacement ParseLine(string input) {
			Match match = Regex.Match(input, @"(?<from>\w+) => (?<to>\w+)");
			if (match.Success) {
				return new(match.Groups["from"].Value, match.Groups["to"].Value);
			}
			return null!;
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
}
