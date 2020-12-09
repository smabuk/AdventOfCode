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

		private static long Solution1(string[] input, int preamble) {
			List<string> inputs = input.ToList();

			long[] codes = inputs.Select(x => long.Parse(x)).ToArray();

			int i = 0;
			long[] skipped = codes.Skip(preamble).ToArray();
			foreach (long code in skipped) {
				if (!ValidXmas(code, codes[i..(i+preamble)])) {
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

		private static long Solution2(string[] input, int preamble) {
			List<string> inputs = input.ToList();

			long[] codes = inputs.Select(x => long.Parse(x)).ToArray();

			int foundPos = 0;
			int i = 0;
			long[] skipped = codes.Skip(preamble).ToArray();
			foreach (long code in skipped) {
				if (!ValidXmas(code, codes[i..(i + preamble)])) {
					foundPos = i + preamble;
				}
				i++;
			}

			return ValidSumHighPlusLow(codes[foundPos], codes);
		}

		private static long ValidSumHighPlusLow(long code, long[] values) {
			for (int i = 0; i <= values.Count(); i++) {
				for (int j = i + 1; j <= values.Count(); j++) {
					if (values[i..j].Sum() == code) {
						return values[i..j].LowestValue() + values[i..j].HighestValue();
					}
				}
			}
			return 0;
		}



		public static string Part1(string[]? input, params object[]? args) {
			if (input is null) { return "Error: No data provided"; }
			input = input.StripTrailingBlankLineOrDefault();
			int preamble = 0;
			if (args is null ) {
				preamble = 25;
			} else if (args.Length == 1 && args[0] is int x) {
				preamble = x;
			} else {
				preamble = 25;
			}
			return Solution1(input, preamble).ToString();
		}
		public static string Part2(string[]? input, params object[]? args) {
			if (input is null) { return "Error: No data provided"; }
			input = input.StripTrailingBlankLineOrDefault();
			int preamble = 0;
			if (args is null) {
				preamble = 25;
			} else if (args.Length == 1 && args[0] is int x) {
				preamble = x;
			} else {
				preamble = 25;
			}
			return Solution2(input, preamble).ToString();
		}
	}
}
