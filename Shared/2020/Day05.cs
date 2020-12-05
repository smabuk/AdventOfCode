using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Shared
{
    public class Solution_2020_05 {
		public static string Part1(string[]? input) {
			if (input is null) { return "Error: No data provided"; }
			return Solution(input, 1).ToString();
		}

		public static string Part2(string[]? input) {
			if (input is null) { return "Error: No data provided"; }
			return Solution(input, 2).ToString();
		}

		private static string Solution(string[]? input, int v) {
			return "Error: No solution yet";
		}
	}
}
