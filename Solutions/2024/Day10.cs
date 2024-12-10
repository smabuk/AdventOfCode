namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 10: Hoof It
/// https://adventofcode.com/2024/day/10
/// </summary>
[Description("Hoof It")]
public static partial class Day10
{

	private static List<Trail> _allRoutes = [];

	[Init]
	public static void FindAllTheTrails(string[] input)
	{
		int[,] lavaMap = input.Select(i => i.AsHeights()).ToList().To2dArray();

		_allRoutes = [..
			lavaMap
				.ForEachCell()
				.Where(location => location.Value is TRAIL_HEAD)
				.SelectMany(th => lavaMap
					.HikeToSummit(new(TRAIL_HEAD, TRAIL_HEAD, th.Index, th.Index, [th.Index])))
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
		foreach (Trail nextTrail in map.NextPositions(trail)) {
			if (nextTrail.End is SUMMIT) {
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

	private static IEnumerable<Trail> NextPositions(this int[,] map, Trail trail)
		=> map
			.GetAdjacentCells(trail.EndPoint)
			.Where(next => next.Value == trail.End + 1)
			.Select(next => trail with { EndPoint = next.Index, End = next.Value, Route = [.. trail.Route, next] });

	private static List<int> AsHeights(this string s) => [.. s.Select(x => char.IsAsciiDigit(x) ? int.Parse($"{x}", null) : int.MinValue)];

	public record Trail(int Start, int End, Point StartPoint, Point EndPoint, List<Point> Route);

	private const int TRAIL_HEAD = 0;
	private const int SUMMIT     = 9;
}
