namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 03: Squares With Three Sides
/// https://adventofcode.com/2016/day/03
/// </summary>
[Description("Squares With Three Sides")]
public sealed partial class Day03 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadTriangles(input);
	public static string Part1(string[] input, params object[]? args) => Solution1().ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static List<Triangle> _triangles = [];

	private static void LoadTriangles(string[] input) => _triangles = [.. input.As<Triangle>()];

	private static int Solution1() {
		return _triangles
			.Count(t =>
				   t.SideLength1 + t.SideLength2 > t.SideLength3 
				&& t.SideLength1 + t.SideLength3 > t.SideLength2 
				&& t.SideLength2 + t.SideLength3 > t.SideLength1 
			);
	}

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}

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
