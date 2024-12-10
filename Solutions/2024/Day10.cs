namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 10: Hoof It
/// https://adventofcode.com/2024/day/10
/// </summary>
[Description("Hoof It")]
public static partial class Day10 {

	private static int[,] _lavaMap = default!;

	[Init]
	public static void LoadLavaMap(string[] input)
	{
		char[,] lavaMapTemp = input.Select(i => i).To2dArray();
		_lavaMap = new int[lavaMapTemp.ColsCount(), lavaMapTemp.RowsCount()];

		foreach (Cell<char> cell in lavaMapTemp.ForEachCell()) {
			if (char.IsAsciiDigit(cell)) {
				_lavaMap[cell.Col, cell.Row] = cell.Value.ToString().As<int>();
			} else {
				_lavaMap[cell.Col, cell.Row] = int.MinValue;
			}
		}

	}

	public static int Part1(string[] _)
	{
		var result = _lavaMap.
				ForEachCell()
				.Where(location => location.Value is 0)
				.SelectMany(th => _lavaMap
					.HikeToSummit(new(0, 0, th.Index, th.Index, [th.Index]), Directions.AllDirections))
				.ToList()
				.Where(trail => trail.End == SUMMIT)
				.Select(trail => (trail.StartPoint, trail.EndPoint))
				.Distinct()
				.Count()
				;

		return result;
	}

	private static List<Trail> HikeToSummit(this int[,] map, Trail trail, IEnumerable<Direction> allDirections)
	{
		List<Trail> trails = [];

		foreach (Direction direction in allDirections) {
			Point nextPoint = trail.EndPoint + direction.Delta();
			if (map.TryGetValue(nextPoint, out int height)) {
				if (height == trail.End + 1) {
					Trail nextTrail = trail with { EndPoint = nextPoint, End = height };
					if (height is SUMMIT) {
						trails.Add(nextTrail);
						continue;
					} else {
						List<Trail> newTrails = [.. map.HikeToSummit(nextTrail, allDirections)];
						foreach (Trail newTrail in newTrails) {
							if (newTrail.End is SUMMIT) {
								trails.Add( newTrail);
							}
						}
					}
				}
			}
		}

		return trails;

	}

	public static string Part2(string[] input, params object[]? args) => NO_SOLUTION_WRITTEN_MESSAGE;

	public record Trail(int Start, int End, Point StartPoint, Point EndPoint, List<Point> Route);

	private const int SUMMIT = 9;
}
