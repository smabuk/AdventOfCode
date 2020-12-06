using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AdventOfCode.Solutions.Helpers;

namespace AdventOfCode.Solutions {
	/// <summary>
	/// Day XX: Title
	/// https://adventofcode.com/2015/day/XX
	/// </summary>
	public class Solution_2015_xx {
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

		private static string Solution1(string[] input) {
			List<string> inputs = input.ToList();
			//inputs.Add("");
			return "Error: No solution yet";
		}

		private static string Solution2(string[] input) {
			List<string> inputs = input.ToList();
			inputs.Add("");
			return "Error: No solution yet";
		}

	}
}
