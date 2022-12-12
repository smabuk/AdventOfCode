using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 12: Hill Climbing Algorithm
/// https://adventofcode.com/2022/day/12
/// </summary>
[Description("Hill Climbing Algorithm")]
public sealed partial class Day12 {

	[Init]
	public static void Init(string[] input, params object[]? _) => LoadInstructions(input);
	public static string Part1(string[] input, params object[]? _) => Solution1().ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int[,] _elevationMap;
	private static Point _startPoint;
	private static Point _endPoint;
	private static Dictionary<Point, int> _costs = new();

	private static void LoadInstructions(string[] input) {
		_elevationMap = input.SelectMany(i => i).Select(x => x - 'a').To2dArray(input[0].Length);
		foreach (var c in _elevationMap.Walk2dArrayWithValues()) {
			if (c.Value == -14) {
				_startPoint = new(c.X, c.Y);
				_elevationMap[c.X, c.Y] = 'a' - 'a';
			}
			if (c.Value == -28) {
				_endPoint = new(c.X, c.Y);
				_elevationMap[c.X, c.Y] = 'z' - 'a';
			}
		}
		_costs = new() { { _startPoint, 0 } };
		ComputeCost(_startPoint);

		void ComputeCost(Point p) {
			int currentCost = _costs[p];

			void ComputeNeighbor(Point neighbour) {
				if (neighbour.X < 0 || neighbour.X >= _elevationMap.NoOfColumns() || neighbour.Y < 0 || neighbour.Y >= _elevationMap.NoOfRows())
					return;

				if (_elevationMap[neighbour.X, neighbour.Y] <= _elevationMap[p.X, p.Y] + 1) {
					if (!_costs.TryGetValue((neighbour), out int currentNeighborCost) || currentNeighborCost > currentCost + 1) {
						_costs[neighbour] = currentCost + 1;
						ComputeCost(neighbour);
					}
				}
			}

			ComputeNeighbor(p + (-1, 0));
			ComputeNeighbor(p + (+1, 0));
			ComputeNeighbor(p + (0, -1));
			ComputeNeighbor(p + (0,+1));
		}
	}

	private static int Solution1() {
		int noOfSteps =  _costs[_endPoint];
		return noOfSteps;
	}


	private static int Solution2(string[] input) {
		return 0;
	}

}
