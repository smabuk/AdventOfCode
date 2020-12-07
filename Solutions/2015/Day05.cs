using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AdventOfCode.Solutions.Helpers;

namespace AdventOfCode.Solutions {
	/// <summary>
	/// Day 05: Doesn't He Have Intern-Elves For This?
	/// https://adventofcode.com/2015/day/5
	/// </summary>
	public class Solution_2015_05 {
		private static int Solution1(string[] input) {
			return input.ToList().Count(i => Nice_Part1(i));
		}

		private static int Solution2(string[] input) {
			return input.ToList().Count(i => Nice_Part2(i));
		}

		public static bool Nice_Part1(string item) {
			bool naughty = item switch {
				_ when item.Contains("ab") => true,
				_ when item.Contains("cd") => true,
				_ when item.Contains("pq") => true,
				_ when item.Contains("xy") => true,
				_ when item.Where(c => "aeiou".Contains(c)).Count() < 3 => true,
				_ => false
			};
			if (!naughty) {
				naughty = true;
				for (int i = 0; i < item.Length - 1; i++) {
					if (item[i] == item[i + 1]) {
						naughty = false;
						break;
					}
				}
			}
			return !naughty;
		}

		public static bool Nice_Part2(string item) {
			bool nice = false;
			for (int i = 0; i < item.Length - 2; i++) {
				if (item[i] == item[i + 2]) {
					nice = true;
					break;
				}
			}
			if (nice is false) {
				return false;
			}
			nice = false;
			for (int i = 0; i < item.Length - 2; i++) {
				if (item[(i+2)..].Contains(item[i..(i+2)])) {
					nice = true;
					break;
				}
			}
			if (nice is false) {
				return false;
			}

			return nice;
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
}
