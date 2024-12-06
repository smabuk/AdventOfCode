using static AdventOfCode.Solutions._2024.Day06.Direction;

using DirectionDelta = (int dX, int dY);

namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 06: Guard Gallivant
/// https://adventofcode.com/2024/day/06
/// </summary>
[Description("Guard Gallivant")]
public partial class Day06
{

	public static int Part1(string[] input) => input.To2dArray().GuardPatrol().Visited;

	public static int Part2(string[] input)
	{
		char[,] map = input.To2dArray();
		return map
			.ForEachCell()
			.Where(m => m.Value is not OBSTRUCTION or GUARD)
			.Where(obstruction => map.GuardPatrol(obstruction).StuckInALoop)
			.Count();
	}

	private static (bool StuckInALoop, int Visited) GuardPatrol(this char[,] map, Point? obstruction = null)
	{
		HashSet<(Point, Direction)> cache = [];
		HashSet<Point> visited = [];

		Direction direction = Up;
		DirectionDelta directionDelta = UP;

		Point current = map.ForEachCell().Single(cell => cell.Value == GUARD).Index;

		while (map.TryGetValue(current, out char value)) {
			if (cache.Add((current, direction)) is false) {
				return (true, visited.Count);
			}

			_ = visited.Add(current);

			while ((map.TryGetValue(current + directionDelta, out char nextValue) && nextValue is OBSTRUCTION) || obstruction == current + directionDelta) {
				(direction, directionDelta) = direction switch
				{
					Up => (Right, RIGHT),
					Right => (Down, DOWN),
					Down => (Left, LEFT),
					Left => (Up, UP),
					_ => (direction, directionDelta)
				};
			}

			current += directionDelta;
		}

		return (false, visited.Count);
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
	private const char SPACE       = '.';

	public static readonly (int dX, int dY) UP = (0, -1);
	public static readonly (int dX, int dY) DOWN = (0, 1);
	public static readonly (int dX, int dY) LEFT = (-1, 0);
	public static readonly (int dX, int dY) RIGHT = (1, 0);
}
