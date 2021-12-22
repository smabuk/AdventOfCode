namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 22: Reactor Reboot
/// https://adventofcode.com/2021/day/22
/// </summary>
[Description("Reactor Reboot")]
public class Day22 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		List<RebootStep> rebootSteps = input.Select(i => ParseLine(i)).ToList();

		HashSet<Point3d> cubes = new();

		foreach (var rebootStep in rebootSteps) {
			if (OutOfBounds(rebootStep.XMin, rebootStep.YMin, rebootStep.ZMin)
				|| OutOfBounds(rebootStep.XMax, rebootStep.YMax, rebootStep.ZMax)) {
				continue;
			}
			for (int z = rebootStep.ZMin; z <= rebootStep.ZMax; z++) {
				for (int y = rebootStep.YMin; y <= rebootStep.YMax; y++) {
					for (int x = rebootStep.XMin; x <= rebootStep.XMax; x++) {
						if (rebootStep.TurnOn) {
							cubes.Add(new Point3d(x, y, z));
						} else {
							cubes.Remove(new Point3d(x, y, z));
						}
					}
				}
			}
		}

		return cubes.Count;

		static bool OutOfBounds(int x, int y, int z)
			=> x < -50 || x > 50 || y < -50 || y > 50 || z < -50 || z > 50;
	}

	private static long Solution2(string[] input) {
		List<RebootStep> rebootSteps = input.Select(i => ParseLine(i)).ToList();

		List<Cube> cubes = new();
		List<Cube> tempCubes = new();

		foreach (var rebootStep in rebootSteps) {
			Point3d from = new(rebootStep.XMin, rebootStep.YMin, rebootStep.ZMin);
			Point3d to =   new(rebootStep.XMax, rebootStep.YMax, rebootStep.ZMax);
			Cube newCube = new(from, to);

			tempCubes.Clear();
			// Find overlapping cuboids and take a big chunk out of it
			// where the new cube fits by splitting it up
			for (int i = 0; i < cubes.Count; i++) {
				Cube cube2 = cubes[i];
				if (CubesOverlap(cube2, newCube) is false) {
					tempCubes.Add(cube2);
				} else {
					if (newCube.From.X > cube2.From.X) {
						Cube split = cube2 with {
							To = cube2.To with {
								X = newCube.From.X - 1
							}
						};
						tempCubes.Add(split);
					}
					if (newCube.To.X < cube2.To.X) {
						Cube split = cube2 with {
							From = cube2.From with {
								X = newCube.To.X + 1
							}
						};
						tempCubes.Add(split);
					}
					if (newCube.From.Y > cube2.From.Y) {
						Cube split = cube2 with {
							From = cube2.From with {
								X = Math.Max(newCube.From.X, cube2.From.X)
							},
							To = cube2.To with {
								X = Math.Min(newCube.To.X, cube2.To.X),
								Y = newCube.From.Y - 1
							}
						};
						tempCubes.Add(split);
					}
					if (newCube.To.Y < cube2.To.Y) {
						Cube split = cube2 with {
							From = cube2.From with {
								X = Math.Max(newCube.From.X, cube2.From.X),
								Y = newCube.To.Y + 1
							},
							To = cube2.To with {
								X = Math.Min(newCube.To.X, cube2.To.X)
							}
						};
						tempCubes.Add(split);
					}
					if (newCube.From.Z > cube2.From.Z) {
						Cube split = cube2 with {
							From = cube2.From with {
								X = Math.Max(newCube.From.X, cube2.From.X),
								Y = Math.Max(newCube.From.Y, cube2.From.Y)
							},
							To = cube2.To with {
								X = Math.Min(newCube.To.X, cube2.To.X),
								Y = Math.Min(newCube.To.Y, cube2.To.Y),
								Z = Math.Max(newCube.From.Z, cube2.From.Z) - 1,
							}
						};
						tempCubes.Add(split);
					}
					if (newCube.To.Z < cube2.To.Z) {
						Cube split = cube2 with {
							From = cube2.From with {
								X = Math.Max(newCube.From.X, cube2.From.X),
								Y = Math.Max(newCube.From.Y, cube2.From.Y),
								Z = Math.Min(newCube.To.Z, cube2.To.Z) + 1
							},
							To = cube2.To with {
								X = Math.Min(newCube.To.X, cube2.To.X),
								Y = Math.Min(newCube.To.Y, cube2.To.Y)
							}
						};
						tempCubes.Add(split);
					}
				}
			}

			if (rebootStep.TurnOn) {
				tempCubes.Add(newCube);
			};

			cubes = tempCubes.ToList();
		}

		// [x] Count all the ons left
		return cubes.Sum(cube => cube.Size);
	}

	public record struct Cube(Point3d From, Point3d To) {
		public long Size => (long)(To.X - From.X + 1) * (long)(To.Y - From.Y + 1) * (long)(To.Z - From.Z + 1);
	}

	public static bool CubesOverlap(Cube a, Cube b) {
		bool cond1 = a.To.X < b.From.X;
		bool cond2 = b.To.X < a.From.X;
		bool cond3 = a.To.Y < b.From.Y;
		bool cond4 = b.To.Y < a.From.Y;
		bool cond5 = a.To.Z < b.From.Z;
		bool cond6 = b.To.Z < a.From.Z;

		return !(cond1 || cond2 || cond3 || cond4 || cond5 || cond6);
	}
	
	record struct RebootStep(bool TurnOn, int XMin, int XMax, int YMin, int YMax, int ZMin, int ZMax);

	private static RebootStep ParseLine(string input) {
		MatchCollection matches = Regex.Matches(input, @"([\+\-]*\d+)");
		if (matches.Count == 6) {
			return new(input.StartsWith("on")
				, int.Parse(matches[0].Groups[0].ValueSpan)
				, int.Parse(matches[1].Groups[0].ValueSpan)
				, int.Parse(matches[2].Groups[0].ValueSpan)
				, int.Parse(matches[3].Groups[0].ValueSpan)
				, int.Parse(matches[4].Groups[0].ValueSpan)
				, int.Parse(matches[5].Groups[0].ValueSpan)
				);
		}
		throw new ArgumentOutOfRangeException();
	}
}
