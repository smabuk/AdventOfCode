namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 18: RAM Run
/// https://adventofcode.com/2024/day/18
/// </summary>
[Description("RAM Run")]
public static partial class Day18 {

	private static List<Point> _bytes = default!;
	private static Action<string[], bool>? _visualise = null;

	[Init]
	public static void Init(string[] input, Action<string[], bool>? visualise = null)
	{
		_bytes = [.. input.Select(i => i.As<Point>())];
		_visualise = visualise;
	}

	public static int Part1(string[] _, params object[]? args)
	{
		int gridSize  = args.GridSize();
		int noOfBytes = args.Bytes();

		Point start = Point.Zero;
		Point end   = new(gridSize - 1, gridSize - 1);

		_bytes.Take(noOfBytes).VisualiseRam("Initial state:", gridSize, []);

		List<Point> shortestPath = FindShortestPath(start, end, _bytes.Take(noOfBytes), gridSize);

		_bytes.Take(noOfBytes).VisualiseRam("Final:", gridSize, shortestPath);

		return shortestPath.Count - 1; // shortestPath includes start
	}

	public static string Part2(string[] _, params object[]? args)
	{
		int gridSize  = args.GridSize();
		int noOfBytes = args.Bytes();

		Point start = Point.Zero;
		Point end   = new(gridSize - 1, gridSize - 1);
		
		List<Point> shortestPath = [Point.Zero];
		while (shortestPath is not []) {
			shortestPath = FindShortestPath(start, end, _bytes.Take(++noOfBytes), gridSize);
		}

		noOfBytes--;
		return $"{_bytes[noOfBytes].X},{_bytes[noOfBytes].Y}";
	}

	public static List<Point> FindShortestPath(Point start, Point goal, IEnumerable<Point> obstacles, int gridSize)
	{
		PriorityQueue<Point, int> priorityQueue = new();
		Dictionary<Point, int> cost = new() { [start] = 0 };
		Dictionary<Point, int> distances = new() { [start] = start.ManhattanDistance(goal) };
		Dictionary<Point, Point> previous = [];

		HashSet<Point> obstaclesSet = [.. obstacles];

		priorityQueue.Enqueue(start, distances[start]);

		while (priorityQueue.Count > 0) {
			Point current = priorityQueue.Dequeue();

			if (current == goal) {
				return ReconstructPath(previous, current);
			}

			foreach (Direction direction in Directions.NSEW) {
				Point adjacent = current + direction.Delta();

				// Ignore invalid neighbours
				if (adjacent.X < 0 || adjacent.Y < 0 || adjacent.X >= gridSize || adjacent.Y >= gridSize
					|| obstaclesSet.Contains(adjacent)) {
					continue;
				}

				int tentativeCost = cost[current] + 1;

				if (tentativeCost < cost.GetValueOrDefault(adjacent, int.MaxValue)) {
					previous[adjacent]  = current;
					cost[adjacent]      = tentativeCost;
					distances[adjacent] = tentativeCost + adjacent.ManhattanDistance(goal);

					// Add to priority queue with updated distance score
					priorityQueue.Enqueue(adjacent, distances[adjacent]);
				}
			}
		}

		return []; // No path found
	}

	private static List<Point> ReconstructPath(Dictionary<Point, Point> cameFrom, Point current)
	{
		List<Point> totalPath = [current];

		while (cameFrom.ContainsKey(current)) {
			current = cameFrom[current];
			totalPath.Insert(0, current);
		}

		return totalPath;
	}

	private static int GridSize(this object[]? args) => GetArgument(args, 1, 71);
	private static int Bytes(this object[]? args)    => GetArgument(args, 2, 1024);

	private static void VisualiseRam(this IEnumerable<Point> bytes, string title, int gridSize, IEnumerable<Point> route, bool clearScreen = false)
	{
		const char EMPTY = '.';
		const char BYTE  = '#';
		const char PATH  = 'O';

		if (_visualise is null) {
			return;
		}

		char[,] outputRamMap = new char[gridSize, gridSize];
		outputRamMap.FillInPlace(EMPTY);

		foreach (Point p in bytes ?? []) { outputRamMap[p.X, p.Y] = BYTE; }
		foreach (Point p in route ?? []) { outputRamMap[p.X, p.Y] = PATH; }

		string[] output = ["", title, .. outputRamMap.AsStrings()];
		_visualise?.Invoke(output, clearScreen);
	}
}
