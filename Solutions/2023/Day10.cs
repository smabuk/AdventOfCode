using Microsoft.VisualBasic;

using Smab.Helpers;

namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 10: Pipe Maze
/// https://adventofcode.com/2023/day/10
/// </summary>
[Description("Pipe Maze")]
public sealed partial class Day10 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private const char STRAIGHT_LEFT_RIGHT = '-';
	private const char STRAIGHT_UP_DOWN    = '|';
	private const char BEND_TOP_LEFT       = 'F';
	private const char BEND_TOP_RIGHT      = '7';
	private const char BEND_BOTTOM_RIGHT   = 'J';
	private const char BEND_BOTTOM_LEFT    = 'L';
	private const char GROUND              = '.';
	private const char STARTING_POSITION   = 'S';

	private static int Solution1(string[] input) {
		char[,] pipe_maze = input.To2dArray();
		Point startingPosition = pipe_maze
			.Walk2dArrayWithValues()
			.Where(cell => cell.Value == STARTING_POSITION)
			.Single()
			.Index;

		Direction startingDirection = Direction.Left;
		List<Cell<char>> adjacentCells = [.. pipe_maze.GetAdjacentCells(startingPosition)];
		foreach (Cell<char> cell in adjacentCells) {
			if (cell.Value is STRAIGHT_LEFT_RIGHT or BEND_BOTTOM_LEFT or BEND_BOTTOM_RIGHT or BEND_TOP_LEFT or BEND_TOP_RIGHT) {
				startingDirection = cell.X > startingPosition.X ? Direction.Right : Direction.Left;
				break;
			}
		}

		Animal animal = new(startingPosition, startingDirection);
		int steps = 0;
		do {
			animal = animal.Move(pipe_maze);
			steps++;
		} while (animal.Position != startingPosition);

		return steps / 2;
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}

	private record Animal(Point Position, Direction Facing)
	{

		public Animal Move(char[,] pipe_maze)
		{
			Point position = Facing switch
			{
				Direction.Left => Position.Left(),
				Direction.Right => Position.Right(),
				Direction.Up => Position.Up(),
				Direction.Down => Position.Down(),
				_ => throw new NotImplementedException(),
			};

			char track = pipe_maze[position.X, position.Y];
			Direction facing = track switch
			{
				BEND_TOP_LEFT => Facing switch
				{
					Direction.Left => Direction.Down,
					Direction.Up => Direction.Right,
					_ => throw new NotImplementedException(),
				},
				BEND_TOP_RIGHT => Facing switch
				{
					Direction.Right => Direction.Down,
					Direction.Up => Direction.Left,
					_ => throw new NotImplementedException(),
				},
				BEND_BOTTOM_RIGHT => Facing switch
				{
					Direction.Right => Direction.Up,
					Direction.Down => Direction.Left,
					_ => throw new NotImplementedException(),
				},
				BEND_BOTTOM_LEFT => Facing switch
				{
					Direction.Left => Direction.Up,
					Direction.Down => Direction.Right,
					_ => throw new NotImplementedException(),
				},
				_ => Facing,
			};

			return this with { Position = position, Facing = facing };
		}
	}

	private enum Direction { Left, Right, Up, Down }
}

