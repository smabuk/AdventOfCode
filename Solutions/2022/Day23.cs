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



	private static int Solution1(string[] input, int noOfRounds = 10) {
		Dictionary<string, Point> elves = LoadElves(input);

		int choiceOffset = 0;

		for (int round = 1; round <= noOfRounds; round++) {
			_ = ProcessRound(elves, choiceOffset);
			choiceOffset = NextDirectionChoice(choiceOffset);
		}

		return GetSize(elves.Values) - elves.Count();
	}



	private static int Solution2(string[] input) {
		Dictionary<string, Point> elves = LoadElves(input);

		int round = 1;
		int choiceOffset = 0;

		while (ProcessRound(elves, choiceOffset)) {
			round++;
			choiceOffset = NextDirectionChoice(choiceOffset);
		}

		return round;
	}



	private static Dictionary<string, Point> LoadElves(string[] input) =>
		input
		.SelectMany((i, y) => i.Select((Tile, x) => (Tile, x, y))
		.Where(item => item.Tile == ELF)
		.Select(item => (Name: Guid.NewGuid().ToString(), Location: new Point(item.x, item.y))))
		.ToDictionary(item => item.Name, item => item.Location);




	private static bool ProcessRound(Dictionary<string, Point> elves, int choiceOffset) {
		List<Elf> proposedElfMoves = [];

		HashSet<Point> elfLocations = elves.Values.ToHashSet();
		List<string> elvesThatCanMove = elves
			.Where(elf => elfLocations.Overlaps(elf.Value.AllAdjacent()))
			.Select(elf => elf.Key)
			.ToList();

		bool noMoves = true;

		foreach (string elfName in elvesThatCanMove) {
			foreach (Point direction in Directions(choiceOffset)) {
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

		return !noMoves;


		#region Local functions

		bool CanElfMove(Point direction, Point location) {
			int[] DIRECTION_OFFSETS = { -1, 0, 1 };

			bool foundElf = false;
			foreach (int dOffset in DIRECTION_OFFSETS) {
				Point checkLocation = location + direction.X switch {
					0 => direction with { X = dOffset },
					_ => direction with { Y = dOffset },
				};
				if (elfLocations.Contains(checkLocation)) {
					foundElf = true;
					break;
				}
			}

			return !foundElf;
		}

		bool ElfChoseUniqueLocation(Elf elf)
			=> proposedElfMoves.Where(e => e.Location == elf.Location).Count() == 1;

		static IEnumerable<Point> Directions(int index) {
			Point[] DIRECTION_ORDER = {
				new( 0, -1), // North
				new( 0,  1), // South
				new(-1,  0), // West
				new( 1,  0), // East
			};

			for (int choice = 0; choice < 4; choice++) {
				Point direction = DIRECTION_ORDER[(choice + index) % 4];
				yield return direction;
			}
		}

		#endregion Local functions
	}

	private static int GetSize(IEnumerable<Point> points) {
		(Point start, Point end) = GetBounds(points);
		return (end.X - start.X + 1) * (end.Y - start.Y + 1);
	}

	private static (Point Start, Point End) GetBounds(IEnumerable<Point> points) {
		int minX = int.MaxValue;
		int maxX = int.MinValue;
		int minY = int.MaxValue;
		int maxY = int.MinValue;
		foreach (Point point in points) {
			minX = Math.Min(minX, point.X);
			maxX = Math.Max(maxX, point.X);
			minY = Math.Min(minY, point.Y);
			maxY = Math.Max(maxY, point.Y);
		}
		return (new Point(minX, minY), new Point(maxX, maxY));
	}

	private static int NextDirectionChoice(int choiceOffset) => (choiceOffset + 1) % 4;

	private record struct Elf(string Name, Point Location);

	private static void DebugPrint(IEnumerable<Point> locations) {

		(Point start, Point end) = GetBounds(locations);
		int size = (end.X - start.X + 1) * (end.Y - start.Y + 1);

		Debug.WriteLine("");
		for (int y = start.Y; y <= end.Y; y++) {
			Debug.WriteLine("");
			for (int x = start.X; x <= end.X; x++) {
				Debug.Write(locations.Any(e => e == new Point(x, y)) ? ELF : EMPTY);
			}
		}
		Debug.WriteLine("");
	}

}
