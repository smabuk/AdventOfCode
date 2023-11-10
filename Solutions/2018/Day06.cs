namespace AdventOfCode.Solutions._2018;

/// <summary>
/// Day 06: Chronal Coordinates
/// https://adventofcode.com/2018/day/06
/// </summary>
[Description("Chronal Coordinates")]
public sealed partial class Day06 {

	[Init]
	public static    void Init(string[] input, params object[]? _) => LoadCoordinates(input);
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static IEnumerable<Coordinate> _coordinates = [];

	private static void LoadCoordinates(string[] input) {
		_coordinates = input.AsPoints().Select(p => new Coordinate(p));
	}

	private static int Solution1(string[] _) {
		List<Coordinate> closestList = [];

		int minX = int.MaxValue, minY = int.MaxValue;
		int maxX = int.MinValue, maxY = int.MinValue;
		foreach (Coordinate coord in _coordinates) {
			if (coord.Coord.X < minX) {
				minX = coord.Coord.X;
			}
			if (coord.Coord.Y < minY) {
				minY = coord.Coord.Y;
			}
			if (coord.Coord.X > maxX) {
				maxX = coord.Coord.X;
			}
			if (coord.Coord.Y > maxY) {
				maxY = coord.Coord.Y;
			}
		}

		for (int y = minY; y <= maxY; y++) {
			for (int x = minX; x <= maxX; x++) {
				int minDistance= _coordinates
					.Select(c => c.ManhattanDistance(x, y))
					.Min();
				List<Coordinate> closest = _coordinates
					.Where(c => c.ManhattanDistance(x, y) == minDistance)
					.ToList();
				if (closest.Count == 1) {
					closestList.Add(new(new(x, y)) { Name = closest[0].Name });
				}
			}
		}

		List<string> ignore = closestList
			.Where(c => c.Coord.X <= minX || c.Coord.Y <= minY || c.Coord.X >= maxX || c.Coord.Y >= maxY)
			.Select(c => c.Name)
			.Distinct()
			.ToList();

		return closestList
			.Where(c => !ignore.Contains(c.Name))
			.GroupBy(c => c.Name)
			.Select(g => new
				{
					Name = g.Key,
					Area = g.Count()
				})
			.MaxBy(c => c.Area)
			!.Area;
	}

	private static string Solution2(string[] input) {
		//string inputLine = input[0];
		//List<string> inputs = input.ToList();
		//List<Instruction> instructions = input.Select(ParseLine).ToList();
		return "** Solution not written yet **";
	}

	private record Coordinate(Point Coord) {
		public string Name { get; init; } = Coord.ToString();
		public int ManhattanDistance(int x, int y) => Math.Abs(x - Coord.X) + Math.Abs(y - Coord.Y);
	}

	//static int ManhattanDistance(Point point, int x, int y) => Math.Abs(x - point.X) + Math.Abs(y - point.Y);

}
