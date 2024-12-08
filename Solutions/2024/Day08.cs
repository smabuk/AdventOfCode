namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 08: Resonant Collinearity
/// https://adventofcode.com/2024/day/08
/// </summary>
[Description("Resonant Collinearity")]
public static partial class Day08 {

	private static char[,] _map = default!;
	private static ILookup<char, Point> _antennae = default!;

	[Init]
	public static void LoadMap(string[] input, Action<string[], bool>? visualise = null)
	{
		_map = input.To2dArray();
		_antennae = _map
			.ForEachCell()
			.Where(cell => char.IsAsciiLetterOrDigit(cell))
			.ToLookup(cell => cell.Value, a => a.Index);

		_map.VisualiseMap([], "Initial", visualise);
	}

	public static int Part1(string[] _, Action<string[], bool>? visualise = null)
	{
		List<Point> antinodes = _antennae.GetAntinodes(a => a.Antinodes(_map, start: 1));
		_map.VisualiseMap(antinodes, "Final", visualise);

		return antinodes.Count;
	}

	public static int Part2(string[] _, Action<string[], bool>? visualise = null)
	{
		List<Point> antinodes = _antennae.GetAntinodes(a => a.Antinodes(_map, start: 0));
		_map.VisualiseMap(antinodes, "Final", visualise);

		return antinodes.Count;
	}

	private static List<Point> GetAntinodes(this ILookup<char, Point> antennae, Func<IEnumerable<Point>, IEnumerable<Point>> antinodesFunc)
	{
		return [..
			antennae
			.Select(a => antennae[a.Key])
			.Select(antinodesFunc)
			.SelectMany(antinodes => antinodes)
			.Distinct()
			];
	}

	private static IEnumerable<Point> Antinodes(this IEnumerable<Point> locations, char[,] map, int start)
	{
		foreach (Point[] pair in locations.Permute(2)) {
			for (int n = start; ; n++) {
				Point antinode = AntinodeN(pair[0], pair[1], n);
				if (map.IsInBounds(antinode)) {
					yield return antinode;
				} else {
					break;
				}

				if (start == 1) { // Part 1
					break;
				}
			}
		}
	}

	private static Point AntinodeN(Point ant1, Point ant2, int n = 1) => ((ant1 - ant2) * n) + ant1;

	// Not needed for AoC because of the way the input has been designed
	//private static Point AntinodeGcdN(Point ant1, Point ant2, int n = 1)
	//{
	//	Point gcd = new(
	//		0.GreatestCommonDivisor(ant1.X - ant2.X),
	//		0.GreatestCommonDivisor(ant1.Y - ant2.Y));
	//	return (gcd * n) + ant1;
	//}

	public static void VisualiseMap(this char[, ] map, IEnumerable<Point> antinodes, string title, Action<string[], bool>? visualise)
	{
		const char ANTINODE = '#';
		
		char[,] vMap = (char[,])map.Clone();

		foreach (Point antinode in antinodes) {
			vMap[antinode.X, antinode.Y] = ANTINODE;
		}

		if (visualise is not null) {
			string[] output = ["", title, .. vMap.AsStrings()];
			_ = Task.Run(() => visualise?.Invoke(output, false));
		}
	}
}
