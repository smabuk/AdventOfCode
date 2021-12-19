namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 19: Beacon Scanner
/// https://adventofcode.com/2021/day/XX
/// </summary>
[Description("Beacon Scanner")]
public class Day19 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();
	private static int Solution1(string[] input) {
		List<Scanner> scanners = Parse(input);

		HashSet<Point3d> allBeacons = scanners[0]
			.GetBeacons(0)
			.ToHashSet();
		scanners.First().Position = new(0, 0, 0);
		while (scanners.Where(s => s.Position is null).Any()) {
			foreach (Scanner scanner in scanners.Where(s => s.Position is null)) {
				(bool success, Point3d position, int alignment) = FindOverlap(scanners, scanner);
				if (success) {
					scanner.Position = position;
					scanner.Alignment = alignment;
					allBeacons.UnionWith(scanner.GetBeacons(alignment)
								.Select(b => b + position)
								.ToHashSet());
					break;
				}
			}
		}

		return allBeacons.Count;
	}

	private static (bool success, Point3d position, int alignment) FindOverlap(List<Scanner> scanners, Scanner scanner) {
		foreach (Scanner referenceScanner in scanners.Where(s => s.Position is not null)) {
			foreach (Point3d pos in referenceScanner.GetBeacons(referenceScanner.Alignment)) {
				for (int alignment = 0; alignment < 24; alignment++) {
					foreach (Point3d pos2 in scanner.GetBeacons(alignment)) {
						if (referenceScanner.Position.HasValue) {
							Point3d possiblePosition = -(pos2 - pos);
							var beacons = scanner.
								GetBeacons(alignment)
								.Select(b => b + possiblePosition);
							int overlapCount = beacons
								.Intersect(referenceScanner.GetBeacons(referenceScanner.Alignment))
								.Count();

							if (overlapCount >= 12) {
								scanner.Position = referenceScanner.Position + possiblePosition;
								scanner.Alignment = alignment;
								return (true, (Point3d)referenceScanner.Position + possiblePosition, alignment);
							}
						}
					}
				}
			}
		}
		return (false, (0, 0, 0), 0);
	}

	private static int Solution2(string[] input) {
		List<Scanner> scanners = Parse(input);

		return 0;
	}


	record Scanner(string Name) {
		public List<Point3d> Beacons { get; set; } = new();
		public Point3d? Position { get; set; } = null;
		public int Alignment { get; set; } = 0;

		public IEnumerable<Point3d> GetBeacons(int alignment = -1) {
			if (alignment < 0) {
				alignment = Alignment;
			}

			foreach (Point3d beacon in Beacons) {
				Point3d newB = (alignment % 6) switch {
					0 => (beacon.X, beacon.Y, beacon.Z) ,
					1 => (-beacon.X, beacon.Y, -beacon.Z),
					2 => (beacon.Y, -beacon.X, beacon.Z),
					3 => (-beacon.Y, beacon.X, beacon.Z),
					4 => (beacon.Z, beacon.Y, -beacon.X),
					5 => (-beacon.Z, beacon.Y, beacon.X),
					_ => throw new NotImplementedException(),
				};

				newB = ((alignment / 6) % 4) switch {
					0 => (newB.X, newB.Y, newB.Z),
					1 => (newB.X, -newB.Z, newB.Y),
					2 => (newB.X, -newB.Y, -newB.Z),
					3 => (newB.X, newB.Z, -newB.Y),
					_ => throw new NotImplementedException(),
				};

				yield return newB;
			}

		}

	};

	public static Point3d GetAlignedPoint3d(Point3d point, int alignment) {

		Point3d newPoint = (alignment % 6) switch {
			0 => point with { X = point.X, Y = point.Y, Z = point.Z },
			1 => point with { X = -point.X, Y = point.Y, Z = -point.Z },
			2 => point with { X = point.Y, Y = -point.X, Z = point.Z },
			3 => point with { X = -point.Y, Y = point.X, Z = point.Z },
			4 => point with { X = point.Z, Y = point.Y, Z = -point.X },
			5 => point with { X = -point.Z, Y = point.Y, Z = point.X },
			_ => throw new NotImplementedException(),
		};

		newPoint = ((alignment / 6) % 4) switch {
			0 => newPoint with { X = newPoint.X, Y = newPoint.Y, Z = newPoint.Z },
			1 => newPoint with { X = newPoint.X, Y = -newPoint.Z, Z = newPoint.Y },
			2 => newPoint with { X = newPoint.X, Y = -newPoint.Y, Z = -newPoint.Z },
			3 => newPoint with { X = newPoint.X, Y = newPoint.Z, Z = -newPoint.Y },
			_ => throw new NotImplementedException(),
		};

		return newPoint;
	}

	private static List<Scanner> Parse(string[] input) {
		List<Scanner> scanners = new();
		bool isScannerNameLine = true;
		Scanner scanner = new("not used");
		foreach (string line in input) {
			if (isScannerNameLine) {
				scanner = new Scanner(line[4..^4]);
				isScannerNameLine = false;
			} else if (String.IsNullOrWhiteSpace(line)) {
				scanners.Add(scanner);
				isScannerNameLine = true;
				continue;
			} else {
				int[] coords = line.Split(",").AsInts().ToArray();
				Point3d beacon = (coords[0], coords[1], coords[2]);
				scanner.Beacons.Add(beacon);
			}
		}
		scanners.Add(scanner);
		return scanners;
	}
}
