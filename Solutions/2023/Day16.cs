﻿using static AdventOfCode.Solutions._2023.Day16.Direction;

namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 16: The Floor Will Be Lava
/// https://adventofcode.com/2023/day/16
/// </summary>
[Description("The Floor Will Be Lava")]
public sealed partial class Day16 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	public const char SPLITTER_H = '-';
	public const char SPLITTER_V = '|';
	public const char MIRROR_1   = '/';
	public const char MIRROR_2   = '\\';
	public const char ENERGISED  = '#';
	public const char EMPTY      = '.';

	public static readonly char[] MIRRORS   = [MIRROR_1, MIRROR_2]; 
	public static readonly char[] SPLITTERS = [SPLITTER_H, SPLITTER_V]; 
	public static readonly char[] OBSTACLES = [.. MIRRORS, .. SPLITTERS];

	private static int Solution1(string[] input) {
		char[,] cave = input.To2dArray();

		Point point = new(0, 0);
		Beam  beam  = new(point, Right);

		return Energise(cave, beam).Count;
	}

	private static int Solution2(string[] input) {
		char[,] cave = input.To2dArray();

		object? tileLock = 1;
		int size = cave.ColsCount();
		int maxTiles = int.MinValue;
		_ = Parallel.For(0, size * 4, (i, state) =>
		{
			Beam beam = GetStartBeam(i, size);
			int tiles = Energise(cave, beam).Count;
			lock (tileLock) {
				maxTiles = int.Max(maxTiles, tiles);
			}
		});

		return maxTiles;
	}

	private static Beam GetStartBeam(int i, int size)
	{
		Direction direction = (Direction)(i / size);
		int start = i % size;
		Point point = direction switch
		{
			Left => new(size - 1, start),
			Right => new(0, start),
			Up => new(start, size - 1),
			Down => new(start, 0),
			_ => throw new NotImplementedException(),
		};
		Beam beam = new(point, direction);
		return beam;
	}

	private static HashSet<Point> Energise(char[,] cave, Beam beam)
	{
		HashSet<Beam> visited = [];
		Queue<Beam> queue = new([beam]);

		while (queue.TryDequeue(out beam)) {
			_ = visited.Add(beam);

			char obstacle = cave[beam.Position.X, beam.Position.Y];
			Direction[] newDirections = obstacle switch
			{
//				   /
				MIRROR_1 when beam.Direction is Left  => [Down],
				MIRROR_1 when beam.Direction is Up    => [Right],
				MIRROR_1 when beam.Direction is Right => [Up],
				MIRROR_1 when beam.Direction is Down  => [Left],

//				   \
				MIRROR_2 when beam.Direction is Right => [Down],
				MIRROR_2 when beam.Direction is Up    => [Left],
				MIRROR_2 when beam.Direction is Left  => [Up],
				MIRROR_2 when beam.Direction is Down  => [Right],

				SPLITTER_H when beam.Direction is Down or Up    => [Left, Right],
				SPLITTER_V when beam.Direction is Left or Right => [Up, Down],
				_ => [beam.Direction],
			};

			foreach (Direction newDirection in newDirections) {
				Point newPosition = newDirection switch
				{
					Left  => beam.Position.Left(),
					Right => beam.Position.Right(),
					Up    => beam.Position.Up(),
					Down  => beam.Position.Down(),
					_ => throw new NotImplementedException(),
				};

				Beam possibleBeam = new(newPosition, newDirection);
				if (cave.InBounds(newPosition) && !visited.Contains(possibleBeam)) {
					queue.Enqueue(possibleBeam);
				}
			}
		}

		return [.. visited.Select(b => b.Position)];
	}

	private record struct Beam(Point Position, Direction Direction);

	public enum Direction { Left, Right, Up, Down }
}
