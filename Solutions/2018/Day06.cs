namespace AdventOfCode.Solutions._2018;

/// <summary>
/// Day 06: Chronal Coordinates
/// https://adventofcode.com/2018/day/06
/// </summary>
[Description("Chronal Coordinates")]
public sealed partial class Day06
{

	[Init]
	public static void    Init(string[] input, params object[]? _) => LoadCoordinates(input);
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args)
	{
		int distance = GetArgument<int>(args, argumentNumber: 1, 10_000);
		return Solution2(input, distance).ToString();
	}

	private static IEnumerable<Coordinate> _coordinates = [];
	private static int _minX = int.MaxValue;
	private static int _minY = int.MaxValue;
	private static int _maxX = int.MinValue;
	private static int _maxY = int.MinValue;

	private static void LoadCoordinates(string[] input)
	{
		_coordinates = input.AsPoints().Select(p => new Coordinate(p));
		foreach (Coordinate coord in _coordinates) {
			if (coord.Coord.X < _minX) {
				_minX = coord.Coord.X;
			}
			if (coord.Coord.Y < _minY) {
				_minY = coord.Coord.Y;
			}
			if (coord.Coord.X > _maxX) {
				_maxX = coord.Coord.X;
			}
			if (coord.Coord.Y > _maxY) {
				_maxY = coord.Coord.Y;
			}
		}
	}

	private static int Solution1(string[] _)
	{
		List<Coordinate> closestList = [];

		for (int y = _minY; y <= _maxY; y++) {
			for (int x = _minX; x <= _maxX; x++) {
				int minDistance = _coordinates
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
			.Where(c => c.Coord.X <= _minX || c.Coord.Y <= _minY || c.Coord.X >= _maxX || c.Coord.Y >= _maxY)
			.Select(c => c.Name)
			.Distinct()
			.ToList();

		return closestList
			.Where(c => !ignore.Contains(c.Name))
			.GroupBy(c => c.Name)
			.Select(g => (Name: g.Key, Area: g.Count()))
			.MaxBy(c => c.Area)
			!.Area;
	}

	private static int Solution2(string[] _, int distance)
	{
		int safeCount = 0;

		for (int y = _minY; y <= _maxY; y++) {
			for (int x = _minX; x <= _maxX; x++) {
				int totalDistance = _coordinates
					.Sum(c => c.ManhattanDistance(x, y));
				if (totalDistance < distance) {
					safeCount++;
				}
			}
		}
		return safeCount;
	}

	private record Coordinate(Point Coord)
	{
		public string Name { get; init; } = Coord.ToString();
		public int ManhattanDistance(int x, int y) => Math.Abs(x - Coord.X) + Math.Abs(y - Coord.Y);
	}
}
