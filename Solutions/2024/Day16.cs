using static Smab.Helpers.Direction;

namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 16: Reindeer Maze
/// https://adventofcode.com/2024/day/16
/// </summary>
[Description("Reindeer Maze")]
public static partial class Day16 {
	private const char START = 'S';
	private const char END = 'E';
	private const char WALL = '#';

	private static char[,] _maze = default!;
	private static Action<string[], bool>? _visualise = null;

	[Init]
	public static void LoadMaze(string[] input, Action<string[], bool>? visualise = null)
	{
		_maze = input.To2dArray();

		_visualise = visualise;
		_maze.VisualiseMaze("Initial state:");
	}

	public static int Part1(string[] _)
	{
		ReindeerPosition reindeerPosition = new(_maze.ForEachCell().Single(c => c.Value is START).Index, East);
		Point end   = _maze.ForEachCell().Single(c => c.Value is END).Index;

		(int lowestScore, List<Point> route) = _maze.FindShortestPath(reindeerPosition, end);

		_maze.VisualiseMaze($"Lowest Score {lowestScore}:", route);

		return lowestScore;
	}

	public static int Part2(string[] _)
	{
		ReindeerPosition reindeerPosition = new(_maze.ForEachCell().Single(c => c.Value is START).Index, East);
		Point end = _maze.ForEachCell().Single(c => c.Value is END).Index;

		List<(int Score, List<Point> Route)> routes = [.. _maze.FindAllPaths(reindeerPosition, end)];
		int lowestScore = routes.Min(route => route.Score);

		List<Point> tiles = [..routes.Where(r => r.Score == lowestScore).SelectMany(p => p.Route).Distinct()];
		_maze.VisualiseMaze($"Tiles:", tiles);

		return tiles.Count;
	}

	private static IEnumerable<(int, List<Point>)> FindAllPaths(this char[,] maze, ReindeerPosition start, Point end)
	{
		Queue<(int, List<Point>, Direction)> queue = new();
		queue.Enqueue((0, new List<Point> { start.Position }, start.Direction));
		HashSet<Point> visited = [];

		while (queue.Count > 0) {
			(int currentDist, List<Point> path, Direction prevDir) = queue.Dequeue();
			Point position = path[^1];


			if (position == end) {
				yield return (currentDist, [.. path]);
				continue;
			}

			_ = visited.Add(position);

			foreach (Direction direction in Directions.NESW) {
				Point newPosition = position + direction.Delta();

				if (maze.TryGetValue(newPosition, out char value) && value is not WALL
					&& !path.Contains(newPosition)
					//&& !visited.Contains(newPosition)
					)
				{
					int turnCost = prevDir != direction ? 1000 : 0;
					int newDist = currentDist + 1 + turnCost;
					List<Point> newPath = [.. path, newPosition];
					queue.Enqueue((newDist, newPath, direction));
				}
			}
		}
	}

	private static (int, List<Point>) FindShortestPath(this char[,] maze, ReindeerPosition start, Point end)
	{
		int noOfRows = maze.RowsCount();
		int noOfCols = maze.ColsCount();

		int[,] distances = new int[noOfCols, noOfRows];
		Point[,] previous = new Point[noOfCols, noOfRows];
		distances.FillInPlace(int.MaxValue);
		previous.FillInPlace(new Point(-1, -1));

		distances[start.Position.X, start.Position.Y] = 0;
		PriorityQueue<(int, Point, Direction), int> pq = new();
		pq.Enqueue((0, start.Position, start.Direction), 0);

		while (pq.Count > 0) {
			(int currentDist, Point position, Direction prevDir) = pq.Dequeue();

			if (position ==end) {
				List<Point> path = [];
				for (Point at = end; at.X != -1 && at.Y != -1; at = previous[at.X, at.Y]) {
					path.Add(at);
				}

				path.Reverse();
				return (currentDist, path);
			}

			foreach (Direction direction in Directions.NESW) {
				Point newPosition = position + direction.Delta();

				if (maze.TryGetValue(newPosition, out char value) && value is not WALL) {
					int turnCost = (prevDir != direction) ? 1000 : 0;
					int newDist = currentDist + 1 + turnCost;
					if (newDist < distances[newPosition.X, newPosition.Y]) {
						distances[newPosition.X, newPosition.Y] = newDist;
						previous[newPosition.X, newPosition.Y] = position;
						pq.Enqueue((newDist, newPosition, direction), newDist);
					}
				}
			}
		}

		return (-1, []); // Path not found
	}


	private record ReindeerPosition(Point Position, Direction Direction);
	private record Step(ReindeerPosition Position, Direction PreviousDirection)
	{
		public int Score => Position.Direction != PreviousDirection ? 1001 : 1;
	}

	private static void VisualiseMaze(this char[,] map, string title, IEnumerable<Point>? route = null, bool clearScreen = false)
	{
		if (_visualise is null) {
			return;
		}

		char[,] outputMap = (char[,])map.Clone();

		foreach (Point point in route ?? []) {
			outputMap[point.X, point.Y] = 'O';
		}

		string[] output = ["", title, .. outputMap.AsStrings()];
		_visualise?.Invoke(output, clearScreen);
	}
}
