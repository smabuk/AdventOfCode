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
		int size  = args.MemorySpaceSize();
		int noOfBytes = args.NoOfBytes();

		Point start = Point.Zero;
		Point end   = new(size - 1, size - 1);

		_bytes.Take(noOfBytes).VisualiseRam("Initial state:", size, []);

		List<Point> shortestPath = FindShortestPath(start, end, _bytes.Take(noOfBytes), size);

		_bytes.Take(noOfBytes).VisualiseRam("Final:", size, shortestPath);

		return shortestPath.Count - 1; // shortestPath includes start
	}

	public static string Part2(string[] _, params object[]? args)
	{
		int size  = args.MemorySpaceSize();
		int noOfBytes = _bytes.Count;

		Point start = Point.Zero;
		Point end   = new(size - 1, size - 1);
		
		List<Point> shortestPath = [];
		while (shortestPath is []) {
			shortestPath = FindShortestPath(start, end, _bytes.Take(--noOfBytes), size);
		}

		return $"{_bytes[noOfBytes].X},{_bytes[noOfBytes].Y}";
	}

	public static List<Point> FindShortestPath(Point start, Point goal, IEnumerable<Point> bytes, int size)
	{
		Dictionary<Point, int>    cost          = new() { [start] = 0 };
		Dictionary<Point, int>    distances     = new() { [start] = start.ManhattanDistance(goal) };
		Dictionary<Point, Point>  previous      = [];
		HashSet<Point>            corruptions   = [.. bytes];

		PriorityQueue<Point, int> priorityQueue = new();
		priorityQueue.Enqueue(start, distances[start]);

		while (priorityQueue.Count > 0) {
			Point current = priorityQueue.Dequeue();

			if (current == goal) {
				return ReconstructPath(previous, current);
			}

			foreach (Direction direction in Directions.NSEW) {
				Point adjacent = current + direction.Delta();

				if (adjacent.IsOutOfBounds(size) || corruptions.Contains(adjacent)) {
					continue;
				}

				int tentativeCost = cost[current] + 1;

				if (tentativeCost < cost.GetValueOrDefault(adjacent, int.MaxValue)) {
					previous[adjacent] = current;
					cost[adjacent] = tentativeCost;
					distances[adjacent] = tentativeCost + adjacent.ManhattanDistance(goal);
					priorityQueue.Enqueue(adjacent, distances[adjacent]);
				}
			}
		}

		return []; // No path found
	}

	private static bool IsOutOfBounds(this Point adjacent, int gridSize)
		=> adjacent.X < 0 || adjacent.X >= gridSize
		|| adjacent.Y < 0 || adjacent.Y >= gridSize;

	private static List<Point> ReconstructPath(Dictionary<Point, Point> previous, Point current)
	{
		List<Point> fullPath = [current];

		while (previous.ContainsKey(current)) {
			current = previous[current];
			fullPath = [current, .. fullPath];
		}

		return fullPath;
	}

	private static int MemorySpaceSize(this object[]? args) => GetArgument(args, 1, 71);
	private static int NoOfBytes(this object[]? args)       => GetArgument(args, 2, 1024);

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
