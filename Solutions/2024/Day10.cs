namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 10: Hoof It
/// https://adventofcode.com/2024/day/10
/// </summary>
[Description("Hoof It")]
public static partial class Day10 {

	private static List<Trail> _allRoutes = [];

	[Init]
	public static void FindAllTheTrails(string[] input)
	{
		int[,] lavaMap = input.Select(i => i.AsHeights()).ToList().To2dArray();

		_allRoutes = [..
			lavaMap
				.ForEachCell()
				.Where(location => location.Value is 0)
				.SelectMany(th => lavaMap
					.HikeToSummit(new(0, 0, th.Index, th.Index, [th.Index])))
				];
	}

	public static int Part1(string[] _)
	{
		return _allRoutes
				.Select(trail => (trail.StartPoint, trail.EndPoint))
				.Distinct()
				.Count();
	}

	public static int Part2(string[] _) => _allRoutes.Count;

	private static IEnumerable<Trail> HikeToSummit(this int[,] map, Trail trail)
	{
		foreach (Direction direction in Directions.AllDirections) {
			Point nextPoint = trail.EndPoint + direction.Delta();
			if (map.TryGetValue(nextPoint, out int height)) {
				if (height == trail.End + 1) {
					Trail nextTrail = trail with { EndPoint = nextPoint, End = height, Route = [..trail.Route, nextPoint] };
					if (height is SUMMIT) {
						yield return nextTrail;
						continue;
					}

					List<Trail> newTrails = [.. map.HikeToSummit(nextTrail)];
					foreach (Trail newTrail in newTrails) {
						if (newTrail.End is SUMMIT) {
							yield return newTrail;
						}
					}
				}
			}
		}
	}

	private static List<int> AsHeights(this string s) => [..s.Select(x => char.IsAsciiDigit(x) ? int.Parse($"{x}", null) : int.MinValue)];

	public record Trail(int Start, int End, Point StartPoint, Point EndPoint, List<Point> Route);

	private const int SUMMIT = 9;
}
