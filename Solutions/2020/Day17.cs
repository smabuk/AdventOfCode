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

		record Point(int X, int Y);
		record CubePoint(int X, int Y, int Z) : Point(X, Y);
		record HyperPoint(int W, int X, int Y, int Z) : Point(X, Y);
		record Cube(Point Point, bool State);

		private static int Solution1(string[] input) {
			int noOfIterations = 6;

			Dictionary<Point, Cube> cubes = ParseInput(input, 3);
			Dictionary<Point, Cube> nextCubes = new();

			int countOn = 0;
			for (int i = 0; i < noOfIterations; i++) {

				countOn = 0;
				nextCubes = new();
				int minX = cubes.Select(c => ((CubePoint)c.Key).X).Min() - 1;
				int maxX = cubes.Select(c => ((CubePoint)c.Key).X).Max() + 1;
				int minY = cubes.Select(c => ((CubePoint)c.Key).Y).Min() - 1;
				int maxY = cubes.Select(c => ((CubePoint)c.Key).Y).Max() + 1;
				int minZ = cubes.Select(c => ((CubePoint)c.Key).Z).Min() - 1;
				int maxZ = cubes.Select(c => ((CubePoint)c.Key).Z).Max() + 1;

				for (int z = minZ; z <= maxZ; z++) {
					for (int y = minY; y <= maxY; y++) {
						for (int x = minX; x <= maxX; x++) {
							CubePoint p = new(x, y, z);
							Cube current;
							if (cubes.ContainsKey(p)) {
								current = cubes[p];
							} else {
								current = new(p, OFF_STATE);
							}
							Cube next = current;
							List<Cube> adjacent = GetAdjacentCubes(current, cubes, 3);
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

		private static List<Cube> GetAdjacentCubes(Cube cube, Dictionary<Point, Cube> cubes, int noOfDimensions) {
			List<Cube> adjacent = new();
			int minW = -1, maxW = 1;
			if (noOfDimensions == 3) {
				minW = 0;
				maxW = 0;
			}

			for (int z = -1; z <= 1; z++) {
				for (int y = -1; y <= 1; y++) {
					for (int x = -1; x <= 1; x++) {
						for (int w = minW; w <= maxW; w++) {
							if (w == 0 && x == 0 && y == 0 && z == 0) {
								continue;
							}
							Point p = cube.Point switch {
								HyperPoint hp => new HyperPoint(hp.W + w, hp.X + x, hp.Y + y, hp.Z + z),
								CubePoint cp => new CubePoint(cp.X + x, cp.Y + y, cp.Z + z),
								_ => new Point(cube.Point.X + x, cube.Point.Y + y)
							};
							bool state;
							if (cubes.ContainsKey(p)) {
								state = cubes[p].State;
								adjacent.Add(new(p, state));
							}
						}
					}
				}
			}
			return adjacent;
		}

		private static int Solution2(string[] input) {
			int noOfIterations = 6;

			Dictionary<Point, Cube> cubes = ParseInput(input, 4);
			Dictionary<Point, Cube> nextCubes = new();

			int countOn = 0;
			for (int i = 0; i < noOfIterations; i++) {

				countOn = 0;
				nextCubes = new();
				int minW = cubes.Select(c => ((HyperPoint)c.Key).W).Min() - 1;
				int maxW = cubes.Select(c => ((HyperPoint)c.Key).W).Max() + 1;
				int minX = cubes.Select(c => ((HyperPoint)c.Key).X).Min() - 1;
				int maxX = cubes.Select(c => ((HyperPoint)c.Key).X).Max() + 1;
				int minY = cubes.Select(c => ((HyperPoint)c.Key).Y).Min() - 1;
				int maxY = cubes.Select(c => ((HyperPoint)c.Key).Y).Max() + 1;
				int minZ = cubes.Select(c => ((HyperPoint)c.Key).Z).Min() - 1;
				int maxZ = cubes.Select(c => ((HyperPoint)c.Key).Z).Max() + 1;

				for (int z = minZ; z <= maxZ; z++) {
					for (int y = minY; y <= maxY; y++) {
						for (int x = minX; x <= maxX; x++) {
							for (int w = minW; w <= maxW; w++) {
							HyperPoint p = new(w, x, y, z);
								Cube current;
								if (cubes.ContainsKey(p)) {
									current = cubes[p];
								} else {
									current = new(p, OFF_STATE);
								}
								Cube next = current;
								List<Cube> adjacent = GetAdjacentCubes(current, cubes, 4);
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
				}
				cubes = nextCubes;
			}

			return countOn;
		}

		private static Dictionary<Point, Cube> ParseInput(string[] input, int noOfDimensions) {
			Dictionary<Point, Cube> cubes = new();
			int width = input[0].Length;
			int height = input.Length;

			for (int y = 0; y < height; y++) {
				string itemLine = input[y];
				for (int x = 0; x < width; x++) {
					if (itemLine[x] == ON) {
						Point point = noOfDimensions switch {
							3 => new CubePoint(x, y, 0),
							4 => new HyperPoint(0, x, y, 0),
							_ => new Point( x, y)
						};
						cubes[point] = new(point, ON_STATE);
					}
				}
			}
			return cubes;
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
