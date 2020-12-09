﻿using System;
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
			long[] codes = input.Select(x => long.Parse(x)).ToArray();
			int index = FindInvalidCode(codes, preamble);
			return codes[index];
		}

		private static long Solution2(string[] input, int preamble) {
			long[] codes = input.Select(x => long.Parse(x)).ToArray();
			int index = FindInvalidCode(codes, preamble);
			return ValidSumHighPlusLow(codes[index], codes);
		}

		private static int FindInvalidCode(long[] codes, int preamble) {
			int i = 0;
			long[] skipped = codes.Skip(preamble).ToArray();
			foreach (long code in skipped) {
				if (!ValidXmas(code, codes[i..(i + preamble)])) {
					return i + preamble;
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

		private static long ValidSumHighPlusLow(long code, long[] values) {
			for (int i = 0; i <= values.Length; i++) {
				for (int j = i + 1; j <= values.Length; j++) {
					if (values[i..j].Sum() == code) {
						return values[i..j].Min() + values[i..j].Max();
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
