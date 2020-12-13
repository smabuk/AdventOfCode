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
	/// Day 13: Shuttle Search
	/// https://adventofcode.com/2020/day/13
	/// </summary>
	public class Day13 {

		record RecordType(string Name, int Value);

		private static long Solution1(string[] input) {
			int arrivalTime = int.Parse(input[0]);
			int[] buses = input[1]
				.Split(',')
				.Where(b =>b != "x")
				.Select(i => int.Parse(i))
				.ToArray();

			int busiD = 0;

			int currentTime = arrivalTime;
			bool lookingForBus = true;
			do {
				busiD = buses.Where(b => (currentTime % b) == 0).SingleOrDefault();
				if (busiD != 0) {
					lookingForBus = false;
					break;
				}
				currentTime++;
			} while (lookingForBus);


			int timeToWait = currentTime - arrivalTime;
			return timeToWait * busiD;
		}

		private static long Solution2(string[] input) {
			int[] buses = input[0]
				.Split(',')
				.Select(i => i == "x" ? 1 : int.Parse(i))
				.ToArray();

			int noOfBuses = buses.Length;
			long step = buses[0];
			//for (int i = 0; i < buses.Length; i++) {
			//	int bus = buses[i];
			//	step *= (buses[i] - i);
			//}
			long currentTime = step;
			while (Enumerable.Range(0, noOfBuses).Any(x => (currentTime + x) % (buses[x]) != 0)) {
				currentTime += step;
			}

			return currentTime;
		}



		#region Problem initialisation
		public static string Part1(string[]? input, params object[]? args) {
			if (input is null) { return "Error: No data provided"; }
			// int arg1 = GetArgument(args, 1, 25);
			input = input.StripTrailingBlankLineOrDefault();
			return Solution1(input).ToString();
		}
		public static string Part2(string[]? input, params object[]? args) {
			if (input is null) { return "Error: No data provided"; }
			// int arg1 = GetArgument(args, 1, 25);
			input = input.StripTrailingBlankLineOrDefault();
			return Solution2(input).ToString();
		}
		#endregion

	}
}
