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
	private const char EMPTY               = ' ';
	private const char STARTING_POSITION   = 'S';
	private const char INSIDE              = 'I';
	private const char OUTSIDE             = 'O';
	private static readonly char[] PIPES   = [STRAIGHT_LEFT_RIGHT, STRAIGHT_UP_DOWN, BEND_TOP_LEFT, BEND_TOP_RIGHT, BEND_BOTTOM_RIGHT, BEND_BOTTOM_LEFT];

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
			if (cell.Value is STRAIGHT_LEFT_RIGHT or BEND_BOTTOM_LEFT or BEND_TOP_LEFT && startingPosition.X > cell.X) {
				startingDirection = Direction.Left;
				break;
			}
			if (cell.Value is STRAIGHT_LEFT_RIGHT or BEND_BOTTOM_RIGHT or BEND_TOP_RIGHT && startingPosition.X < cell.X) {
				startingDirection = Direction.Right;
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

	private static int Solution2(string[] input) {
		char[,] pipe_maze = input.To2dArray();
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

		List<Point> loopRoute = [];

		Animal animal = new(startingPosition, startingDirection);
		do {
			animal = animal.Move(pipe_maze);
			loopRoute.Add(animal.Position);
		} while (animal.Position != startingPosition);

		List<Point> obstacles = pipe_maze
			.Walk2dArrayWithValues()
			.Where(cell => PIPES.Contains(cell.Value))
			.Select(cell => cell.Index)
			.Except(loopRoute)
			.ToList();

		foreach (Cell<char>? cell in pipe_maze.Walk2dArrayWithValues()) {
			if (loopRoute.Contains(cell.Index)) {
			} else {
				pipe_maze[cell.X, cell.Y] = INSIDE;
			}
		}

		char[,] biggerPipeMaze = MakeABiggerMaze(pipe_maze);
		FloodFillPipeMaze(biggerPipeMaze, new(0, 0), [EMPTY, GROUND, INSIDE], OUTSIDE);

		List<Cell<char>> innerTiles = [.. biggerPipeMaze.Walk2dArrayWithValues().Where(cell => cell.Value == INSIDE)];
		return innerTiles.Count;
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

	private static void FloodFillPipeMaze(char[,] pipe_maze, Point start, char[] space, char outSide)
	{
		Queue<Point> queue = [];
		queue.Enqueue(start);
		while (queue.Count != 0) {
			Point point = queue.Dequeue();
			if (!space.Contains(pipe_maze[point.X, point.Y])) {
				continue;
			}
			pipe_maze[point.X, point.Y] = outSide;
			foreach (Cell<char> adjacent in pipe_maze.GetAdjacentCells(point)) {
				queue.Enqueue(adjacent.Index);
			}
		}
	}

	private static char[,] MakeABiggerMaze(char[,] pipe_maze)
	{
		char[,] newMaze = ArrayHelpers.Create2dArray((pipe_maze.NoOfColumns() * 3) + 3, (pipe_maze.NoOfRows() * 3) + 3, GROUND);
		foreach (Cell<char>? cell in pipe_maze.Walk2dArrayWithValues()) {
			char[,] newPipe = ConvertToBigPipe(cell.Value).To2dArray(3);
			for (int dy = 0; dy < 3; dy++) {
				for (int dx = 0; dx < 3; dx++) {
					newMaze[(cell.X * 3) + 1 + dx, (cell.Y * 3) + 1 + dy] = newPipe[dx,dy];
				}
			}
		}
		return newMaze;
	}

	private static string ConvertToBigPipe(char value) => value switch
		{
			BEND_TOP_RIGHT      => "   -7  | ",
			BEND_TOP_LEFT       => "    F- | ",
			BEND_BOTTOM_LEFT    => " |  L-   ",
			BEND_BOTTOM_RIGHT   => " | -J    ",
			STRAIGHT_UP_DOWN    => " |  |  | ",
			STRAIGHT_LEFT_RIGHT => "   ---   ",
			STARTING_POSITION   => " | -S- | ",
			_ => $"    {value}    ",
		};

	private enum Direction { Left, Right, Up, Down }


	#region Failed Attempts

	//******************************************************************************************//
	//******************************************************************************************//
	//**               P A R T I A L L Y    F A I L E D    A T T E M P T S                    **//
	//**                     W O R K    F O R    S O M E    T E S T S                         **//
	//******************************************************************************************//
	//******************************************************************************************//


	//Dictionary<int, (int Min, int Max)> minMaxPerRow = [];
	//	for (int row = 0; row<pipe_maze.NoOfRows(); row++) {
	//		minMaxPerRow[row] = (int.MaxValue, int.MinValue);
	//	}
	//minMaxPerRow[animal.Position.Y] = (
	//	Math.Min(minMaxPerRow[animal.Position.Y].Min, animal.Position.X),
	//	Math.Max(minMaxPerRow[animal.Position.Y].Max, animal.Position.X));

	//foreach (KeyValuePair<int, (int Min, int Max)> kvp in minMaxPerRow) {
	//	if (kvp.Value.Min == int.MaxValue) { continue; }
	//	int y = kvp.Key;
	//	for (int x = kvp.Value.Min + 1; x < kvp.Value.Max; x++) {
	//		if (loopRoute.Contains(new Point(x, y))) { continue; }

	//		Point currentPoint = new(x, y);
	//		if (pipe_maze[x, y] == GROUND) {

	//			if (IsPointInPolygon4([..loopRoute], currentPoint)) {
	//			//if (FindRoutesFromAToB(currentPoint, startingPosition, int.MaxValue, loopRoute, obstacles, out List<Route> routes)) {
	//				noOfTiles++;
	//			}
	//		}
	//	}
	//}

	//private static bool FindRoutesFromAToB(Point startingPosition, Point endingPosition, int maxRouteLength, IEnumerable<Point> loop, IEnumerable<Point> obstacles, [NotNullWhen(true)] out List<Route> routes)
	//{
	//	List<Route> foundRoutes = [];
	//	HashSet<Point> visited = [startingPosition];
	//	Queue<Route> queue = [];
	//	queue.Enqueue([startingPosition]);
	//	int shortestRouteLength = maxRouteLength;
	//	while (queue.Count != 0) {
	//		Route routeSoFar = queue.Dequeue();
	//		Point lastPosition = routeSoFar.Last();
	//		if (lastPosition == endingPosition) {
	//			if (routeSoFar.Count <= maxRouteLength) {
	//				foundRoutes.Add(routeSoFar);
	//				routes = foundRoutes;
	//				return true;
	//			}
	//		} else if (routeSoFar.Count < maxRouteLength) {
	//			IEnumerable<Point> nextSteps = lastPosition
	//				.Adjacent()
	//				.Where(p => !visited.Contains(p))
	//				.Except(obstacles);
	//			foreach (Point step in nextSteps) {
	//				queue.Enqueue([.. routeSoFar, step]);
	//				_ = visited.Add(step);
	//			}
	//		}
	//	}

	//	routes = foundRoutes;
	//	return routes.Count > 0;
	//}


	//private static int CountInversions(Point point, char[,] pipe_maze)
	//{
	//	for (int i = 0; i < pipe_maze.NoOfColumns(); i++) {
	//		for (int j = 0; j < pipe_maze.NoOfColumns(); j++) {
	//			if (true) {

	//			}
	//		}
	//	}
	//	return 0;
	//}



	///// <summary>
	///// Determines if the given point is inside the polygon
	///// </summary>
	///// <param name="polygon">the vertices of polygon</param>
	///// <param name="testPoint">the given point</param>
	///// <returns>true if the point is inside the polygon; otherwise, false</returns>
	//public static bool IsPointInPolygon4(Point[] polygonI, Point testPointI)
	//{
	//	System.Drawing.PointF testPoint = new(testPointI.X, testPointI.Y);
	//	System.Drawing.PointF[] polygon = [.. polygonI.Select(p => new System.Drawing.PointF(p.X, p.Y))];

	//	bool result = false;
	//	int j = polygon.Length - 1;
	//	for (int i = 0; i < polygon.Length; i++) {
	//		if ((polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y) ||
	//			(polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)) {
	//			if (polygon[i].X + ((testPoint.Y - polygon[i].Y) /
	//			   (polygon[j].Y - polygon[i].Y) *
	//			   (polygon[j].X - polygon[i].X)) < testPoint.X) {
	//				result = !result;
	//			}
	//		}
	//		j = i;
	//	}
	//	return result;
	//}
	#endregion
}
