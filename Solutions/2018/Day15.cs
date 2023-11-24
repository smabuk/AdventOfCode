using Route = System.Collections.Generic.List<Smab.Helpers.Point>;

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
			HashSet<Point> unitPositions = [.. units.Where(u => u.IsAlive).Select(u => u.Position)];
			List<Unit> orderedUnits = [.. units.Where(u => u.IsAlive).ReadingOrder()];
			for (int uIndex = 0; uIndex < orderedUnits.Count; uIndex++) {
				Unit unit = orderedUnits[uIndex];
				if (unit.IsDead) {
					continue;
				}
				if (!units.Where(u => u.IsTarget(unit) && u.IsAlive).Any()) {
					goto FightOver;
				}
				IEnumerable<Point> obstacles = [.. walls, .. units.Where(u => u.IsAlive).Select(u => u.Position)];
				IEnumerable<Point> inRange = InRangePositions(unit, units, obstacles);
				if (TryAndAttack(unit, units) is false) {
					Route? route = FindShortestRoute(unit, inRange, obstacles);
					if (route is not null) {
						unit.MoveTo(route.Skip(1).First());
					}
					_ = TryAndAttack(unit, units);
				}
			}

			round++;
		} while (round < 1_000); // remove this when the function works

	FightOver:
		return (round * units.Where(u => u.IsAlive).Sum(u => u.HitPoints));
	}

	private static bool TryAndAttack(Unit unit, List<Unit> units)
	{
		List<Point> adjacentPointsWithTargets = unit.Position
		.Adjacent()
		.ToHashSet()
		.Intersect(units.Where(u => u.IsTarget(unit)).Select(u => u.Position).ToList())
		.ToList();
		if (adjacentPointsWithTargets.Count != 0) {
			Unit target = adjacentPointsWithTargets
				.Select(t => units.Where(u => u.Position == t && u.IsAlive).Single())
				.OrderBy(u => u.HitPoints)
				.ThenByReadingOrder()
				.First();
			_ = target.TakeDamage(unit.AttackPower);
			return true;
		}
		return false;
	}


	private static IEnumerable<Point> InRangePositions(Unit unit, List<Unit> units, IEnumerable<Point> obstacles)
	{
		return units
			.Where(u => u.IsTarget(unit))
			.SelectMany(u => u.Position.Adjacent())
			.Except(obstacles)
			//.ReadingOrder()
			.Distinct();
	}

	private static Route? FindShortestRoute(Unit unit, IEnumerable<Point> inRangePositions, IEnumerable<Point> obstacles)
	{
		List<Route> foundRoutes = [];
		int shortestRouteLength = int.MaxValue;
		foreach (Point point in inRangePositions) {
			if (FindShortestRoutesFromAToB(unit.Position, point, shortestRouteLength, obstacles, out List<Route> routes)) {
				int shortest = routes.MinBy(r => r.Count)?.Count ?? int.MaxValue;
				if (shortest <= shortestRouteLength) {
					shortestRouteLength = shortest;
					foundRoutes.AddRange(routes);
				}
			}

		}

		Route? route = foundRoutes
			.Where(r => r.Count == shortestRouteLength)
			.OrderBy(r => r.First().Y).ThenBy(r => r.First().X)
			.FirstOrDefault();

		return route;
	}

	private static bool FindShortestRoutesFromAToB(Point startingPosition, Point endingPosition, int maxRouteLength, IEnumerable<Point> obstacles, [NotNullWhen(true)] out List<Route> routes)
	{
		List<Route> foundRoutes = [];
		HashSet<Point> visited = [startingPosition];
		Queue<Route> queue = [];
		queue.Enqueue([startingPosition]);
		int shortestRouteLength = maxRouteLength;
		while (queue.Count != 0) {
			Route routeSoFar = queue.Dequeue();
			Point lastPosition = routeSoFar.Last();
			if (lastPosition == endingPosition) {
				if (routeSoFar.Count <= maxRouteLength) {
					foundRoutes.Add(routeSoFar);
				}
			} else if (routeSoFar.Count < maxRouteLength) {
				IEnumerable<Point> nextSteps = lastPosition
					.Adjacent()
					.Where(p => !visited.Contains(p))
					.Except(obstacles)
					.ReadingOrder();
				foreach (Point step in nextSteps) {
					queue.Enqueue([.. routeSoFar, step]);
					_ = visited.Add(step);
				}
			}
		}

		routes = foundRoutes;
		return routes.Count > 0;
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

	private static string[] PrintGrid(List<Unit> units, char[,] cavern) {
		string[] output = [..cavern.PrintAsStringArray(0)];
		foreach (Unit unit in units.Where(u => u.IsAlive).ReadingOrder()) {
			output[unit.Position.Y] = $"{output[unit.Position.Y][..unit.Position.X]}{unit.Display}{output[unit.Position.Y][(unit.Position.X + 1)..]}";
			output[unit.Position.Y] += $"  {unit.Display}({unit.HitPoints})";
		}
		return output;
	}

	internal record Unit(Point InitialPosition, int AttackPower)
	{
		public Point Position { get; set; } = InitialPosition;
		public int HitPoints { get; set; } = 200;
		public bool IsAlive => HitPoints  > 0;
		public bool IsDead  => HitPoints <= 0;
		public bool IsTarget(Unit unit) => IsAlive && !GetType().Equals(unit.GetType());
		public void MoveTo(Point newPosition) => Position = newPosition;
		public bool TakeDamage(int value)
		{
			HitPoints -= value;
			return IsDead;
		}
		public char Display => this is Elf ? ELF : GOBLIN;

	};

	internal record Elf   (Point Position) : Unit(Position, 3);
	internal record Goblin(Point Position) : Unit(Position, 3);

}
file static class PointExtensions
{
	public static IOrderedEnumerable<Point> ReadingOrder(this IEnumerable<Point> points)
		=> points.OrderBy(p => p.Y).ThenBy(p => p.X);

	public static IOrderedEnumerable<Point> ThenByReadingOrder(this IOrderedEnumerable<Point> points)
		=> points.ThenBy(p => p.Y).ThenBy(p => p.X);
}

file static class UnitExtensions
{
	public static IOrderedEnumerable<Day15.Unit> ReadingOrder(this IEnumerable<Day15.Unit> units)
		=> units.OrderBy(u => u.Position.Y).ThenBy(u => u.Position.X);
	public static IOrderedEnumerable<Day15.Unit> ThenByReadingOrder(this IOrderedEnumerable<Day15.Unit> units)
		=> units.ThenBy(u => u.Position.Y).ThenBy(u => u.Position.X);

}
