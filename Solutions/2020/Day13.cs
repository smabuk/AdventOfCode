using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		record Bus(string BusNo, int Value, int Offset);

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
			Bus[] buses = input[1]
				.Split(',')
				.Select((busNo, i) => new Bus(busNo, 9999,  i))
				.Where(bus =>bus.BusNo != "x")
				.Select(bus => bus with { Value = int.Parse(bus.BusNo) })
				.ToArray();

			long step = buses[0].Value;
			long time = step;
			int iteration = 1;
			do {
				time += step;
				if(Enumerable.Range(0, iteration + 1).All(x => (time + buses[x].Offset) % (buses[x].Value) == 0)) {
					step *= buses[iteration].Value;
					iteration++;
				}
			} while (iteration < buses.Length);

			return time;
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
