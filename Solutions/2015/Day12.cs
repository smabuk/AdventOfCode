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
	/// Day 12: JSAbacusFramework.io
	/// https://adventofcode.com/2015/day/12
	/// </summary>
	public class Day12 {

		private static int Solution1(string[] input) {
			string inputLine = string.Join("", input);

			int numberSum = 0;
			MatchCollection matches = Regex.Matches(inputLine, @"([\+\-]*\d+)");
			foreach (Match m in matches) {
				numberSum += int.Parse(m.Value);
			}

			return numberSum;
		}

		private static string Solution2(string[] input) {
			List<string> inputs = input.ToList();
			throw new NotImplementedException();
		}

		//private static recordType ParseLine(string input) {
		//	MatchCollection match = Regex.Matches(input, @"(opt1|opt2|opt3) ([\+\-]\d+)");
		//	Match match = Regex.Match(input, @"(opt1|opt2|opt3) ([\+\-]\d+)");
		//	if (match.Success) {
		//		return new(match.Groups[1].Value, int.Parse(match.Groups[2].Value));
		//	}
		//	return null!;
		//}





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
			// int arg1 = GetArgument(args, 1, 25);
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
			// int arg1 = GetArgument(args, 1, 25);
			input = input.StripTrailingBlankLineOrDefault();
			return Solution2(input).ToString();
		}
		#endregion

	}
}
