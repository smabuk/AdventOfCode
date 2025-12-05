using static AdventOfCode.Solutions._2025.Day05;

namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 05: Cafeteria
/// https://adventofcode.com/2025/day/05
/// </summary>
[Description("Cafeteria")]
[GenerateVisualiser]
public partial class Day05 {

	[Init]
	public static void LoadDatabase(string[] input)
	{
		_freshIdRanges = [.. input
			.TakeWhile(i => i.HasNonWhiteSpaceContent())
			.Select(i => i.TrimmedSplit('-'))
			.Select(r => new LongRange(r[0].As<long>(), r[1].As<long>()))
			];

		_ingredients = [.. input[(_freshIdRanges.Count + 1)..].As<Ingredient>()];
	}

	internal static List<LongRange> _freshIdRanges = [];
	private static List<Ingredient> _ingredients = [];

	public static int Part1() => _ingredients.Count(ingredient => ingredient.IsFresh());

	public static string Part2() => NO_SOLUTION_WRITTEN_MESSAGE;

	[GenerateIParsable]
	internal sealed partial record Ingredient(long Id)
	{
		public static Ingredient Parse(string s) => new(s.As<long>());
	}
}

file static class Day05Helpers
{
	extension(Ingredient ingredient)
	{
		public bool IsFresh() => _freshIdRanges.Any(ingredient.IsInRange);

		public bool IsInRange(LongRange range) => ingredient.Id >= range.Start && ingredient.Id <= range.End;
	}
}
