﻿using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

using AdventOfCode.Solutions.Helpers;

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

		private static int Solution2(string[] input) {
			using JsonDocument document = JsonDocument.Parse(string.Join("", input));
			return StripRedAndSum(document.RootElement);
		}

		static int StripRedAndSum(JsonElement node) {
			return node.ValueKind switch {
				JsonValueKind.Object => node.EnumerateObject()
					.Where(item =>
						item.Value.ValueKind == JsonValueKind.String
						&& item.Value.GetString() == "red").Any()
							? 0 :
						node.EnumerateObject()
					.Sum(item => StripRedAndSum(item.Value)),
				JsonValueKind.Array => node.EnumerateArray()
					.Sum(item => StripRedAndSum(item)),
				JsonValueKind.Number => node.GetInt16(),
				_ => 0,
			};
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
