using static Smab.Helpers.ArrayHelpers;
using static AdventOfCode.Solutions._2024.Day06.Direction;

using DirectionDelta = (int dX, int dY);

namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 06: Guard Gallivant
/// https://adventofcode.com/2024/day/06
/// </summary>
[Description("Guard Gallivant")]
public partial class Day06 {

	[Part1]
	public static int Part1(string[] input, params object[]? args)
	{
		char[,] map = input.To2dArray();

		HashSet<Point> visited = [];
		Direction direction = Direction.Up;
		DirectionDelta directionDelta = UP;
		Point current = map.ForEachCell().Single(cell => cell.Value == GUARD).Index;

		while (map.TryGetValue(current, out char value)) {
			visited.Add(current);

			int count = 0;
			while (map.TryGetValue(current + directionDelta, out char nextValue) && nextValue is OBSTRUCTION)
			{
				(direction, directionDelta) = direction switch
				{
					Up    => (Right, RIGHT),
					Right => (Down, DOWN),
					Down  => (Left, LEFT),
					Left  => (Up, UP),
					_ => (direction, directionDelta)
				};
			}

			current += directionDelta;
		}

		return visited.Count();
	}

	[Part2] public static string Part2(string _, params object[]? args ) => NO_SOLUTION_WRITTEN_MESSAGE;

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
}
