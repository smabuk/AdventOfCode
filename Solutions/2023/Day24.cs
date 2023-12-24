namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 24: Never Tell Me The Odds
/// https://adventofcode.com/2023/day/24
/// </summary>
[Description("Never Tell Me The Odds")]
public sealed partial class Day24 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadHailstones(input);
	public static string Part1(string[] input, params object[]? args)
	{
		long targetMin = GetArgument(args, argumentNumber: 1, defaultResult: 200_000_000_000_000);
		long targetMax = GetArgument(args, argumentNumber: 2, defaultResult: 400_000_000_000_000);
		return Solution1(targetMin, targetMax).ToString();
	}

	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static IEnumerable<Hailstone> _hailstones = [];

	private static void LoadHailstones(string[] input) {
		_hailstones = input.As<Hailstone>();
	}

	private static int Solution1(long targetMin, long targetMax) {
		int noOfIntersections = _hailstones
			.Combinations(2)
			.Count(pair => WillTheyCollideInsideTestArea(pair, targetMin, targetMax));
		return noOfIntersections;
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}

	private static bool WillTheyCollideInsideTestArea(IEnumerable<Hailstone> hailstones, long targetMin, long targetMax) {
		Hailstone h0 = hailstones.First();
		Hailstone h1 = hailstones.Last();

		decimal t0ToMin = (targetMin - h0.X) / h0.VX;
		decimal t0ToMax = (targetMax - h0.X) / h0.VX;
		decimal t1ToMin = (targetMin - h1.X) / h1.VX;
		decimal t1ToMax = (targetMax - h1.X) / h1.VX;

		//Create line segments between x=targetMin and x=targetMax
		Line line0 = new(new(targetMin, h0.Y + (h0.VY * t0ToMin)), new(targetMax, h0.Y + (h0.VY * t0ToMax)));
		Line line1 = new(new(targetMin, h1.Y + (h1.VY * t1ToMin)), new(targetMax, h1.Y + (h1.VY * t1ToMax)));

		DecimalPoint? intersection = LineIntersection.Find(line0, line1);

		// doesn't intersect
		if (intersection is null) {
			return false;
		}

		// doesn't intersect in the area because Y is out of the range
		if (intersection.Y < targetMin || intersection.Y > targetMax ) {
			return false;
		}

		// In the past for hailstone0
		if ((intersection.X - h0.X) / h0.VX < 0) {
			return false;
		}

		// In the past for hailstone1
		if ((intersection.X - h1.X) / h1.VX < 0) {
			return false;
		}

		return true;
	}

	private sealed record Hailstone(long X, long Y, long Z, int VX, int VY, int VZ) : IParsable<Hailstone> {
		public static Hailstone Parse(string s, IFormatProvider? provider)
		{
			char[] splitBy = ['@', ','];
			string[] tokens = s.TrimmedSplit(splitBy);
			return new(
				tokens[0].As<long>(), tokens[1].As<long>(), tokens[2].As<long>(),
				tokens[3].As<int>(),  tokens[4].As<int>(),  tokens[5].As<int>());
		}

		public static Hailstone Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Hailstone result)
			=> ISimpleParsable<Hailstone>.TryParse(s, provider, out result);
	}
}
