using static AdventOfCode.Solutions._2016.Day15Constants;
using static AdventOfCode.Solutions._2016.Day15Types;
namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 15: Timing is Everything
/// https://adventofcode.com/2016/day/15
/// </summary>
[Description("Timing is Everything")]
public sealed partial class Day15 {

	public static string Part1(string[] input) => Solution1(input).ToString();
	public static string Part2(string[] input) => Solution2(input).ToString();

	private static int Solution1(string[] input) => input.As<Disc>().FindAlignment();
	private static int Solution2(string[] input) => 
		input.As<Disc>()
		.Append(new(input.Length + 1, 11, 0))
		.FindAlignment();
}

file static class Day15Extensions
{
	public static int FindAlignment(this IEnumerable<Disc> discs)
	{
		for (int time = 0; ; time++) {
			if (discs
				.All(disc => (disc.PositionAtTimeZero + time + disc.DiscNo) % disc.NoOfPositions == 0)) {
				return time;
			}
		}
	}
}

internal sealed partial class Day15Types
{
	public sealed record Disc(int DiscNo, int NoOfPositions, int PositionAtTimeZero) : IParsable<Disc>
	{
		public readonly int OffsetToZero = (NoOfPositions - PositionAtTimeZero - DiscNo) % NoOfPositions;
		public static Disc Parse(string s, IFormatProvider? provider)
		{
			Match match = InputRegEx().Match(s);
			if (match.Success) {
				return new(
					match.As<int>("discNo"),
					match.As<int>("positions"),
					match.As<int>("position"));
			}

			return null!;
		}

		public static Disc Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Disc result)
			=> ISimpleParsable<Disc>.TryParse(s, provider, out result);
	}

	[GeneratedRegex(""""Disc \#(?<discNo>[\+\-]?\d+) has (?<positions>[\+\-]?\d+) positions; at time=0, it is at position (?<position>[\+\-]?\d+)."""")]
	public static partial Regex InputRegEx();
}

file static class Day15Constants
{
}
