using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using AdventOfCode.Solutions.Helpers;

namespace AdventOfCode.Solutions {
	/// <summary>
	/// Day 04: The Ideal Stocking Stuffer
	/// https://adventofcode.com/2015/day/4
	/// </summary>
	public class Solution_2015_04 {
		private static long Solution1(string[] input) {
			string secretKey = input[0];

			long i;
			for (i = 0; i < 999999999999; i++) {
				string hashSource = $"{secretKey}{i}";
				byte[] bytes = Encoding.UTF8.GetBytes(hashSource);
				byte[] hashBytes = MD5.HashData(bytes);
				string hex = BitConverter.ToString(hashBytes).Replace("-", "");
				if (hex.StartsWith("00000")) {
					break;
				}
			}
			return i;
		}

		private static long Solution2(string[] input) {
			string secretKey = input[0];

			long i;
			for (i = 0; i < 9999999999; i++) {
				string hashSource = $"{secretKey}{i}";
				byte[] bytes = Encoding.UTF8.GetBytes(hashSource);
				byte[] hashBytes = MD5.HashData(bytes);
				string hex = BitConverter.ToString(hashBytes).Replace("-", "");
				if (hex.StartsWith("000000")) {
					break;
				}
			}
			return i;
		}



		public static string Part1(string[]? input) {
			input = input.StripTrailingBlankLineOrDefault();
			return Solution1(input).ToString();
		}

		public static string Part2(string[]? input) {
			input = input.StripTrailingBlankLineOrDefault();
			return Solution2(input).ToString();
		}

	}
}
