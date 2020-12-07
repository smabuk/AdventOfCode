using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AdventOfCode.Solutions.Helpers;

namespace AdventOfCode.Solutions {
	/// <summary>
	/// Day 06: Probably a Fire Hazard
	/// https://adventofcode.com/2015/day/6
	/// </summary>
	public class Solution_2015_06 {

		public static bool[,] Lights = new bool[1000, 1000];
		public static int [,] Lights2 = new int[1000, 1000];

		public record Point(int X, int Y);
		public record Instruction(string Command, Point Point1, Point Point2);


		public static long Solution1(string[] input) {
			List<string> inputs = input.ToList();

			Lights = new bool[1000, 1000];
			foreach (string line in inputs) {
				Instruction instruction = GetInstruction(line);
				ChangeGroup(instruction);
			}
			return Lights.Cast<bool>().Count(l => l);
		}

		public static long Solution2(string[] input) {
			List<string> inputs = input.ToList();

			Lights2 = new int[1000, 1000];
			foreach (string line in inputs) {
				Instruction instruction = GetInstruction(line);
				ChangeGroup2(instruction);
			}
			return Lights2.Cast<int>().Sum();
		}



		public static Instruction GetInstruction(string input) {
			Match? match = Regex.Match(input, @"(turn on|turn off|toggle) (\d+),(\d+) through (\d+),(\d+)");
			if (match.Success) {
				return new(
					match.Groups[1].Value,
					new Point(int.Parse(match.Groups[2].Value), int.Parse( match.Groups[3].Value)),
					new Point(int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value))
				);
			}
			return null!;
		}

		public static void ChangeGroup(Instruction instruction) {
			(int xStart, int xEnd) = (instruction.Point1.X <= instruction.Point2.X) ? (instruction.Point1.X, instruction.Point2.X) : (instruction.Point2.X, instruction.Point1.X);
			(int yStart, int yEnd) = (instruction.Point1.Y <= instruction.Point2.Y) ? (instruction.Point1.Y, instruction.Point2.Y) : (instruction.Point2.Y, instruction.Point1.Y);
			for (int x = xStart; x <= xEnd; x++) {
				for (int y = yStart; y <= yEnd; y++) {
					switch (instruction.Command) {
						case "turn on":
							Lights[x, y] = true;
							break;
						case "turn off":
							Lights[x, y] = false;
							break;
						case "toggle":
							Lights[x, y] = !Lights[x, y];
							break;
						default:
							break;
					}
				}
			}
		}
		public static void ChangeGroup2(Instruction instruction) {
			(int xStart, int xEnd) = (instruction.Point1.X <= instruction.Point2.X) ? (instruction.Point1.X, instruction.Point2.X) : (instruction.Point2.X, instruction.Point1.X);
			(int yStart, int yEnd) = (instruction.Point1.Y <= instruction.Point2.Y) ? (instruction.Point1.Y, instruction.Point2.Y) : (instruction.Point2.Y, instruction.Point1.Y);
			for (int x = xStart; x <= xEnd; x++) {
				for (int y = yStart; y <= yEnd; y++) {
					switch (instruction.Command) {
						case "turn on":
							Lights2[x, y] += 1;
							break;
						case "turn off":
							Lights2[x, y] -= 1;
							break;
						case "toggle":
							Lights2[x, y] += 2;
							break;
						default:
							break;
					}
					if (Lights2[x, y] < 0) {
						Lights2[x, y] = 0;
					}
				}
			}
		}


		public static string Part1(string[]? input) {
			if (input is null) { return "Error: No data provided"; }
			input = input.StripTrailingBlankLineOrDefault();
			return Solution1(input).ToString();
		}
		public static string Part2(string[]? input) {
			if (input is null) { return "Error: No data provided"; }
			input = input.StripTrailingBlankLineOrDefault();
			return Solution2(input).ToString();
		}
	}
}
