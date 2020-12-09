using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AdventOfCode.Solutions.Helpers;

namespace AdventOfCode.Solutions.Year2020 {
	/// <summary>
	/// Day 09: Encoding Error
	/// https://adventofcode.com/2020/day/9
	/// </summary>
	public class Day09 {

		private static long Solution1(string[] input) {
			List<string> inputs = input.ToList();

			int PreambleNo = 25;
			long[] codes = inputs.Select(x => long.Parse(x)).ToArray();

			int i = 0;
			long[] skipped = codes.Skip(PreambleNo).ToArray();
			foreach (long code in skipped) {
				if (!ValidXmas(code, codes[i..(i+PreambleNo)])) {
					return code; 
				}
				i++;
			}

			return 0;

		}

		private static bool ValidXmas(long code, IEnumerable<long> values) {
			foreach (long i in values) {
				foreach (long j in values) {
					if (code == (i + j) && i != j) {
						return true;
					}
				}
			}
			return false;
		}

		private static long Solution2(string[] input) {
			List<string> inputs = input.ToList();

			int PreambleNo = 25;
			long[] codes = inputs.Select(x => long.Parse(x)).ToArray();

			int i = 0;
			long[] skipped = codes.Skip(PreambleNo).ToArray();
			foreach (long code in skipped) {
				if (!ValidXmas(code, codes[i..(i + PreambleNo)])) {
					return code;
				}
				i++;
			}

			return 0;
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
