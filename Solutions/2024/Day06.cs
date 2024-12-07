using static AdventOfCode.Solutions._2024.Day06.Direction;

using Delta = (int dX, int dY);

namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 06: Guard Gallivant
/// https://adventofcode.com/2024/day/06
/// </summary>
[Description("Guard Gallivant")]
public static partial class Day06
{
	private static char[,] _map = default!;
	private static Guard _guardStart = default!;

	public static void Init(string[] input)
	{
		_map = input.To2dArray();
		_guardStart = new(_map.ForEachCell().Single(cell => cell.Value == GUARD).Index, Up);
	}

	public static int Part1(string[] _) => _map.GuardPatrol(_guardStart).Visited.Count;

	public static int Part2(string[] _)
	{
		return
			_map
			.GuardPatrol(_guardStart)
			.FullPath
			.Where(pos => pos.Position != _guardStart.Position)
			.ToList()
			.CountLoops(_map);
	}

	/// <summary>
	///    The obstruction is always in the direction of movement so we only have to
	///    place them in the locations that the guard will normally move to
	///    EUREKA MOMENT - Start the Guard from just before the obstacle!!!
	/// </summary>
	private static int CountLoops(this List<Guard> patrol, char[,] map)
	{
		HashSet<Point> visited = [];

		int count = 0;
		for (int i = 0; i < patrol.Count - 1; i++) {
			Point obstruction = patrol[i].Position;
			if (visited.Contains(obstruction)) {
				continue;
			}

			_ = visited.Add(obstruction);

			if (map.GuardPatrol(patrol[int.Clamp(i - 1, 0, patrol.Count - 2)], obstruction).StuckInALoop) {
				count++;
			}
		}

		return count;
	}

	private static (bool StuckInALoop, HashSet<Point> Visited, List<Guard> FullPath) GuardPatrol(this char[,] map, Guard start, Point? obstruction = null)
	{
		HashSet<(Point, Direction)> cache = [];
		HashSet<Point> visited = [];
		List<Guard> fullRoute = [];

		Direction direction = start.Direction;
		Point current = start.Position;

		while (map.IsInBounds(current)) {
			if (cache.Add((current, direction)) is false) {
				return (true, visited, fullRoute);
			}

			_ = visited.Add(current);

			if (obstruction is null) {
				fullRoute.Add(new Guard(current, direction));
			}

			while ((map.TryGetValue(current + direction.Delta(), out char nextValue) && nextValue is OBSTRUCTION)
				|| obstruction == current + direction.Delta()) {
				direction = direction.TurnRight();
			}

			current += direction.Delta();
		}

		if (obstruction is null) {
			fullRoute.Add(new Guard(current, direction));
		}

		return (false, visited, fullRoute);
	}

	private static Delta Delta(this Direction direction) => direction switch
	{
		Up    => UP,
		Right => RIGHT,
		Down  => DOWN,
		Left  => LEFT,
		_ => throw new NotImplementedException(),
	};

	private static Direction TurnRight(this Direction direction) => direction switch
	{
		Up => Right,
		Right => Down,
		Down => Left,
		Left => Up,
		_ => throw new NotImplementedException(),
	};

	private record Guard(Point Position, Direction Direction);

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
