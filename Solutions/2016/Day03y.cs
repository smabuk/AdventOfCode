namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 03: Squares With Three Sides
/// https://adventofcode.com/2016/day/03
/// </summary>
[Description("Squares With Three Sides")]
public sealed partial class Day03 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();


	private static int Solution1(string[] input) {
		return input
			.As<Triangle>()
			.Count(IsValidTriangle);
	}

	private static int Solution2(string[] input) {
		return input
			.Select(i => i.TrimmedSplit(' ').As<int>().ToList())
			.To2dArray()
			.Cast<int>()
			.Chunk(3)
			.Select(chunk => new Triangle(chunk[0], chunk[1], chunk[2]))
			.Count(IsValidTriangle);
	}

	private static bool IsValidTriangle(Triangle t) =>
		   t.SideLength1 + t.SideLength2 > t.SideLength3
		&& t.SideLength1 + t.SideLength3 > t.SideLength2
		&& t.SideLength2 + t.SideLength3 > t.SideLength1;


	private sealed record Triangle(int SideLength1, int SideLength2, int SideLength3) : IParsable<Triangle> {
		public static Triangle Parse(string s, IFormatProvider? provider)
		{
			string[] tokens = s.TrimmedSplit(' ');
			return new(tokens[0].As<int>(), tokens[1].As<int>(), tokens[2].As<int>());
		}

		public static Triangle Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Triangle result)
			=> ISimpleParsable<Triangle>.TryParse(s, provider, out result);
	}
}
