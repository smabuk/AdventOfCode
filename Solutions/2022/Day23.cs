namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 23: Unstable Diffusion
/// https://adventofcode.com/2022/day/23
/// </summary>
[Description("Unstable Diffusion")]
public sealed partial class Day23 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static readonly char ELF = '#';
	private static readonly char EMPTY = '.';
	private static readonly Point ARBITRARY_START_POINT = new(0, 0);

	private static IEnumerable<Elf> LoadElves(string[] input) {
		for (int y = 0; y < input.Length; y++) {
			for (int x = 0; x < input[y].Length; x++) {
				if (input[y][x] == ELF) {
					yield return new Elf(Guid.NewGuid().ToString(), new Point(x, y) + ARBITRARY_START_POINT);
				}
			}
		}
	}

	private static int Solution1(string[] input) {
		const int NoOfRounds = 10;

		Dictionary<string, Elf> elves = LoadElves(input).ToDictionary(e => e.Name, e => e);
		List<Elf> proposedElfMoves = new();

		List<Point> proposedDirections = new() {
			new(0, -1), // North
			new(0, 1), // South
			new(-1, 0), // West
			new(1, 0) // East
		};


		int directionChoiceOffset = 0;
		for (int round = 1; round <= NoOfRounds; round++) {
			Debug.WriteLine("");
			Debug.WriteLine($"Round: {round}");

			proposedElfMoves.Clear();
			foreach (Elf elf in elves.Values) {

				if (!elf.Location.AllAdjacent.Intersect(elves.Values.Select(e => e.Location)).Any()) {
					continue;
				}

				for (int choice = 0; choice < 4; choice++) {
					Point direction = proposedDirections[(choice + directionChoiceOffset) % 4];
					bool foundElf = false;
					for (int d = -1; d <= 1; d++) {
						Point checkPoint = direction.X switch {
							0 => direction with { X = d },
							_ => direction with { Y = d },
						};
						if (elves.Values.Any(e => e.Location == elf.Location + checkPoint)) {
							foundElf = true;
							break;
						}
					}
					if (foundElf) {
						continue;
					} else {
						proposedElfMoves.Add(elf with { Location = elf.Location + direction });
						break;
					}
				}
			}

			foreach (Elf elf in proposedElfMoves) {
				if (proposedElfMoves.Where(e => e.Location == elf.Location).Count() == 1) {
					Debug.WriteLine($"Move Elf from {elves[elf.Name].Location} to {elf.Location}");
					elves[elf.Name] = elves[elf.Name] with { Location = elf.Location };
				}
			}
			directionChoiceOffset = (directionChoiceOffset + 1) % 4;
			DebugPrint(elves.Values);
		}


		int minX = elves.Values.Select(elf => elf.Location.X).Min();
		int minY = elves.Values.Select(elf => elf.Location.Y).Min();
		int maxX = elves.Values.Select(elf => elf.Location.X).Max();
		int maxY = elves.Values.Select(elf => elf.Location.Y).Max();
		int size = (maxX - minX + 1) * (maxY - minY + 1);

		return size - elves.Distinct().Count();
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}

	private record struct Elf(string Name, Point Location);

	private static void DebugPrint(IEnumerable<Elf> elves) {

		int minX = elves.Select(elf => elf.Location.X).Min();
		int minY = elves.Select(elf => elf.Location.Y).Min();
		int maxX = elves.Select(elf => elf.Location.X).Max();
		int maxY = elves.Select(elf => elf.Location.Y).Max();
		int size = (maxX - minX + 1) * (maxY - minY + 1);

		Debug.WriteLine("");
		for (int y = minY; y <= maxY; y++) {
			Debug.WriteLine("");
			for (int x = minX; x <= maxX; x++) {
				if (elves.Any(e => e.Location == new Point(x, y))) {
					Debug.Write(ELF);
				} else {
					Debug.Write(EMPTY);
				}
			}

		}
		Debug.WriteLine("");
	}

}
