using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AdventOfCode.Solutions.Helpers;

using static AdventOfCode.Solutions.Helpers.ArgumentHelpers;

namespace AdventOfCode.Solutions.Year2020 {
	/// <summary>
	/// Day 12: Rain Risk
	/// https://adventofcode.com/2020/day/12
	/// </summary>
	public class Day12 {

		record Instruction(string Command, int Value);
		

		private static int Solution1(string[] input) {
			IEnumerable<Instruction> instructions = input.Select(i => ParseLine(i));

			int dX = 1; //East
			int dY = 0;
			int x = 0;
			int y = 0;
			foreach (Instruction? instruction in instructions) {
				switch (instruction.Command) {
					case "N":
						y += instruction.Value;
						break;
					case "S":
						y -= instruction.Value;
						break;
					case "E":
						x += instruction.Value;
						break;
					case "W":
						x -= instruction.Value;
						break;
					case "L":
					case "R":
						(dX, dY) = ChangeDirection((dX, dY), instruction.Command, instruction.Value);
						break;
					case "F":
						x += instruction.Value * dX;
						y += instruction.Value * dY;
						break;
					default:
						break;
				}
			}

			return Math.Abs(x) + Math.Abs(y);
		}

		static (int, int) ChangeDirection((int dX, int dY) current, string direction, int Value) {
			(int, int)[] VECTORS = {	(1, 0), (0, -1), (-1, 0), (0, 1),
										(1, 0), (0, -1), (-1, 0), (0, 1),
										(1, 0), (0, -1), (-1, 0), (0, 1)  };
			int move = (Value / 90) % 4;
			int currentIndex = 0;
			for (int i = 4; i <= 7; i++) {
				if (current == VECTORS[i]) {
					currentIndex = i;
					break;
				}
			}
			return direction switch {
				"L" => VECTORS[currentIndex - move],
				"R" => VECTORS[currentIndex + move],
				_ => (-999, -999)
			};
		}



		private static string Solution2(string[] input) {
			List<string> inputs = input.ToList();
			return "** Solution not written yet **";
		}

		private static Instruction ParseLine(string input) {
			Match match = Regex.Match(input, @"([NSEWLRF])(\d+)");
			if (match.Success) {
				return new(match.Groups[1].Value, int.Parse(match.Groups[2].Value));
			}
			return null!;
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
