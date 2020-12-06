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
			input ??= _inputLines;
			input = input.StripTrailingBlankLineOrDefault();
			return SolutionPart1(input).ToString();
		}

		public static string Part2(string[]? input) {
			if (input is null) { return "Error: No data provided"; }
			input ??= _inputLines;
			input = input.StripTrailingBlankLineOrDefault();
			return SolutionPart2(input).ToString();
		}

		private static string SolutionPart1(string[] input) {
			return "Error: No solution yet";
		}

		private static string SolutionPart2(string[] input) {
			return "Error: No solution yet";
		}

		static readonly string[] _inputLines = new string[] {
			""
		};
	}
}
