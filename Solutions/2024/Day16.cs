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
	private const int TURN_COST = 1000;

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

		(int lowestScore, List<ReindeerPosition> route) = _maze.FindShortestPath(reindeerPosition, end);

		_maze.VisualiseMaze($"Lowest Score {lowestScore}:", route[1..^1]);

		return lowestScore;
	}

	public static string Part2(string[] _)
	{
		if (_maze.ColsCount() > 20) {
			return NO_SOLUTION_MESSAGE;
		}

		ReindeerPosition reindeerPosition = new(_maze.ForEachCell().Single(c => c.Value is START).Index, East);
		Point end = _maze.ForEachCell().Single(c => c.Value is END).Index;

		List<(int Score, List<ReindeerPosition> Route)> routes = [.. _maze.FindAllPaths(reindeerPosition, end)];
		int lowestScore = routes.Min(route => route.Score);

		List<ReindeerPosition> tiles = [..routes.Where(r => r.Score == lowestScore).SelectMany(p => p.Route)];
		_maze.VisualiseMaze($"Tiles:", tiles.Select(r => r with { Direction = None }));

		return tiles.Select(p => p.Position).Distinct().Count().ToString();
	}

	private static IEnumerable<(int, List<ReindeerPosition>)> FindAllPaths(this char[,] maze, ReindeerPosition start, Point end)
	{
		Queue<(int, List<ReindeerPosition>, Direction)> queue = new();
		queue.Enqueue((0, new List<ReindeerPosition> { start }, start.Direction));
		HashSet<ReindeerPosition> visited = [];

		while (queue.Count > 0) {
			(int currentDist, List<ReindeerPosition> path, Direction prevDir) = queue.Dequeue();
			ReindeerPosition reindeerPosition = path[^1];


			if (reindeerPosition.Position == end) {
				yield return (currentDist, [.. path]);
				continue;
			}

			_ = visited.Add(reindeerPosition);

			foreach (Direction direction in Directions.NESW) {
				ReindeerPosition newPosition = reindeerPosition with { Position = reindeerPosition.Position.Translate(direction), Direction = direction};

				if (maze[newPosition.Position.X, newPosition.Position.Y] is not WALL
					//&& path.DoesNotContain(newPosition)
					&& visited.DoesNotContain(newPosition)
					)
				{
					int turnCost = (prevDir != direction) ? TURN_COST : 0;
					int newDist = currentDist + 1 + turnCost;
					List<ReindeerPosition> newPath = [.. path, newPosition];
					queue.Enqueue((newDist, newPath, direction));
				}
			}
		}
	}

	private static (int, List<ReindeerPosition>) FindShortestPath(this char[,] maze, ReindeerPosition start, Point end)
	{
		int noOfRows = maze.RowsCount();
		int noOfCols = maze.ColsCount();

		int[,] distances = new int[noOfCols, noOfRows];
		ReindeerPosition[,] previous = new ReindeerPosition[noOfCols, noOfRows];
		distances.FillInPlace(int.MaxValue);
		previous.FillInPlace(new ReindeerPosition(new Point(-1, -1), None));

		distances[start.Position.X, start.Position.Y] = 0;
		PriorityQueue<(int, Point, Direction), int> pq = new();
		pq.Enqueue((0, start.Position, start.Direction), 0);

		while (pq.Count > 0) {
			(int currentDist, Point position, Direction prevDir) = pq.Dequeue();

			if (position ==end) {
				List<ReindeerPosition> path = [];
				for (ReindeerPosition at = new(end, None); at.Position.X != -1 && at.Position.Y != -1; at = previous[at.Position.X, at.Position.Y]) {
					path.Add(at);
				}

				path.Reverse();
				return (currentDist, path);
			}

			foreach (Direction direction in Directions.NESW) {
				Point newPosition = position.Translate(direction);

				if (maze.TryGetValue(newPosition, out char value) && value is not WALL) {
					int turnCost = (prevDir != direction) ? TURN_COST : 0;
					int newDist = currentDist + 1 + turnCost;
					if (newDist < distances[newPosition.X, newPosition.Y]) {
						distances[newPosition.X, newPosition.Y] = newDist;
						previous[newPosition.X, newPosition.Y] = new (position, direction);
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

	private static void VisualiseMaze(this char[,] map, string title, IEnumerable<ReindeerPosition>? route = null, bool clearScreen = false)
	{
		if (_visualise is null) {
			return;
		}

		char[,] outputMap = (char[,])map.Clone();

		foreach (ReindeerPosition reindeerPosition in route ?? []) {
			outputMap[reindeerPosition.Position.X, reindeerPosition.Position.Y] = reindeerPosition.Direction switch
			{
				North => '^',
				East  => '>',
				West  => '<',
				South => 'v',
				_     => 'O',
			};
		}

		string[] output = ["", title, .. outputMap.AsStrings().Select(s => s.Replace('.', ' '))];
		_visualise?.Invoke(output, clearScreen);
	}
}
