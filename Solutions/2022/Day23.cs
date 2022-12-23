namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 23: Unstable Diffusion
/// https://adventofcode.com/2022/day/23
/// </summary>
[Description("Unstable Diffusion")]
public sealed partial class Day23 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static readonly char ELF   = '#';
	private static readonly char EMPTY = '.';

	private static readonly List<Point> DIRECTION_ORDER = new() {
		new( 0, -1), // North
		new( 0,  1), // South
		new(-1,  0), // West
		new( 1,  0), // East
	};

	private static int Solution1(string[] input) {
		const int NoOfRounds = 10;

		Dictionary<string, Point> elves = LoadElves(input);
		List<Elf> proposedElfMoves = new();

		int directionOffset = 0;
		for (int round = 1; round <= NoOfRounds; round++) {
			_ = ProcessRound(elves, directionOffset);
			directionOffset = NextDirectionOffset(directionOffset);
		}

		int minX = elves.Values.Select(l => l.X).Min();
		int minY = elves.Values.Select(l => l.Y).Min();
		int maxX = elves.Values.Select(l => l.X).Max();
		int maxY = elves.Values.Select(l => l.Y).Max();
		int size = (maxX - minX + 1) * (maxY - minY + 1);

		return size - elves.Distinct().Count();
	}

	private static int Solution2(string[] input) {
		Dictionary<string, Point> elves = LoadElves(input);

		int round = 1;
		int directionOffset = 0;
		
		while (ProcessRound(elves, directionOffset) is false) {
			round++;
			directionOffset = NextDirectionOffset(directionOffset);
		}

		return round;
	}


	private static Dictionary<string, Point> LoadElves(string[] input) =>
		input
		.SelectMany((i, y) => i.Select((Tile, x) => (Tile, x, y))
		.Where(item => item.Tile == ELF)
		.Select(item => (Name: Guid.NewGuid().ToString(), Location: new Point(item.x, item.y))))
		.ToDictionary(item => item.Name, item => item.Location);

	private static bool ProcessRound(Dictionary<string, Point> elves, int directionChoiceOffset) {
		List<Elf> proposedElfMoves = new();

		HashSet<Point> elfLocations = elves.Values.ToHashSet();

		List<string> elvesThatCanMove = elves
			.Where(elf => elfLocations.Overlaps(elf.Value.AllAdjacent))
			.Select(elf => elf.Key)
			.ToList();

		bool noMoves = true;
		
		foreach (string elfName in elvesThatCanMove) {
			foreach (Point direction in Directions(directionChoiceOffset)) {
				Point location = elves[elfName];
				if (CanElfMove(direction, location)) {
					proposedElfMoves.Add(new Elf(elfName, location + direction));
					break;
				}
			}
		}

		foreach (Elf elf in proposedElfMoves) {
			if (ElfChoseUniqueLocation(elf)) {
				elves[elf.Name] = elf.Location;
				noMoves = false;
			}
		}

		return noMoves;

		bool CanElfMove(Point direction, Point location) {
			bool foundElf = false;

			for (int d = -1; d <= 1; d++) {
				Point checkDirection = direction.X switch {
					0 => direction with { X = d },
					_ => direction with { Y = d },
				};
				if (elfLocations.Contains(location + checkDirection)) {
					foundElf = true;
					break;
				}
			}

			return !foundElf;
		}

		bool ElfChoseUniqueLocation(Elf elf)
			=> proposedElfMoves.Where(e => e.Location == elf.Location).Count() == 1;

		static IEnumerable<Point> Directions(int index) {
			for (int choice = 0; choice < 4; choice++) {
				Point direction = DIRECTION_ORDER[(choice + index) % 4];
				yield return direction;
			}
		}
	}
	
	private static int NextDirectionOffset(int directionOffset) => (directionOffset + 1) % 4;

	private record struct Elf(string Name, Point Location);

	private static void DebugPrint(IEnumerable<Point> locations) {

		int minX = locations.Select(l => l.X).Min();
		int minY = locations.Select(l => l.Y).Min();
		int maxX = locations.Select(l => l.X).Max();
		int maxY = locations.Select(l => l.Y).Max();
		int size = (maxX - minX + 1) * (maxY - minY + 1);

		Debug.WriteLine("");
		for (int y = minY; y <= maxY; y++) {
			Debug.WriteLine("");
			for (int x = minX; x <= maxX; x++) {
				if (locations.Any(e => e == new Point(x, y))) {
					Debug.Write(ELF);
				} else {
					Debug.Write(EMPTY);
				}
			}

		}
		Debug.WriteLine("");
	}

}
