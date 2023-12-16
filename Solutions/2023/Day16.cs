namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 16: The Floor Will Be Lava
/// https://adventofcode.com/2023/day/16
/// </summary>
[Description("The Floor Will Be Lava")]
public sealed partial class Day16 {

	public static string Part1(string[] input, Action<string[], bool>? visualise = null, params object[]? args) => Solution1(input, visualise).ToString();
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
	public static Action<string[], bool>? _visualise = null;

	private static int Solution1(string[] input, Action<string[], bool>? visualise = null) {
		_visualise = visualise;
		char[,] cave = input.To2dArray();

		Beam beam = new(new(0, 0), Direction.Right);
		HashSet<Beam> beams = [.. Beam.Energise(cave, beam, [])];

		int noOfEnergisedTiles = beams.Select(b => b.Position).Distinct().Count();
		return noOfEnergisedTiles;
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}

	private record struct Beam(Point Position, Direction Direction)
	{
		public static HashSet<Beam> Energise(char[,] cave, Beam thisBeam, HashSet<Beam> visited)
		{
			int count = 0;

			while (true && count++ <1000) {
				visited = [..visited, thisBeam];

				if (cave[thisBeam.Position.X, thisBeam.Position.Y] is EMPTY) {
					cave[thisBeam.Position.X, thisBeam.Position.Y] = thisBeam.Direction switch
					{
						Direction.Left  => '<',
						Direction.Right => '>',
						Direction.Up    => '^',
						Direction.Down  => 'v',
						_ => throw new NotImplementedException(),
					};
				} else if (cave[thisBeam.Position.X, thisBeam.Position.Y] is '<' or '>' or '^' or 'v') {
					cave[thisBeam.Position.X, thisBeam.Position.Y] = '2';
				} else if (cave[thisBeam.Position.X, thisBeam.Position.Y] is '2' or '3' or '4' or '5') {
					cave[thisBeam.Position.X, thisBeam.Position.Y] = (char)(cave[thisBeam.Position.X, thisBeam.Position.Y] + 1);
				}
				DisplayGrid(cave, "", _visualise);

				char obstacle = cave[thisBeam.Position.X, thisBeam.Position.Y];
				Direction newDirection = obstacle switch
				{
					MIRROR_1   when thisBeam.Direction is Direction.Left                    => Direction.Down,
					MIRROR_1   when thisBeam.Direction is Direction.Up                      => Direction.Right,
					MIRROR_1   when thisBeam.Direction is Direction.Right                   => Direction.Up,
					MIRROR_1   when thisBeam.Direction is Direction.Down                    => Direction.Left,
					MIRROR_2   when thisBeam.Direction is Direction.Right                   => Direction.Down,
					MIRROR_2   when thisBeam.Direction is Direction.Up                      => Direction.Left,
					MIRROR_2   when thisBeam.Direction is Direction.Left                    => Direction.Up,
					MIRROR_2   when thisBeam.Direction is Direction.Down                    => Direction.Right,
					SPLITTER_H when thisBeam.Direction is Direction.Down or Direction.Up    => Direction.Left,
					SPLITTER_V when thisBeam.Direction is Direction.Left or Direction.Right => Direction.Up,
					_ => thisBeam.Direction,
				};

				Point newPosition = newDirection switch
				{
					Direction.Left  => thisBeam.Position.Left(),
					Direction.Right => thisBeam.Position.Right(),
					Direction.Up    => thisBeam.Position.Up(),
					Direction.Down  => thisBeam.Position.Down(),
					_ => throw new NotImplementedException(),
				};

				if (thisBeam.Direction != newDirection && SPLITTERS.Contains(obstacle)) {
					if (cave.InBounds(newPosition.X, newPosition.Y)) {
						Beam newBeam = new(newPosition, newDirection);
						if (!visited.Contains(newBeam)) {
							visited = [.. visited, .. Energise(cave, newBeam, visited)];
						}
					}
					Direction splitDirection = obstacle == SPLITTER_H ? Direction.Right : Direction.Down;
					Point splitPosition = splitDirection switch
					{
						Direction.Left  => thisBeam.Position.Left(),
						Direction.Right => thisBeam.Position.Right(),
						Direction.Up    => thisBeam.Position.Up(),
						Direction.Down  => thisBeam.Position.Down(),
						_ => throw new NotImplementedException(),
					};
					if (cave.InBounds(splitPosition.X, splitPosition.Y)) {
						Beam splitBeam = new(splitPosition, splitDirection);
						if (!visited.Contains(splitBeam)) {
							visited = [.. visited, .. Energise(cave, splitBeam, visited)];
						}
					}
					return visited;
				}

				if (cave.OutOfBounds(newPosition.X, newPosition.Y) || visited.Contains(new Beam(new(newPosition.X, newPosition.Y), newDirection))) {
					return visited;
				}

				if (visited.Contains(new(newPosition, newDirection))) {
					return visited;
				}
				thisBeam.Position  = newPosition;
				thisBeam.Direction = newDirection;
			}
			return visited;
		}
		private static void DisplayGrid(char[,] grid, string title, Action<string[], bool>? visualise)
		{
			if (visualise is not null) {
				string[] output = ["", title, "0123456789", .. grid.PrintAsStringArray(0)];
				_ = Task.Run(() => visualise?.Invoke(output, false));
			}
		}

	}


	public enum Direction { Left, Right, Up, Down }

}
