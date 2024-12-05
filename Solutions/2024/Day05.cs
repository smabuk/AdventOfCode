using static AdventOfCode.Solutions._2024.Day05Constants;
using static AdventOfCode.Solutions._2024.Day05Types;
namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 05: Print Queue
/// https://adventofcode.com/2024/day/05
/// </summary>
[Description("Print Queue")]
public sealed partial class Day05 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static IEnumerable<Rule> _rules = [];
	private static Dictionary<int, Rule> _lookup = [];

	[Init]
	public static void LoadRules(string[] input)
	{
		_rules = [.. input.TakeWhile(i => !string.IsNullOrWhiteSpace(i)).As<Rule>()];
		_lookup = _rules.ToDictionary(r => r.Page1);
	}

	private static int Solution1(string[] input)
	{
		List<IEnumerable<int>> result = [
			..input
			.SkipWhile(i => !string.IsNullOrWhiteSpace(i))
			.Skip(1)
			.Select(i => i.As<int>(separator: ','))];

		return result
			.Where(pages => pages.ObeysTheRules(_rules))
			.Sum(pages => pages.Middle());
	}

	private static string Solution2(string[] input) => NO_SOLUTION_WRITTEN_MESSAGE;
}

file static class Day05Extensions
{
	public static bool ObeysTheRules(this IEnumerable<int> pages, IEnumerable<Rule> rules)
	{
		List<int> pageSet = [.. pages];
		for (int i = 0; i < pageSet.Count; i++) {
			if (rules.Any(r => r.Page1 == pageSet[i] && pageSet[..i].Contains(r.Page2))) {
				return false;
			}
		}

		return true;
	}

	public static int Middle(this IEnumerable<int> pages)
	{
		List<int> pageSet = [.. pages];
		return pageSet[pageSet.Count() / 2];
	}
}

internal sealed partial class Day05Types
{

	public sealed record Rule(int Page1, int Page2) : IParsable<Rule>
	{
		public static Rule Parse(string s, IFormatProvider? provider)
		{
			int[] tokens = [..s.As<int>(separator: '|')];
			return new(tokens[0], tokens[1]);
		}

		public static Rule Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Rule result)
			=> ISimpleParsable<Rule>.TryParse(s, provider, out result);
	}
}

file static class Day05Constants
{
}
