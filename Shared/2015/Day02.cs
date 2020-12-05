﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Shared {
	public class Solution_2015_02 {
		public static string Part1(string[]? input) {
			if (input is null) { return "Error: No data provided"; }
			return Part1_Solution(input).ToString();
		}

		public static string Part2(string[]? input) {
			if (input is null) { return "Error: No data provided"; }
			return Part2_Solution(input).ToString();
		}



		private static int Part1_Solution(string[] input) {
			int total = 0;
			foreach (string line in input) {
				string[] parts = line.Split('x');
				_ = int.TryParse(parts[0], out int l);
				_ = int.TryParse(parts[1], out int w);
				_ = int.TryParse(parts[2], out int h);
				total += CalculateWrappingPaperArea(l, w, h);
			}
			return total;
		}

		private static int CalculateWrappingPaperArea(int l, int w, int h) {
			int lw = 2 * l * w;
			int wh = 2 * w * h;
			int hl = 2 * h * l;
			int area = lw + wh + hl + (Math.Min(lw, Math.Min(wh, hl)) / 2);
			return area;
		}

		private static int Part2_Solution(string[] input) {
			int total = 0;
			foreach (string line in input) {
				string[] parts = line.Split('x');
				_ = int.TryParse(parts[0], out int l);
				_ = int.TryParse(parts[1], out int w);
				_ = int.TryParse(parts[2], out int h);
				total += CalculateRibbonLength(l, w, h);
			}
			return total;
		}

		private static int CalculateRibbonLength(int l, int w, int h) {
			int maxSide = Math.Max(l, Math.Max(w, h));
			int length = ((l + w + h - maxSide) * 2) + (l * w * h);
			return length;
		}
	}
}
