namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 19: Beacon Scanner
/// https://adventofcode.com/2021/day/XX
/// </summary>
[Description("Beacon Scanner")]
public class Day19 {
	static List<Scanner> Scanners = new();

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		FindPositionsOfScanners(input);

		return Scanners
			.SelectMany(s => s.GetBeacons(s.Alignment).Select(b => b + (Point3d)s.Position!))
			.ToHashSet()
			.Count;
	}

	private static int Solution2(string[] input) {
		FindPositionsOfScanners(input);
			
		return Scanners
			.SelectMany(s => Scanners, (a, b) => (a, b))
			.Select(x => (pos1: (Point3d)x.a.Position!, pos2: (Point3d)x.b.Position!))
			.Select(x => GetManhattanDistance(x.pos1, x.pos2))
			.Max();
	}

	private static int GetManhattanDistance(Point3d a, Point3d b) =>
		Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z);

	private static void FindPositionsOfScanners(string[] input) {
		if (Scanners.Count != 0) {
			return;
		}

		Scanners = Parse(input);
		Scanners.First().Position = (0, 0, 0);
		while (Scanners.Where(s => s.Position is null).Any()) {
			foreach (Scanner scanner in Scanners.Where(s => s.Position is null)) {
				(bool success, Point3d position, int alignment) = FindOverlap(scanner);
				if (success) {
					scanner.Position = position;
					scanner.Alignment = alignment;
					break;
				}
			}
		}
	}

	private static (bool success, Point3d position, int alignment) FindOverlap(Scanner scanner) {
		foreach (Scanner referenceScanner in Scanners.Where(s => s.Position is not null)) {
			HashSet<Point3d> referenceBeacons = referenceScanner
				.GetBeacons(referenceScanner.Alignment)
				.ToHashSet();
			foreach (Point3d pos in referenceBeacons) {
				for (int alignment = 0; alignment < 24; alignment++) {
					HashSet<Point3d> scannerBeacons = scanner
						.GetBeacons(alignment)
						.ToHashSet();
					foreach (Point3d pos2 in scannerBeacons) {
						if (referenceScanner.Position.HasValue) {
							Point3d possiblePosition = -(pos2 - pos);
							int overlapCount = scannerBeacons
								.Select(b => b + possiblePosition)
								.Intersect(referenceBeacons)
								.Count();

							if (overlapCount >= 12) {
								return (true, (Point3d)referenceScanner.Position + possiblePosition, alignment);
							}
						}
					}
				}
			}
		}
		return (false, (0, 0, 0), 0);
	}

	record Scanner(string Name) {
		public List<Point3d> Beacons { get; set; } = new();
		public Point3d? Position { get; set; } = null;
		public int Alignment { get; set; } = 0;

		public IEnumerable<Point3d> GetBeacons(int alignment) {
			foreach (Point3d beacon in Beacons) {
				yield return ReAlign(beacon, alignment);
			}
		}
	};

	static Point3d ReAlign(Point3d p, int alignment) {

		Point3d pX = (alignment % 12) switch {
			0 =>  ( p.X,  p.Y,  p.Z),
			1 =>  ( p.X,  p.Z, -p.Y),
			2 =>  ( p.X, -p.Y, -p.Z),
			3 =>  ( p.X, -p.Z,  p.Y),
			4 =>  ( p.Y,  p.X, -p.Z),
			5 =>  ( p.Y,  p.Z,  p.X),
			6 =>  ( p.Y, -p.X,  p.Z),
			7 =>  ( p.Y, -p.Z, -p.X),
			8 =>  ( p.Z,  p.X,  p.Y),
			9 =>  ( p.Z,  p.Y, -p.X),
			10 => ( p.Z, -p.X, -p.Y),
			11 => ( p.Z, -p.Y,  p.X),
			_ => throw new NotImplementedException(),
		};

		pX = (alignment / 12) switch {
			0 => pX,
			1 => pX * (-1, 1, -1),
			_ => throw new NotImplementedException(),
		};

		return pX;
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
