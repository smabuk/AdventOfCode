using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AdventOfCode.Solutions.Helpers;

using static AdventOfCode.Solutions.Helpers.ArgumentHelpers;

namespace AdventOfCode.Solutions.Year2020 {
	/// <summary>
	/// Day XX: Title
	/// https://adventofcode.com/2020/day/XX
	/// </summary>
	public class DayXX {

		private static string Solution1(string[] input) {
			//string inputLine = input[0];
			List<string> inputs = input.ToList();
			//inputs.Add("");
			return "** Solution not written yet **";
		}

		private static string Solution2(string[] input) {
			//string inputLine = input[0];
			List<string> inputs = input.ToList();
			//inputs.Add("");
			return "** Solution not written yet **";
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
		public static string Part1(string[]? input, params object[]? args) {
			if (input is null) { return "Error: No data provided"; }
			// int arg1 = GetArgument(args, 1, 25);
			input = input.StripTrailingBlankLineOrDefault();
			return "** Solution not written yet **";
			return Solution1(input).ToString();
		}
		public static string Part2(string[]? input, params object[]? args) {
			if (input is null) { return "Error: No data provided"; }
			// int arg1 = GetArgument(args, 1, 25);
			input = input.StripTrailingBlankLineOrDefault();
			return "** Solution not written yet **";
			return Solution2(input).ToString();
		}
		#endregion

	}
}
