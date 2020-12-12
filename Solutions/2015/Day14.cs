using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AdventOfCode.Solutions.Helpers;

using static AdventOfCode.Solutions.Helpers.ArgumentHelpers;

namespace AdventOfCode.Solutions.Year2015 {
	/// <summary>
	/// Day 14: Reindeer Olympics
	/// https://adventofcode.com/2015/day/14
	/// </summary>
	public class Day14 {

		record Reindeer(string Name, int Speed, int FlyingTime, int RestingTime) {
			public bool IsFlying(int noOfSeconds) =>
				noOfSeconds % (FlyingTime + RestingTime) < FlyingTime;
			public bool IsResting(int noOfSeconds) =>
				!IsFlying(noOfSeconds);
		};

		private static int Solution1(string[] input, int raceTime) {
			List<Reindeer> reindeer = input.Select(i => ParseLine(i)).ToList();
			Dictionary<string, int> distances = new();
			foreach (Reindeer r in reindeer) {
				distances.Add(r.Name, 0);
			}

			for (int i = 0; i < raceTime; i++) {
				foreach (Reindeer r in reindeer) {
					if (r.IsFlying(i)) {
						distances[r.Name] += r.Speed;
					}
				}
			}

			return distances.Values.Max();
		}

		private static string Solution2(string[] input) {
			//string inputLine = input[0];
			List<string> inputs = input.ToList();
			//inputs.Add("");
			return "** Solution not written yet **";
		}

		private static Reindeer ParseLine(string input) {
			Match match = Regex.Match(input, @"(\w+) can fly (\d+) km/s for (\d+) second[s]*, but then must rest for (\d+) second[s]*.");
			if (match.Success) {
				return new Reindeer(
					match.Groups[1].Value,
					int.Parse(match.Groups[2].Value),
					int.Parse(match.Groups[3].Value),
					int.Parse(match.Groups[4].Value)
					);
			}
			return null!;
		}






		#region Problem initialisation
		public static string Part1(string[]? input, params object[]? args) {
			if (input is null) { return "Error: No data provided"; }
			int raceTime = GetArgument(args, 1, 2503);
			input = input.StripTrailingBlankLineOrDefault();
			return Solution1(input, raceTime).ToString();
		}
		public static string Part2(string[]? input, params object[]? args) {
			if (input is null) { return "Error: No data provided"; }
			// int arg1 = GetArgument(args, 1, 25);
			input = input.StripTrailingBlankLineOrDefault();
			return "** Solution not written yet **";
			return Solution2(input).ToString();
		}
		#endregion

	}
}
