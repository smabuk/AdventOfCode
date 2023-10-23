namespace AdventOfCode.Solutions._2021;

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

		HashSet<Point3d> cuboids = [];

		foreach (var rebootStep in rebootSteps) {
			if (OutOfBounds(rebootStep.XMin, rebootStep.YMin, rebootStep.ZMin)
				|| OutOfBounds(rebootStep.XMax, rebootStep.YMax, rebootStep.ZMax)) {
				continue;
			}
			for (int z = rebootStep.ZMin; z <= rebootStep.ZMax; z++) {
				for (int y = rebootStep.YMin; y <= rebootStep.YMax; y++) {
					for (int x = rebootStep.XMin; x <= rebootStep.XMax; x++) {
						if (rebootStep.TurnOn) {
							_ = cuboids.Add(new Point3d(x, y, z));
						} else {
							_ = cuboids.Remove(new Point3d(x, y, z));
						}
					}
				}
			}
		}

		return cuboids.Count;

		static bool OutOfBounds(int x, int y, int z)
			=> x < -50 || x > 50 || y < -50 || y > 50 || z < -50 || z > 50;
	}

	private static long Solution2(string[] input) {
		List<RebootStep> rebootSteps = input.Select(i => ParseLine(i)).ToList();

		List<Cuboid> cuboids = [];
		List<Cuboid> tempCuboids = [];

		foreach (var rebootStep in rebootSteps) {
			Point3d from = new(rebootStep.XMin, rebootStep.YMin, rebootStep.ZMin);
			Point3d to =   new(rebootStep.XMax, rebootStep.YMax, rebootStep.ZMax);
			Cuboid newCuboid = new(from, to);

			tempCuboids.Clear();
			// Find overlapping cuboids and take a big chunk out of it
			// where the new cube fits by splitting it up
			for (int i = 0; i < cuboids.Count; i++) {
				Cuboid cuboid2 = cuboids[i];
				if (CuboidsOverlap(cuboid2, newCuboid) is false) {
					tempCuboids.Add(cuboid2);
				} else {
					if (newCuboid.From.X > cuboid2.From.X) {
						Cuboid split = cuboid2 with {
							To = cuboid2.To with {
								X = newCuboid.From.X - 1
							}
						};
						tempCuboids.Add(split);
					}
					if (newCuboid.To.X < cuboid2.To.X) {
						Cuboid split = cuboid2 with {
							From = cuboid2.From with {
								X = newCuboid.To.X + 1
							}
						};
						tempCuboids.Add(split);
					}
					if (newCuboid.From.Y > cuboid2.From.Y) {
						Cuboid split = cuboid2 with {
							From = cuboid2.From with {
								X = Math.Max(newCuboid.From.X, cuboid2.From.X)
							},
							To = cuboid2.To with {
								X = Math.Min(newCuboid.To.X, cuboid2.To.X),
								Y = newCuboid.From.Y - 1
							}
						};
						tempCuboids.Add(split);
					}
					if (newCuboid.To.Y < cuboid2.To.Y) {
						Cuboid split = cuboid2 with {
							From = cuboid2.From with {
								X = Math.Max(newCuboid.From.X, cuboid2.From.X),
								Y = newCuboid.To.Y + 1
							},
							To = cuboid2.To with {
								X = Math.Min(newCuboid.To.X, cuboid2.To.X)
							}
						};
						tempCuboids.Add(split);
					}
					if (newCuboid.From.Z > cuboid2.From.Z) {
						Cuboid split = cuboid2 with {
							From = cuboid2.From with {
								X = Math.Max(newCuboid.From.X, cuboid2.From.X),
								Y = Math.Max(newCuboid.From.Y, cuboid2.From.Y)
							},
							To = cuboid2.To with {
								X = Math.Min(newCuboid.To.X, cuboid2.To.X),
								Y = Math.Min(newCuboid.To.Y, cuboid2.To.Y),
								Z = Math.Max(newCuboid.From.Z, cuboid2.From.Z) - 1,
							}
						};
						tempCuboids.Add(split);
					}
					if (newCuboid.To.Z < cuboid2.To.Z) {
						Cuboid split = cuboid2 with {
							From = cuboid2.From with {
								X = Math.Max(newCuboid.From.X, cuboid2.From.X),
								Y = Math.Max(newCuboid.From.Y, cuboid2.From.Y),
								Z = Math.Min(newCuboid.To.Z, cuboid2.To.Z) + 1
							},
							To = cuboid2.To with {
								X = Math.Min(newCuboid.To.X, cuboid2.To.X),
								Y = Math.Min(newCuboid.To.Y, cuboid2.To.Y)
							}
						};
						tempCuboids.Add(split);
					}
				}
			}

			if (rebootStep.TurnOn) {
				tempCuboids.Add(newCuboid);
			};

			cuboids = tempCuboids.ToList();
		}

		// [x] Count all the ons left
		return cuboids.Sum(cube => cube.Size);
	}

	public record struct Cuboid(Point3d From, Point3d To) {
		public long Size => (long)(To.X - From.X + 1) * (long)(To.Y - From.Y + 1) * (long)(To.Z - From.Z + 1);
	}

	public static bool CuboidsOverlap(Cuboid a, Cuboid b) {
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
