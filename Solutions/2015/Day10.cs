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
	/// Day 10: Title
	/// https://adventofcode.com/2015/day/10
	/// </summary>
	public class Day10 {

		private static long Solution1(string[] input) {
			string inputString = input[0];

			string newString = LookAndSay2(inputString);
			for (int i = 0; i < 39; i++) {
				newString = LookAndSay2(newString);
			}
			return newString.Length;
		}

		private static string LookAndSay(string inputString) {
			char current = inputString[0];
			string consecutiveString = "";
			long count = 0;
			string newString = "";
			foreach (char c in inputString) {
				if (current == c) {
					consecutiveString += c;
					count++;
				} else {
					if (string.IsNullOrEmpty(consecutiveString)) {
						newString += $"1{consecutiveString[0]}";
					} else {
						newString += $"{consecutiveString.Length}{consecutiveString[0]}";
					}
					consecutiveString = c.ToString();
					current = c;
					count = 0;
				}
			} 
			if (!string.IsNullOrEmpty(consecutiveString)) {
				newString += $"{consecutiveString.Length}{consecutiveString[0]}";
			}
			return newString;
		}
		private static long Solution2(string[] input) {
			string inputString = input[0];

			string newString = LookAndSay2(inputString);
			for (int i = 0; i < 49; i++) {
				newString = LookAndSay2(newString);
			}
			return newString.Length;
		}
		private static string LookAndSay2(string inputString) {
			char current = inputString[0];
			char c;
			StringBuilder sb = new();
			int jump = 0;
			for (int index = 0; index < inputString.Length; index += jump) {
				c = inputString[index];
				if (index + 3 <= inputString.Length && c == inputString[index + 1] && c == inputString[index + 2]) {
					sb.Append('3');
					sb.Append(c);
					jump = 3;
				} else if (index + 2 <= inputString.Length && c == inputString[index + 1]) {
					sb.Append('2');
					sb.Append(c);
					jump = 2;
				} else if (index + 1<= inputString.Length) {
					sb.Append('1');
					sb.Append(c);
					jump = 1;
				}
			}
			return sb.ToString();
		}







		#region Problem initialisation
		/// <summary>
		/// Sets up the inputs for Part1 of the problem and calls Solution1
		/// </summary>
		/// <param name="input"></param>
		/// Array of strings
		/// <param name="args"></param>
		/// Optional extra parameters that may be required as input to the problem
		/// <returns></returns>
		public static string Part1(string[]? input, params object[]? args) {
			if (input is null) { return "Error: No data provided"; }
			input = input.StripTrailingBlankLineOrDefault();
			return Solution1(input).ToString();
		}
		/// <summary>
		/// Sets up the inputs for Part2 of the problem and calls Solution2
		/// </summary>
		/// <param name="input"></param>
		/// Array of strings
		/// <param name="args"></param>
		/// Optional extra parameters that may be required as input to the problem
		/// <returns></returns>
		public static string Part2(string[]? input, params object[]? args) {
			if (input is null) { return "Error: No data provided"; }
			input = input.StripTrailingBlankLineOrDefault();
			return Solution2(input).ToString();
		}
		#endregion

	}
}
