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

	private static char[,] _elevationMap = default!;
	private static Dictionary<Point, int> _costs = [];
	private static Dictionary<char, Point> _endPoints = [];
	private static Point _startPoint;
	private static Point _endPoint;

	private static void LoadElevationMap(string[] input) {
		_elevationMap = input
			.SelectMany(i => i)
			.To2dArray(input[0].Length);

		_endPoints = _elevationMap
			.ForEachCell()
			.Where(c => c.Value is 'S' or 'E')
			.ToDictionary(c => c.Value, c => c.Index);

		_startPoint = _endPoints['S'];
		_endPoint   = _endPoints['E'];

		_costs = new() {
			{ _endPoint, 0 }
		};

		CalculateCost(_endPoint);
	}

	private static int Solution1() => _costs[_startPoint];
	private static int Solution2() =>
		_elevationMap
			.ForEachCell()
			.Where(x => x.Value == 'a' && _costs.ContainsKey(x.Index))
			.Select(x => _costs[x.Index] -_costs[_endPoint])
			.Min();


	private static void CalculateCost(Point point) {
		int currentCost = _costs[point];

		_elevationMap
			.GetAdjacentCells(point)
			.ToList()
			.ForEach(adj => CalculateAdjacent(adj.Index));

		void CalculateAdjacent(Point adjacent) {
			if (IsThisACorrectRoute(point, adjacent)) {
				if (!_costs.TryGetValue(adjacent, out int adjacentCost) || adjacentCost > currentCost + 1) {
					_costs[adjacent] = currentCost + 1;
					CalculateCost(adjacent);
				}
			}

			// if the step uphill is too steep we return false
			//    we're going downhill so we can work with parts 1 and 2
			static bool IsThisACorrectRoute(Point p, Point adjacent) 
				=> ElevationValue(_elevationMap[p.X, p.Y]) - 1 <= ElevationValue(_elevationMap[adjacent.X, adjacent.Y]);
		}

		static char ElevationValue(char c) => c switch {
			'S' => 'a',
			'E' => 'z',
			_ => c
		};
	}

}
