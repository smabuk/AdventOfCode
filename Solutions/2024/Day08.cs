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
			.Where(c => char.IsAsciiLetterOrDigit(c))
			.ToLookup(a => a.Value, a => a.Index);
		_map.VisualiseMap([], "Initial", visualise);
	}

	public static int Part1(string[] _, Action<string[], bool>? visualise = null)
	{
		List<Point> antinodes = _antennae.GetAntinodes(Antinodes);
		_map.VisualiseMap(antinodes, "Final", visualise);

		return antinodes.Count;
	}

	public static int Part2(string[] _, Action<string[], bool>? visualise = null)
	{
		List<Point> antinodes = _antennae.GetAntinodes(a => a.ResidentHarmonicAntinodes(_map));
		_map.VisualiseMap(antinodes, "Final", visualise);

		return antinodes.Count;
	}

	private static List<Point> GetAntinodes(this ILookup<char, Point> antennae, Func<IEnumerable<Point>, IEnumerable<Point>> antinodesFunc)
	{
		return [..antennae
			.Select(a => antennae[a.Key])
			.Select(antinodesFunc)
			.SelectMany(antinodes => antinodes)
			.Distinct()
			.Where(antinode => _map.IsInBounds(antinode))];
	}

	private static IEnumerable<Point> Antinodes(this IEnumerable<Point> locations)
	{
		foreach (IEnumerable<Point> item in locations.Combinations(2)) {
			Point a1 = item.First();
			Point a2 = item.Last();
			yield return a1 - a2 + a1;
			yield return a2 - a1 + a2;
		}
	}

	private static IEnumerable<Point> ResidentHarmonicAntinodes(this IEnumerable<Point> locations, char[,] map)
	{
		foreach (IEnumerable<Point> item in locations.Combinations(2)) {
			Point a1 = item.First();
			Point a2 = item.Last();
			int i = 0;
			while (map.IsInBounds(((a1 - a2) * i) + a1)) {
				yield return ((a1 - a2) * i) + a1;
				i++;
			}

			i = 0;
			while (map.IsInBounds(((a2 - a1) * i) + a2)) {
				yield return ((a2 - a1) * i) + a2;
				i++;
			}
		}
	}


	public static void VisualiseMap(this char[, ] map, IEnumerable<Point> antinodes, string title, Action<string[], bool>? visualise)
	{
		char[,] copy = (char[,])map.Clone();

		foreach (Point antinode in antinodes) {
			copy[antinode.X, antinode.Y] = ANTINODE;
		}

		if (visualise is not null) {
			string[] output = ["", title, .. copy.PrintAsStringArray(0)];
			_ = Task.Run(() => visualise?.Invoke(output, false));
		}
	}

	private const char ANTINODE = '#';
}
