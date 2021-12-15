namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 15: Chiton
/// https://adventofcode.com/2021/day/15
/// </summary>
[Description("Chiton")]
public class Day15 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		int[,] grid = input.SelectMany(i => i.AsDigits()).To2dArray(input[0].Length);

		Point start = new(0, 0);
		Point end = new(new(grid.GetUpperBound(0), grid.GetUpperBound(1)));

		// https://en.wikipedia.org/wiki/Dijkstra's_algorithm

		PriorityQueue<Cell<int>, int> priorityQueue = new();
		priorityQueue.Enqueue(new(start, 0), 0);
		Dictionary<Point, int> costs = new();
		costs.Add(start, 0);

		while (priorityQueue.Count > 0) {
			Cell<int> cell = priorityQueue.Dequeue();

			foreach ((int x, int y, int value) in grid.GetAdjacentCells(cell.Index)) {
				Cell<int> neighbour = new(x, y, value);
				if (!costs.ContainsKey(neighbour.Index)) {
					int cost = costs[cell.Index] + neighbour.Value;
					costs[neighbour.Index] = cost;
					if (neighbour.Index == end) {
						break;
					}
					priorityQueue.Enqueue(neighbour, cost);
				}
			}
		}

		return costs[end];
	}

	private static int Solution2(string[] input) {
		int[,] inputGrid = input.SelectMany(i => i.AsDigits()).To2dArray(input[0].Length);

		Point start = new(0, 0);
		int inputGridWidth = inputGrid.GetLength(0);
		int inputGridHeight = inputGrid.GetLength(1);


		int[,] grid = new int[inputGridWidth * 5, inputGridHeight * 5];

		for (int y = 0; y < 5; y++) {
			int yOffset = y * inputGridHeight;
			for (int x = 0; x < 5; x++) {
				int xOffset = x * inputGridWidth;
				foreach (Cell<int> cell in inputGrid.Walk2dArrayWithValues()) {
					int value = (cell.Value + x + y) switch {
						>= 20 => (cell.Value + x + y) - 18,
						>= 10 => (cell.Value + x + y) - 9,
						_ => (cell.Value + x + y),
					};
					grid[cell.X + xOffset, cell.Y + yOffset] = value;
				}
			}
		}

		Point end = new(new(grid.GetUpperBound(0), grid.GetUpperBound(1)));

		// https://en.wikipedia.org/wiki/Dijkstra's_algorithm

		PriorityQueue<Cell<int>, int> priorityQueue = new();
		priorityQueue.Enqueue(new(start, 0), 0);
		Dictionary<Point, int> costs = new();
		costs.Add(start, 0);

		while (priorityQueue.Count > 0) {
			Cell<int> cell = priorityQueue.Dequeue();

			foreach ((int x, int y, int value) in grid.GetAdjacentCells(cell.Index)) {
				Cell<int> neighbour = new(x, y, value);
				if (!costs.ContainsKey(neighbour.Index)) {
					int cost = costs[cell.Index] + neighbour.Value;
					costs[neighbour.Index] = cost;
					if (neighbour.Index == end) {
						break;
					}
					priorityQueue.Enqueue(neighbour, cost);
				}
			}
		}

		return costs[end];
	}
}
