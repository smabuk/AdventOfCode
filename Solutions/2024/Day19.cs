namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 19: Linen Layout
/// https://adventofcode.com/2024/day/19
/// </summary>
[Description("Linen Layout")]
public static partial class Day19 {

	private static List<string> _towelPatterns  = [];
	private static List<string> _desiredDesigns = [];

	[Init]
	public static void LoadTowels(string[] input)
	{
		_towelPatterns  = [.. input[0].TrimmedSplit(',')];
		_desiredDesigns = [.. input[2..]];
	}

	public static int Part1(string[] _)
		=> _desiredDesigns
			.Where(design => design.IsPossible(_towelPatterns))
			.Count();

	public static long Part2(string[] _)
		=> _desiredDesigns
			.Select(design => design.AllPossible(_towelPatterns, []).Sum())
			.Sum();

	public static bool IsPossible(this string design, List<string> patterns)
	{
		return design is "" ||
			patterns
			.Where(design.StartsWith)
			.Any(pattern => design[pattern.Length..].IsPossible(patterns));
	}

	public static IEnumerable<long> AllPossible(this string design, List<string> patterns, Dictionary<(string, string), long> cache)
	{
		if (design is "") {
			yield return 1;
			yield break;
		}

		foreach (string pattern in patterns.Where(design.StartsWith)) {
			if (cache.TryGetValue((design, pattern), out long cacheCount)) {
				yield return cacheCount;
				continue;
			}

			long sum = design[pattern.Length..].AllPossible(patterns, cache).Sum();
			cache.Add((design, pattern), sum);
			yield return sum;
		}
	}
}
