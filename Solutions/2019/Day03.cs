namespace AdventOfCode.Solutions.Year2019;

/// <summary>
/// Day 03: Crossed Wires
/// https://adventofcode.com/2019/day/3
/// </summary>
[Description("Crossed Wires")]
public class Day03 {

	record WireRoute(string Direction, int Value);
	record Point(int X, int Y) {
		public int ManhattanDistance(int dX, int dY) => (Math.Abs(X - dX) + Math.Abs(Y - dY));
		public int ManhattanDistance(Point point) => (Math.Abs(X - point.X) + Math.Abs(Y - point.Y));
	};

	private static string Solution1(string[] input) {
		Point startPos = new(0, 0);

		List<WireRoute> wireInstructions1 = input[0].Split(",").Select(i => ParseLine(i)).ToList();
		List<WireRoute> wireInstructions2 = input[1].Split(",").Select(i => ParseLine(i)).ToList();

		List<Point> wire1 = new() { startPos };
		List<Point> wire2 = new() { startPos };

		foreach (var instruction in wireInstructions1) {
			wire1.AddRange(GetPoints(wire1.Last(), instruction));
		}

		foreach (var instruction in wireInstructions2) {
			wire2.AddRange(GetPoints(wire2.Last(), instruction));
		}

		return wire1
			.Intersect(wire2)
			.Where(p => p != startPos)
			.MinBy(p => p.ManhattanDistance(startPos))!
			.ManhattanDistance(startPos)
			.ToString();
	}

	static List<Point> GetPoints(Point startPoint, WireRoute route) {
		List<Point> points = new();
		for (int i = 1; i <= route.Value; i++) {
			Point point = route.Direction switch {
				"L" => startPoint with { X = startPoint.X - i },
				"R" => startPoint with { X = startPoint.X + i },
				"D" => startPoint with { Y = startPoint.Y - i },
				"U" => startPoint with { Y = startPoint.Y + i },
				_ => throw new NotImplementedException(),
			};
			points.Add(point);
		}

		return points;
	}

	private static string Solution2(string[] input) {
		Point startPos = new(0, 0);

		List<WireRoute> wireInstructions1 = input[0].Split(",").Select(i => ParseLine(i)).ToList();
		List<WireRoute> wireInstructions2 = input[1].Split(",").Select(i => ParseLine(i)).ToList();

		List<Point> wire1 = new() { startPos };
		List<Point> wire2 = new() { startPos };

		foreach (var instruction in wireInstructions1) {
			wire1.AddRange(GetPoints(wire1.Last(), instruction));
		}

		foreach (var instruction in wireInstructions2) {
			wire2.AddRange(GetPoints(wire2.Last(), instruction));
		}

		List<Point> pointsCrossOver = wire1.Intersect(wire2).Where(p => p != startPos).ToList();

		int shortestDistance = int.MaxValue;
		foreach (var point in pointsCrossOver) {
			int distance = GetDistance(wire1, wire2, point);
			if (distance < shortestDistance) {
				shortestDistance = distance;
			}
		}

		return shortestDistance.ToString();
	}

	private static int GetDistance(List<Point> wire1, List<Point> wire2, Point point) {
		return wire1.IndexOf(point) + wire2.IndexOf(point);
	}

	private static WireRoute ParseLine(string input) {
		Match match = Regex.Match(input, @"(R|L|U|D)(\d+)");
		if (match.Success) {
			return new(match.Groups[1].Value, int.Parse(match.Groups[2].Value));
		}
		throw new Exception();
	}





	#region Problem initialisation
	public static string Part1(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution1(input).ToString();
	}
	public static string Part2(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution2(input).ToString();
	}
	#endregion

}
