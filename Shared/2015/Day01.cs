using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Shared
{
	public class Solution_2015_01 {
		public static string Part1(string[]? input) {
			if (input is null) { return "Error: No data provided"; }
			return Part1_Solution(input).ToString();
		}

		public static string Part2(string[]? input) {
			if (input is null) { return "Error: No data provided"; }
			return Part2_Solution(input).ToString();
		}



		private static int Part1_Solution(string[]? input) {
			string instructions = input?[0] ?? "";
			return instructions.Count(i => i == '(') - instructions.Count(i => i == ')');
		}

		private static int Part2_Solution(string[]? input) {
			string instructions = input?[0] ?? "";
			int charPos = 1;
			int floor = 0;
			foreach (char item in instructions) {
				floor += item switch {
					'(' => 1,
					')' => -1,
					_ => 0
				};
				if (floor == -1) {
					break;
				}
				charPos++;
			}
			return charPos;
		}
	}
}
