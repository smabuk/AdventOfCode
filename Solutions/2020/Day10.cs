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
	/// Day 10: Adapter Array
	/// https://adventofcode.com/2020/day/10
	/// </summary>
	public class Day10 {

		private static int Solution1(string[] input) {
			List<int> inputs = input.Select(i => int.Parse(i)).ToList();
			inputs.Sort();
			int start = 0;
			int count1Jolts = 0;
			int count2Jolts = 0;
			int count3Jolts = 0;

			foreach (int item in inputs) {
				if (item - start == 3) {
					count3Jolts++;
				} else if (item - start == 2) {
					count2Jolts++;
				} else if (item - start == 1) {
					count1Jolts++;
				}
				start = item;
			}
			return count1Jolts * (count3Jolts + 1);
		}

		private static long Solution2(string[] input) {
			List<int> inputs = input.Select(i => int.Parse(i)).ToList();
			inputs.Sort();
			int outlet = 0;
			int device = inputs.Last() + 3;
			inputs = inputs.Prepend(outlet).Append(device).ToList();
			List<(int Adapter, int Count)> adapters = new();

			for (int i = 0; i < inputs.Count - 1; i++) {
				int item = inputs[i];
				int nearby = inputs.Skip(i + 1)
					.Take(3)
					.Where(a => a <= item + 3).Count();
				adapters.Add((item, nearby));
			}

			long total = 1;
			long multiCount = 0;
			(int Adapter, int Count) prev = adapters[0];
			for (int i = 0; i < adapters.Count - 1; i++) {
				if (adapters[i].Count == 1 && multiCount > 1) {
					if (multiCount ==2 ) {
						multiCount++;
					}
					total *= (multiCount - 1);
					multiCount = 0;
				} else if (adapters[i].Count > 1) {
					multiCount += adapters[i].Count;
				}
				prev = adapters[i];
			}

			return total;
			;
		}


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
