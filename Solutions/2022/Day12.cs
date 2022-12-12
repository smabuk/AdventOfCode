using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 12: Hill Climbing Algorithm
/// https://adventofcode.com/2022/day/12
/// </summary>
[Description("Hill Climbing Algorithm")]
public sealed partial class Day12 {

	[Init]
	public static void    Init(string[] input, params object[]? _) => LoadElevationMap(input);
	public static string Part1(string[] input, params object[]? _) => Solution1().ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2().ToString();

	private static int[,] _elevationMap = default!;
	private static Dictionary<Point, int> _costs = new();
	private static Point _startPoint;
	private static Point _endPoint;

	private static void LoadElevationMap(string[] input) {
		_elevationMap = input
			.SelectMany(i => i)
			.Select(x => (int)x)
			.To2dArray(input[0].Length);

		foreach (Cell<int> c in _elevationMap.Walk2dArrayWithValues()) {
			if (c.Value == 'S') {
				_startPoint = c.Index;
				_elevationMap[c.X, c.Y] = 'a';
			}
			if (c.Value == 'E') {
				_endPoint = c.Index;
				_elevationMap[c.X, c.Y] = 'z';
			}
		}

		_costs = new() {
			{ _endPoint, 0 }
		};

		CalculateCost(_endPoint);
	}

	private static int Solution1() => _costs[_startPoint];


	private static int Solution2() =>
		_elevationMap
			.Walk2dArrayWithValues()
			.Where(x => x.Value == 'a' && _costs.ContainsKey(x.Index))
			.Select(x => _costs[x.Index] -_costs[_endPoint])
			.Min();


	private static void CalculateCost(Point p) {
		int currentCost = _costs[p];

		_elevationMap
			.GetAdjacentCells(p)
			.ToList()
			.ForEach(adj => CalculateAdjacent(new(adj.x, adj.y)));

		void CalculateAdjacent(Point adjacent) {
			// Is this step not too steep - we're going backwards so we can work with parts 1 and 2
			if (_elevationMap[p.X, p.Y] - 1 <= _elevationMap[adjacent.X, adjacent.Y]) {
				if (!_costs.TryGetValue(adjacent, out int adjacentCost) || adjacentCost > currentCost + 1) {
					_costs[adjacent] = currentCost + 1;
					CalculateCost(adjacent);
				}
			}
		}
	}

}
