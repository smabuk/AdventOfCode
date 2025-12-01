using static Smab.Helpers.Direction;

namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 16: Reindeer Maze
/// https://adventofcode.com/2024/day/16
/// </summary>
[Description("Reindeer Maze")]
public static partial class Day16
{
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

	public static int Part1()
	{
		ReindeerPosition reindeerPosition = new(_maze.ForEachCell().Single(c => c.Value is START).Index, East);
		Point end = _maze.ForEachCell().Single(c => c.Value is END).Index;

		(int lowestScore, List<ReindeerPosition> route) = _maze.FindShortestPath(reindeerPosition, end);

		_maze.VisualiseMaze($"Lowest Score {lowestScore}:", route[1..^1]);

		return lowestScore;
	}

	public static int Part2()
	{
		ReindeerPosition reindeerPosition = new(_maze.ForEachCell().Single(c => c.Value is START).Index, East);
		Point end = _maze.ForEachCell().Single(c => c.Value is END).Index;

		HashSet<Point> tiles = _maze.FindTilesOnOptimalPaths(reindeerPosition, end);
		_maze.VisualiseMaze($"Tiles: {tiles.Count}", tiles.Select(p => new ReindeerPosition(p, None)));
		return tiles.Count;
	}

	private static HashSet<Point> FindTilesOnOptimalPaths(this char[,] maze, ReindeerPosition start, Point end)
	{
		// Dijkstra: find shortest distance to all reachable states
		Dictionary<ReindeerPosition, int> distances = new() { [start] = 0 };
		PriorityQueue<(ReindeerPosition State, int Cost), int> pq = new();
		pq.Enqueue((start, 0), 0);

		while (pq.Count > 0) {
			(ReindeerPosition current, int cost) = pq.Dequeue();
			if (cost > distances[current]) {
				continue;
			}

			// Action 1: Move forward in current direction
			Point newPos = current.Position.Translate(current.Direction);
			if (maze.IsInBounds(newPos) && maze[newPos.X, newPos.Y] is not WALL) {
				ReindeerPosition newState = new(newPos, current.Direction);
				int newCost = cost + 1;
				if (newCost < distances.GetValueOrDefault(newState, int.MaxValue)) {
					distances[newState] = newCost;
					pq.Enqueue((newState, newCost), newCost);
				}
			}

			// Action 2: Turn to face a different direction without moving
			foreach (Direction newDir in Directions.NESW) {
				if (newDir == current.Direction) {
					continue;
				}

				ReindeerPosition newState = new(current.Position, newDir);
				int newCost = cost + TURN_COST;
				if (newCost < distances.GetValueOrDefault(newState, int.MaxValue)) {
					distances[newState] = newCost;
					pq.Enqueue((newState, newCost), newCost);
				}
			}
		}

		// Find minimum cost to reach end (any direction)
		int minCost = Directions.NESW
			.Select(dir => distances.GetValueOrDefault(new ReindeerPosition(end, dir), int.MaxValue))
			.Min();
		if (minCost is int.MaxValue) {
			return [];
		}

		// BFS work backwards from the end states to collect all of the tiles on optimal paths
		HashSet<ReindeerPosition> endStates = [.. Directions.NESW
			.Select(dir => new ReindeerPosition(end, dir))
			.Where(state => distances.GetValueOrDefault(state, int.MaxValue) == minCost)];

		Queue<ReindeerPosition> queue = new(endStates);
		HashSet<ReindeerPosition> visited = [.. endStates];
		HashSet<Point> tiles = [.. endStates.Select(s => s.Position)];

		while (queue.Count > 0) {
			ReindeerPosition current = queue.Dequeue();
			int currentCost = distances[current];

			// Case 1: Moved here - check if we came from one step back in our current direction
			Point prevPos = current.Position.Translate(current.Direction.Reverse());
			if (maze.IsInBounds(prevPos) && maze[prevPos.X, prevPos.Y] is not WALL) {
				ReindeerPosition prevState = new(prevPos, current.Direction);
				if (distances.TryGetValue(prevState, out int prevCost) && prevCost + 1 == currentCost) {
					_ = tiles.Add(prevPos);
					if (visited.Add(prevState)) {
						queue.Enqueue(prevState);
					}
				}
			}

			// Case 2: Turned here - check if we turned from another direction at the same position
			foreach (Direction otherDir in Directions.NESW) {
				if (otherDir == current.Direction) {
					continue;
				}

				ReindeerPosition prevState = new(current.Position, otherDir);
				if (distances.TryGetValue(prevState, out int prevCost) && prevCost + TURN_COST == currentCost) {
					if (visited.Add(prevState)) {
						queue.Enqueue(prevState);
					}
				}
			}
		}

		return tiles;
	}

	private static (int LowestScore, List<ReindeerPosition> Route) FindShortestPath(this char[,] maze, ReindeerPosition start, Point end)
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

			if (position == end) {
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
						previous[newPosition.X, newPosition.Y] = new(position, direction);
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

		bool isTestOutput = IsTestOutput();

		char[,] outputMap = (char[,])map.Clone();

		foreach (ReindeerPosition reindeerPosition in route ?? []) {
			outputMap[reindeerPosition.Position.X, reindeerPosition.Position.Y] = reindeerPosition.Direction switch
			{
				North => '^',
				East => '>',
				West => '<',
				South => 'v',
				_ => 'O',
			};
		}

		string[] outputMapAsString = isTestOutput
			? [.. outputMap.AsStrings().Select(s => s.Replace('.', ' '))]
			: [.. outputMap.AsStrings().Select(s => s.Replace('.', ' ')
													.Replace("^", "[lime]^[/]")
													.Replace(">", "[lime]>[/]")
													.Replace("<", "[lime]<[/]")
													.Replace("v", "[lime]v[/]")
													.Replace("O", "[lime]O[/]")
			)];

		string[] output = isTestOutput
			? ["", title, .. outputMapAsString]
			: ["markup", "", title, .. outputMapAsString];
		_visualise?.Invoke(output, clearScreen);

		static bool IsTestOutput() => _visualise?.Method.DeclaringType?.FullName?.Contains(".Tests.") ?? false;
	}
}
