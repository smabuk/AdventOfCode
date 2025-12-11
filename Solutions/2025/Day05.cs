using static AdventOfCode.Solutions._2025.Day05;

namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 05: Cafeteria
/// https://adventofcode.com/2025/day/05
/// </summary>
[Description("Cafeteria")]
public partial class Day05
{
	private static List<FreshRange> _freshIdRanges = [];
	private static List<Ingredient> _ingredients = [];

	[Init] public static void LoadDatabase(string[] input)
	{
		_freshIdRanges = [.. input
			.TakeWhile(i => i.HasNonWhiteSpaceContent())
			.As<FreshRange>()
			.MergeOverlapping()
			];

		_ingredients = [.. input
			.SkipWhile(i => i.HasNonWhiteSpaceContent())
			.Skip(1)
			.As<Ingredient>()];
	}

	public static  int Part1() => _ingredients.Count(IsFresh);
	public static long Part2() => _freshIdRanges.Sum(Length);


	static bool IsFresh(Ingredient ingredient) => _freshIdRanges.Any(ingredient.IsInRange);
	static long Length(FreshRange range) => range.Length;


	[GenerateIParsable(SplitChars = "-")] internal sealed partial record FreshRange(long Start, long End);
	[GenerateIParsable] internal sealed partial record Ingredient(long Id);

}

file static class Day05Helpers
{
	extension(Ingredient ingredient)
	{
		public bool IsInRange(FreshRange range) => ingredient.Id >= range.Start && ingredient.Id <= range.End;
	}

	extension(FreshRange range)
	{
		public long Length => range.End - range.Start + 1;
	}

	extension(IEnumerable<FreshRange> ranges)
	{
		public List<FreshRange> MergeOverlapping()
		{
			List<FreshRange> mergedRanges = [];
			foreach (FreshRange range in ranges.OrderBy(r => r.Start)) {
				if (mergedRanges.Count == 0) {
					mergedRanges.Add(range);
					continue;
				}

				FreshRange lastRange = mergedRanges[^1];
				if (range.Start <= lastRange.End + 1) {
					mergedRanges[^1] = new FreshRange(lastRange.Start, long.Max(lastRange.End, range.End));
				} else {
					mergedRanges.Add(range);
				}
			}

			return mergedRanges;
		}
	}
}
