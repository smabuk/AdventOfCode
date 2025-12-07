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
		int[,] lavaMap = input.Select(i => i.AsHeights()).To2dArray();

		_allRoutes = [..
			lavaMap
				.ForEachCell()
				.Where(location => location.Value is TRAIL_HEAD)
				.SelectMany(th => lavaMap.HikeToSummit(th.CreateTrailHead()))
			];
	}

	public static int Part1(string[] _) => _allRoutes.SumOfRatings();
	public static int Part2(string[] _) => _allRoutes.SumOfScores();


	private static IEnumerable<Trail> HikeToSummit(this int[,] map, Trail trail)
	{
		foreach (Trail nextTrail in map.NextPositions(trail)) {
			if (nextTrail.ReachedTheSummit()) {
				yield return nextTrail;
				continue;
			}

			foreach (Trail completedTrail in map.HikeToSummit(nextTrail).ReachedTheSummit()) {
				yield return completedTrail;
			}
		}
	}



	private static IEnumerable<int> AsHeights(this string s)
		=> [.. s.Select(x => char.IsAsciiDigit(x) ? int.Parse($"{x}") : int.MinValue)];

	private static Trail CreateTrailHead(this Cell<int> th)
		=> new (TRAIL_HEAD, TRAIL_HEAD, th.Index, th.Index, [th.Index]);

	private static IEnumerable<Trail> NextPositions(this int[,] map, Trail trail)
		=> map
			.GetAdjacentsAsCells(trail.EndPoint)
			.Where(next => next.Value == trail.End + 1)
			.Select(next => trail with { EndPoint = next.Index, End = next.Value, Route = [.. trail.Route, next] });
	
	private static bool ReachedTheSummit(this Trail trail) => trail.End is SUMMIT;
	private static IEnumerable<Trail> ReachedTheSummit(this IEnumerable<Trail> trails) => trails.Where(ReachedTheSummit);

	private static int SumOfRatings(this List<Trail> trails) => trails.DistinctBy(t => (t.StartPoint, t.EndPoint)).Count();
	private static int SumOfScores(this List<Trail> trails) => trails.Count;


	public record Trail(int Start, int End, Point StartPoint, Point EndPoint, List<Point> Route);

	private const int TRAIL_HEAD = 0;
	private const int SUMMIT     = 9;
}
