using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using AdventOfCode.Solutions.Helpers;

using static AdventOfCode.Solutions.Helpers.ArgumentHelpers;

namespace AdventOfCode.Solutions.Year2020 {
	/// <summary>
	/// Day 17: Conway Cubes
	/// https://adventofcode.com/2020/day/17
	/// </summary>
	public class Day17 {

		public const char OFF = '.';
		public const char ON = '#';
		public const bool OFF_STATE = false;
		public const bool ON_STATE = true;

		record Point(int X, int Y, int Z);
		record Cube(Point Point, bool State);

		private static readonly List<Point> DIRECTIONS = new()
		{
			new(-1, -1, -1),
			new(-1, -1, 0),
			new(-1, -1, 1),
			new(-1, 0, -1),
			new(-1, 0, 0),
			new(-1, 0, 1),
			new(-1, 1, -1),
			new(-1, 1, 0),
			new(-1, 1, 1),

			new(0, -1, -1),
			new(0, -1, 0),
			new(0, -1, 1),
			new(0, 0, -1),
			//new(0, 0, 0),
			new(0, 0, 1),
			new(0, 1, -1),
			new(0, 1, 0),
			new(0, 1, 1),

			new(1, -1, -1),
			new(1, -1, 0),
			new(1, -1, 1),
			new(1, 0, -1),
			new(1, 0, 0),
			new(1, 0, 1),
			new(1, 1, -1),
			new(1, 1, 0),
			new(1, 1, 1),
		};



		private static int Solution1(string[] input) {
			int noOfIterations = 6;

			Dictionary<Point, Cube> cubes = ParseInput(input);
			Dictionary<Point, Cube> nextCubes = new();

			int countOn = 0;
			for (int i = 0; i < noOfIterations; i++) {

				countOn = 0;
				nextCubes = new();
				int minX = cubes.Select(c => c.Key.X).Min() - 1;
				int maxX = cubes.Select(c => c.Key.X).Max() + 1;
				int minY = cubes.Select(c => c.Key.Y).Min() - 1;
				int maxY = cubes.Select(c => c.Key.Y).Max() + 1;
				int minZ = cubes.Select(c => c.Key.Z).Min() - 1;
				int maxZ = cubes.Select(c => c.Key.Z).Max() + 1;

				for (int z = minZ; z <= maxZ; z++) {
					for (int y = minY; y <= maxY; y++) {
						for (int x = minX; x <= maxX; x++) {
							Point p = new(x, y, z);
							Cube current;
							if (cubes.ContainsKey(p)) {
								current = cubes[p];
							} else {
								current = new(p, OFF_STATE);
							}
							Cube next = current;
							List<Cube> adjacent = GetAdjacentCubes(current, cubes);
							if (current.State == OFF_STATE) {
								if (adjacent.Count(s => s.State == ON_STATE) == 3) {
									next = next with { State = ON_STATE };
								}
							} else if (current.State == true) {
								if (adjacent.Count(s => s.State == ON_STATE) is not 2 and not 3) {
									next = next with { State = OFF_STATE };
								}
							}
							if (next.State == ON_STATE) {
								nextCubes[p] = next;
								countOn++;
							}
						}
					}
				}
				cubes = nextCubes;
			}

			return countOn;
		}
		private static List<Cube> GetAdjacentCubes(Cube cube, Dictionary<Point, Cube> cubes) {
			List<Cube> adjacent = new();
			foreach (Point dP in DIRECTIONS) {
				Point p = new(
					cube.Point.X + dP.X,
					cube.Point.Y + dP.Y,
					cube.Point.Z + dP.Z);
				bool state;
				if (cubes.ContainsKey(p)) {
					state = cubes[p].State;
				} else {
					state = false;
				}
				if (state == ON_STATE) {
					adjacent.Add(new (p, state));
				}
			}
			return adjacent;
		}

		private static string Solution2(string[] input) {
			//string inputLine = input[0];
			//List<string> inputs = input.ToList();
			//List<RecordType> instructions = input.Select(i => ParseLine(i)).ToList();
			return "** Solution not written yet **";
		}

		private static Dictionary<Point, Cube> ParseInput(string[] input) {
			Dictionary<Point, Cube> cubes = new();
			int width = input[0].Length;
			int height = input.Length;

			for (int y = 0; y < height; y++) {
				string itemLine = input[y];
				for (int x = 0; x < width; x++) {
					Point point = new(x, y, 0);
					if (itemLine[x] == ON) {
						cubes[point] = new(point, ON_STATE);
					}
				}
			}
			return cubes;
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
			bool testing = GetArgument(args, 1, false);
			if (testing is false) { return "** Solution not written yet **"; }
			// int arg1 = GetArgument(args, 1, 25);
			input = input.StripTrailingBlankLineOrDefault();
			return Solution2(input).ToString();
		}
		#endregion

	}
}
