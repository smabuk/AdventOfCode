namespace AdventOfCode.Solutions._2021;

/// <summary>
/// Day 19: Beacon Scanner
/// https://adventofcode.com/2021/day/19
/// </summary>
[Description("Beacon Scanner")]
public class Day19 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		List<Scanner> scanners = Parse(input);
		scanners[0].SetPosition(new(0, 0, 0), 0);

		return FindPositionsOfScanners(scanners)
			.SelectMany(s => s.PositionedBeacons)
			.Distinct()
			.Count();
	}

	private static int Solution2(string[] input) {
		List<Scanner> scanners = Parse(input);
		scanners[0].SetPosition(new(0, 0, 0), 0);

		return FindPositionsOfScanners(scanners)
			.SelectMany(s => scanners, (a, b) => (a, b))
			.Select(x => (pos1: x.a.Position, pos2: x.b.Position))
			.Select(x => GetManhattanDistance(x.pos1, x.pos2))
			.Max();
	}

	private static int GetManhattanDistance(Point3d a, Point3d b) =>
		Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z);

	private static List<Scanner> FindPositionsOfScanners(List<Scanner> scanners) {

		foreach (Scanner scanner in scanners) {
			scanner.BeaconDistances = scanner.Beacons
				.SelectMany(s => scanner.Beacons, (b1, b2) => (Key: (b1, b2), Distance: GetManhattanDistance(b1, b2)))
				.Where(x => x.Distance != 0)
				.ToDictionary(x => x.Key, x => x.Distance);
		}

		while (scanners.Where(s => s.HasPosition is false).Any()) {
			foreach (Scanner scanner in scanners.Where(s => s.HasPosition is false)) {
				(bool success, Point3d position, int alignment) = FindOverlap(scanner, scanners);
				if (success) {
					scanner.SetPosition(position, alignment);
					break;
				}
			}
		}
		return scanners;
	}

	private static (bool success, Point3d position, int alignment) FindOverlap(Scanner scanner, List<Scanner> scanners) {
		foreach (Scanner referenceScanner in scanners.Where(s => s.HasPosition)) {
			// Do enough beacon distances match up
			if (referenceScanner.BeaconDistances.Values.Intersect(scanner.BeaconDistances.Values).Count() < 60) {
				continue;
			}
			// Know there are at least 12 points that match so guaranteed to still
			// have match if we skip the first 11 of each set
			foreach (Point3d pos in referenceScanner.PositionedBeacons.Skip(11)) {
				for (int alignment = 0; alignment < 24; alignment++) {
					HashSet<Point3d> scannerBeacons = scanner
						.GetBeacons(alignment)
						.ToHashSet();
					foreach (Point3d pos2 in scannerBeacons.Skip(11)) {
						if (referenceScanner.HasPosition) {
							Point3d possiblePosition = pos - pos2;
							int overlapCount = scannerBeacons
								.Select(b => b + possiblePosition)
								.Intersect(referenceScanner.PositionedBeacons)
								.Count();

							if (overlapCount >= 12) {
								return (true, possiblePosition, alignment);
							}
						}
					}
				}
			}
		}
		return (false, new(0, 0, 0), 0);
	}

	record Scanner(string Name) {
		public List<Point3d> Beacons { get; set; } = [];
		public Point3d Position { get; set; }
		public bool HasPosition { get; set; } = false;

		public HashSet<Point3d> PositionedBeacons = [];
		public Dictionary<(Point3d, Point3d), int> BeaconDistances = [];

		public IEnumerable<Point3d> GetBeacons(int alignment) {
			foreach (Point3d beacon in Beacons) {
				yield return ReAlign(beacon, alignment);
			}
		}

		internal void SetPosition(Point3d position, int alignment) {
			HasPosition = true;
			Position = position;
			PositionedBeacons = GetBeacons(alignment)
						.Select(b => b + position)
						.ToHashSet();
		}
	}
	static Point3d ReAlign(Point3d p, int alignment) {

		Point3d pX = alignment switch {
			 0 => new( p.X,  p.Y,  p.Z),
			 1 => new( p.X,  p.Z, -p.Y),
			 2 => new( p.X, -p.Y, -p.Z),
			 3 => new( p.X, -p.Z,  p.Y),
			 4 => new( p.Y,  p.X, -p.Z),
			 5 => new( p.Y,  p.Z,  p.X),
			 6 => new( p.Y, -p.X,  p.Z),
			 7 => new( p.Y, -p.Z, -p.X),
			 8 => new( p.Z,  p.X,  p.Y),
			 9 => new( p.Z,  p.Y, -p.X),
			10 => new( p.Z, -p.X, -p.Y),
			11 => new( p.Z, -p.Y,  p.X),
			12 => new(-p.X,  p.Y, -p.Z),
			13 => new(-p.X,  p.Z,  p.Y),
			14 => new(-p.X, -p.Y,  p.Z),
			15 => new(-p.X, -p.Z, -p.Y),
			16 => new(-p.Y,  p.X,  p.Z),
			17 => new(-p.Y,  p.Z, -p.X),
			18 => new(-p.Y, -p.X, -p.Z),
			19 => new(-p.Y, -p.Z,  p.X),
			20 => new(-p.Z,  p.X, -p.Y),
			21 => new(-p.Z,  p.Y,  p.X),
			22 => new(-p.Z, -p.X,  p.Y),
			23 => new(-p.Z, -p.Y, -p.X),
			_ => throw new NotImplementedException(),
		};

		return pX;
	}

	private static List<Scanner> Parse(string[] input) {
		List<Scanner> scanners = [];
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
				int[] coords = line.Split(",").As<int>().ToArray();
				Point3d beacon = new(coords[0], coords[1], coords[2]);
				scanner.Beacons.Add(beacon);
			}
		}
		scanners.Add(scanner);
		return scanners;
	}
}
