namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 10: Pipe Maze
/// https://adventofcode.com/2023/day/10
/// </summary>
[Description("Pipe Maze")]
public sealed partial class Day10 {

	public static string Part1(string[] input, Action<string[], bool>? visualise = null, params object[]? args)
		=> Solution1(input, visualise).ToString();
	public static string Part2(string[] input, Action<string[], bool>? visualise = null, params object[]? args)
	{
		string method = GetArgument(args, argumentNumber: 1, defaultResult: "inversion").ToLowerInvariant();
		SolutionMethod solutionMethod = method switch
		{
			"inversion" => SolutionMethod.InversionCount,
			"fill"      => SolutionMethod.Fill,
			"polygon"   => SolutionMethod.Polygon,
			_ => throw new ArgumentOutOfRangeException(nameof(args), $"That solution method [{method}] is not supported."),
		};
		return Solution2(input, solutionMethod, visualise).ToString();
	}

	public const char STRAIGHT_LEFT_RIGHT = '-';
	public const char STRAIGHT_UP_DOWN    = '|';
	public const char BEND_TOP_LEFT       = 'F';
	public const char BEND_TOP_RIGHT      = '7';
	public const char BEND_BOTTOM_LEFT    = 'L';
	public const char BEND_BOTTOM_RIGHT   = 'J';
	public const char GROUND              = '.';
	public const char EMPTY               = ' ';
	public const char STARTING_POSITION   = 'S';
	public const char INSIDE              = 'I';
	public const char OUTSIDE             = 'O';

	public static readonly char[] STRAIGHTS   = [STRAIGHT_LEFT_RIGHT, STRAIGHT_UP_DOWN];
	public static readonly char[] BENDS       = [BEND_TOP_LEFT, BEND_TOP_RIGHT, BEND_BOTTOM_RIGHT, BEND_BOTTOM_LEFT];
	public static readonly char[] PIPES       = [.. STRAIGHTS, ..BENDS];

	private static int Solution1(string[] input, Action<string[], bool>? visualise = null)
	{
		char[,] pipe_maze = input.To2dArray();
		SendPipeMaze(pipe_maze, "Pipe Maze:", visualise);

		(Point startingPosition, Direction startingDirection) = FindAnimal(pipe_maze);
		Animal animal = new(startingPosition, startingDirection);

		int steps = 0;
		do {
			animal = animal.Move(pipe_maze);
			steps++;
		} while (animal.Position != startingPosition);

		return steps / 2;
	}

	private static int Solution2(string[] input, SolutionMethod solutionMethod, Action<string[], bool>? visualise = null) {
		char[,] pipe_maze = input.To2dArray();
		SendPipeMaze(pipe_maze, "Pipe Maze:", visualise);

		Animal animal = FindAnimal(pipe_maze);

		Point startingPosition = animal.Position;
		HashSet<Point> loopRoute = [];
		do {
			animal = animal.Move(pipe_maze);
			_ = loopRoute.Add(animal.Position);
		} while (animal.Position != startingPosition);

		IEnumerable<Cell<char>> mazeWithoutLoop = pipe_maze
			.Walk2dArrayWithValues()
			.Where(cell => !loopRoute.Contains(cell.Index));

		foreach (Cell<char> cell in mazeWithoutLoop) {
			pipe_maze[cell.X, cell.Y] = INSIDE;
		}

		// This is what finally cracked it for me
		// I can get the methods to work when I make more room
		char[,] biggerPipeMaze = pipe_maze.CreateABiggerPipeMaze();
		SendPipeMaze(biggerPipeMaze, "Bigger Maze:", visualise);

		if (solutionMethod is SolutionMethod.Fill) {
			biggerPipeMaze.FloodFillPipeMaze(new(0, 0), [EMPTY, GROUND, INSIDE], OUTSIDE);

			SendPipeMaze(biggerPipeMaze.MakeSmallerMaze(), "Filled Maze:", visualise);
			return biggerPipeMaze
				.Walk2dArrayWithValues()
				.Where(cell => cell.Value == INSIDE)
				.Count();
		}

		if (solutionMethod is SolutionMethod.Polygon) {
			Dictionary<int, (int Min, int Max)> minMaxPerRow = [];
			for (int y = 0; y < pipe_maze.NoOfRows(); y++) {
				List<Point> pipes = [.. loopRoute.Where(pipe => pipe.Y == y)];
				if (pipes.Count == 0) {
					minMaxPerRow[y] = (int.MaxValue, int.MinValue);
				} else {
					minMaxPerRow[y] = pipes.MinMax();
				}
			}

			return mazeWithoutLoop
				.Where(cell => cell.Value == INSIDE
							&& cell.X > minMaxPerRow[cell.Y].Min && cell.X < minMaxPerRow[cell.Y].Max
							&& loopRoute.IsPointInPolygon(cell.Index))
				.Count();
		}

		if (solutionMethod is SolutionMethod.InversionCount) {
			return mazeWithoutLoop
				.Where(cell => cell.Value == INSIDE && pipe_maze.InversionsCount(loopRoute, cell.Index))
				.Count();
		}

		throw new ApplicationException("This should never be thrown!");
	}

	private static Animal FindAnimal(char[,] pipe_maze)
	{
		Point startingPosition = pipe_maze
			.Walk2dArrayWithValues()
			.Where(cell => cell.Value == STARTING_POSITION)
			.Single()
			.Index;

		Direction startingDirection = Direction.Left;
		List<Cell<char>> adjacentCells = [.. pipe_maze.GetAdjacentCells(startingPosition)];
		foreach (Cell<char> cell in adjacentCells) {
			if (cell.Value is STRAIGHT_LEFT_RIGHT or BEND_BOTTOM_LEFT or BEND_TOP_LEFT && startingPosition.X > cell.X) {
				startingDirection = Direction.Left;
				break;
			}
			if (cell.Value is STRAIGHT_LEFT_RIGHT or BEND_BOTTOM_RIGHT or BEND_TOP_RIGHT && startingPosition.X < cell.X) {
				startingDirection = Direction.Right;
				break;
			}
		}

		return new(startingPosition, startingDirection);
	}

	private static void SendPipeMaze(char[,] pipeMaze, string title, Action<string[], bool>? visualise)
	{
		if (visualise is not null) {
			string[] output = ["", title, .. pipeMaze.PrintAsStringArray(0)];
			_ = Task.Run(() => visualise?.Invoke(output, false));
		}
	}

	private record Animal(Point Position, Direction Facing)
	{
		public Animal Move(char[,] pipe_maze)
		{
			Point newPosition = Facing switch
			{
				Direction.Left  => Position.Left(),
				Direction.Right => Position.Right(),
				Direction.Up    => Position.Up(),
				Direction.Down  => Position.Down(),
				_ => throw new NotImplementedException(),
			};

			char pipe = pipe_maze[newPosition.X, newPosition.Y];
			Direction newDirection = pipe switch
			{
				BEND_TOP_LEFT     when Facing is Direction.Left  => Direction.Down,
				BEND_TOP_LEFT     when Facing is Direction.Up    => Direction.Right,
				BEND_TOP_RIGHT    when Facing is Direction.Right => Direction.Down,
				BEND_TOP_RIGHT    when Facing is Direction.Up    => Direction.Left,
				BEND_BOTTOM_RIGHT when Facing is Direction.Right => Direction.Up,
				BEND_BOTTOM_RIGHT when Facing is Direction.Down  => Direction.Left,
				BEND_BOTTOM_LEFT  when Facing is Direction.Left  => Direction.Up,
				BEND_BOTTOM_LEFT  when Facing is Direction.Down  => Direction.Right,
				_ => Facing,
			};

			return this with { Position = newPosition, Facing = newDirection };
		}
	}

	private enum Direction
	{
		Left,
		Right,
		Up,
		Down,
	}

	private enum SolutionMethod {
		Fill,
		Polygon,
		InversionCount,
	}
}

public static class Day10Helpers
{
	public static char[,] CreateABiggerPipeMaze(this char[,] pipe_maze)
	{
		char[,] newMaze = ArrayHelpers.Create2dArray((pipe_maze.NoOfColumns() * 3) + 3, (pipe_maze.NoOfRows() * 3) + 3, Day10.GROUND);
		foreach (Cell<char>? cell in pipe_maze.Walk2dArrayWithValues()) {
			char[,] newPipeValue = cell.Value.ConvertToBigPipe().To2dArray(3);
			for (int dy = 0; dy < 3; dy++) {
				for (int dx = 0; dx < 3; dx++) {
					newMaze[(cell.X * 3) + 1 + dx, (cell.Y * 3) + 1 + dy] = newPipeValue[dx, dy];
				}
			}
		}
		return newMaze;
	}

	public static char[,] MakeSmallerMaze(this char[,] pipe_maze)
	{
		char[,] newMaze = ArrayHelpers.Create2dArray((pipe_maze.NoOfColumns() / 3), (pipe_maze.NoOfRows() / 3), Day10.EMPTY);
		for (int y = 0; y < newMaze.NoOfRows() - 1; y++) {
			for (int x = 0; x < newMaze.NoOfColumns() - 1; x++) {
				newMaze[x, y] = pipe_maze[(x * 3) + 2, (y * 3) + 2];
			}
		}
		return newMaze;
	}

	private static string ConvertToBigPipe(this char value) => value switch
	{
		Day10.STRAIGHT_UP_DOWN    => " ┃  ┃  ┃ ",
		Day10.STRAIGHT_LEFT_RIGHT => "   ━━━   ",
		Day10.BEND_TOP_LEFT       => "    ┏━ ┃ ",
		Day10.BEND_TOP_RIGHT      => "   ━┓  ┃ ",
		Day10.BEND_BOTTOM_LEFT    => " ┃  ┗━   ",
		Day10.BEND_BOTTOM_RIGHT   => " ┃ ━┛    ",
		Day10.STARTING_POSITION   => " ┃ ━S━ ┃ ",
		_                         => $"    {value}    ",
	};

	public static void FloodFillPipeMaze(this char[,] pipe_maze, Point start, char[] cellTypesToFill, char fillValue)
	{
		Queue<Point> queue = [];
		queue.Enqueue(start);
		while (queue.Count != 0) {
			Point point = queue.Dequeue();
			if (!cellTypesToFill.Contains(pipe_maze[point.X, point.Y])) {
				continue;
			}
			pipe_maze[point.X, point.Y] = fillValue;
			foreach (Cell<char> adjacent in pipe_maze.GetAdjacentCells(point)) {
				queue.Enqueue(adjacent.Index);
			}
		}
	}

	/// <summary>
	/// Determines if the given point is inside the polygon
	/// </summary>
	/// <param name="points">the vertices of polygon</param>
	/// <param name="testPoint">the given point</param>
	/// <returns>true if the point is inside the polygon; otherwise, false</returns>
	/// <see cref="https://stackoverflow.com/questions/4243042/c-sharp-point-in-polygon"/>
	public static bool IsPointInPolygon(this IEnumerable<Point> points, Point testPoint)
	{
		Point[] polygon = [..points];

		bool result = false;
		int j = polygon.Length - 1;
		for (int i = 0; i < polygon.Length; i++) {
			if ((polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y) ||
				(polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)) {
				if (polygon[i].X + ((testPoint.Y - polygon[i].Y) /
				   (polygon[j].Y - polygon[i].Y) *
				   (polygon[j].X - polygon[i].X)) < testPoint.X) {
					result = !result;
				}
			}
			j = i;
		}
		return result;
	}

	/// <summary>
	/// Determines if the given point is inside the polygon by counting the boundaries crossed.
	/// Starting on the edge we know we are outside at that point, so odd numbers are inside.
	/// </summary>
	/// <param name="points">the vertices of polygon</param>
	/// <param name="testPoint">the given point</param>
	/// <returns>true if the point is inside the polygon; otherwise, false</returns>
	public static bool InversionsCount(this char[,] pipe_maze, HashSet<Point> points, Point testPoint)
	{
		if (points.Contains(testPoint)) {
			return false;
		}

		int y = testPoint.Y;
		int count = 0;
		for (int x = 0; x < testPoint.X; x++) {
			// Every time we cross the vertical boundary we add 1
			if (pipe_maze[x, y] is Day10.STRAIGHT_UP_DOWN or Day10.BEND_TOP_LEFT or Day10.BEND_TOP_RIGHT or Day10.STARTING_POSITION) {
				count++;
			}
		}

		return count % 2 == 1;
	}
}

