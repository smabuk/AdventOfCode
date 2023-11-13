namespace AdventOfCode.Solutions._2018;

/// <summary>
/// Day 15: Beverage Bandits
/// https://adventofcode.com/2018/day/15
/// </summary>
[Description("Beverage Bandits")]
public sealed partial class Day15
{

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();


	private const char WALL = '#';
	private const char OPEN_CAVERN = '.';
	private const char GOBLIN = 'G';
	private const char ELF = 'E';

	private static readonly char[] UNITS = [GOBLIN, ELF];

	private static int Solution1(string[] input)
	{
		LoadCavern(input, out List<Unit> units, out HashSet<Point> walls, out HashSet<Point> open, out char[,] cavern);
		int round = 0;

		do {
			HashSet<Point> unitPositions = units.Select(u => u.Position).ToHashSet();
			List<Unit> orderedUnits = [.. units.OrderBy(u => u.Position.Y).ThenBy(u => u.Position.X)];
			for (int uIndex = 0; uIndex < units.Count; uIndex++) {
				Unit unit = units[uIndex];
				IEnumerable<Point> inRange = InRangePositions(unit, units, walls, unitPositions);
				if (!inRange.Any()) {
					goto FightOver;
				}
			}



			round++;
		} while (round < 10_000);

		FightOver:
		return round * units.Sum(u => u.HitPoints);
	}

	private static IEnumerable<Point> InRangePositions(Unit unit, List<Unit> units, HashSet<Point> walls, HashSet<Point> unitPositions)
	{
		return units
			.Where(u => u.IsTarget(unit))
			.SelectMany(u => u.Position.Adjacent())
			.Except(walls).Except(unitPositions)
			//.OrderBy(p => p.Y).ThenBy(p => p.X)
			.Distinct();
	}

	private static string Solution2(string[] input)
	{
		return "** Solution not written yet **";
	}

	private static void LoadCavern(string[] input, out List<Unit> units, out HashSet<Point> walls, out HashSet<Point> open_cavern, out char[,] cavern)
	{
		cavern = String.Join("", input).To2dArray<char>(input[0].Length);

		units = cavern
			.Walk2dArrayWithValues()
			.Where(c => c.Value is ELF or GOBLIN)
			.Select(c => (Unit)(c.Value switch
			{
				ELF => new Elf(c.Index),
				GOBLIN => new Goblin(c.Index),
				_ => throw new NotImplementedException(),
			}))
			.ToList();
		foreach (var unit in units) {
			cavern[unit.Position.X, unit.Position.Y] = OPEN_CAVERN;
		}

		walls = cavern
			.Walk2dArrayWithValues()
			.Where(c => c.Value == WALL)
			.Select(c => c.Index)
			.ToHashSet();
		open_cavern = cavern
			.Walk2dArrayWithValues()
			.Where(c => c.Value == OPEN_CAVERN)
			.Select(c => c.Index)
			.ToHashSet();
	}

	private record Unit(Point Position)
	{
		public int HitPoints { get; set; } = 200;
		public bool IsTarget(Unit unit) => !GetType().Equals(unit.GetType());
	};

	private record Elf(Point Position) : Unit(Position);
	private record Goblin(Point Position) : Unit(Position);

}
