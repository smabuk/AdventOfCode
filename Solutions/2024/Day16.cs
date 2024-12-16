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

	public static int Part1(string[] _, params object[]? args)
	{
		ReindeerPosition reindeerPosition = new(_maze.ForEachCell().Single(c => c.Value is START).Index, East);
		Point end   = _maze.ForEachCell().Single(c => c.Value is END).Index;

		(int lowestScore, List<Point> route) = _maze.FindShortestPath(reindeerPosition, end);

		_maze.VisualiseMaze($"Lowest Score {lowestScore}:", route);

		return lowestScore;
	}

	public static string Part2(string[] input, params object[]? args) => NO_SOLUTION_WRITTEN_MESSAGE;

	private static (int, List<Point>) FindShortestPath(this char[,] maze, ReindeerPosition start, Point end)
	{
		int noOfRows = maze.RowsCount();
		int noOfCols = maze.ColsCount();

		int[,] distances = new int[noOfCols, noOfRows];
		Point[,] previous = new Point[noOfCols, noOfRows];
		distances.FillInPlace(int.MaxValue);
		previous.FillInPlace(new Point(-1, -1));

		distances[start.Position.X, start.Position.Y] = 0;
		SortedSet<(int, Point, Direction)> pq = [];
		_ = pq.Add((0, start.Position, start.Direction));

		while (pq.Count > 0) {
			(int currentDist, Point position, Direction prevDir) = pq.Min;
			_ = pq.Remove(pq.Min);

			if (position.Equals(end)) {
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
						_ = pq.Remove((distances[newPosition.X, newPosition.Y], newPosition, direction));
						distances[newPosition.X, newPosition.Y] = newDist;
						previous[newPosition.X, newPosition.Y] = position;
						_ = pq.Add((newDist, newPosition, direction));
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
			outputMap[point.X, point.Y] = 'x';
		}

		string[] output = ["", title, .. outputMap.AsStrings()];
		_visualise?.Invoke(output, clearScreen);
	}
}
