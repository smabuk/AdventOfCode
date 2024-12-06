using static AdventOfCode.Solutions._2024.Day06.Direction;

using Delta = (int dX, int dY);

namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 06: Guard Gallivant
/// https://adventofcode.com/2024/day/06
/// </summary>
[Description("Guard Gallivant")]
public partial class Day06
{
	private static char[,] _map = default!;

	public static void Init(string[] input) => _map = input.To2dArray();

	public static int Part1(string[] _) => _map.GuardPatrol().Visited.Count;

	/// <summary>
	///    The obstruction is always in the direction of movement so we only have to
	///    place them in the locations that the guard will normally move to unimpeded
	/// </summary>
	public static int Part2(string[] _)
		=> _map
			.GuardPatrol()
			.Visited
			.Where(obstruction => _map.GuardPatrol(obstruction).StuckInALoop)
			.Count();

	private static (bool StuckInALoop, HashSet<Point> Visited) GuardPatrol(this char[,] map, Point? obstruction = null)
	{
		HashSet<(Point, Direction)> cache = [];
		HashSet<Point> visited = [];

		Direction direction = Up;
		Delta delta = UP;

		Point current = map.ForEachCell().Single(cell => cell.Value == GUARD).Index;

		while (map.IsInBounds(current)) {
			if (cache.Add((current, direction)) is false) {
				return (true, visited);
			}

			_ = visited.Add(current);

			while ((map.TryGetValue(current + delta, out char nextValue) && nextValue is OBSTRUCTION) || obstruction == current + delta) {
				(direction, delta) = direction switch
				{
					Up    => (Right, RIGHT),
					Right => (Down , DOWN),
					Down  => (Left , LEFT),
					Left  => (Up   , UP),
					_ => (direction, delta)
				};
			}

			current += delta;
		}

		return (false, visited);
	}

	public enum Direction
	{
		Up,
		Right,
		Down,
		Left,
	}
}

public static partial class Day06
{
	private const char GUARD       = '^';
	private const char OBSTRUCTION = '#';

	public static readonly Delta UP    = ( 0, -1);
	public static readonly Delta DOWN  = ( 0,  1);
	public static readonly Delta LEFT  = (-1,  0);
	public static readonly Delta RIGHT = ( 1,  0);
}
