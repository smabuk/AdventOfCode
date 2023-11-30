namespace AdventOfCode.Solutions._2018;

/// <summary>
/// Day 17: Reservoir Research
/// https://adventofcode.com/2018/day/17
/// </summary>
[Description("Reservoir Research")]
public sealed partial class Day17 {

	[Init]
	public static    void Init(string[] input, Action<string[], bool>? visualise = null, params object[]? _) => LoadScan(input, visualise);
	public static string Part1(string[] input, Action<string[], bool>? visualise = null, params object[]? _) => Solution1(input, visualise).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();


	const char SPRING        = '+';
	const char CLAY          = '#';
	const char SAND          = '.';
	const char WATER_AT_REST = '~';
	const char DRIED_SAND    = '|';

	private static IEnumerable<ClayVein> _veins = [];
	private static char[,] _ground = new char[0,0];
	private static int minX = int.MaxValue;
	private static int minY = 0;
	private static int maxX = int.MinValue;
	private static int maxY = int.MinValue;
	private static int xOffset = int.MinValue;
	
	private static void LoadScan(string[] input, Action<string[], bool>? visualise = null) {
		_veins = input.Select(ClayVein.Parse);

		foreach (ClayVein vein in _veins) {
			minX = ((int[])[minX, vein.Start.X, vein.End.X]).Min();
			maxX = ((int[])[maxX, vein.Start.X, vein.End.X]).Max();
			maxY = ((int[])[maxY, vein.Start.Y, vein.End.Y]).Max();
		}

		Point springOfWater = new(500, 0);
		_ground = new char[maxX - minX + 3, maxY + 1];
		xOffset = minX - 1;
		for (int y = minY; y <= maxY; y++) {
			for (int x = minX - 1; x <= maxX + 1; x++) {
				_ground[x - xOffset, y] = SAND;
				if (x == springOfWater.X && y == springOfWater.Y) {
					_ground[x - xOffset, y] = SPRING;
				}
			}
		}

		foreach (ClayVein vein in _veins) {
			foreach (Point veinTiles in vein.Tiles) {
				_ground[veinTiles.X - xOffset, veinTiles.Y] = CLAY;
			}
		}
		_ = Task.Run(() => visualise?.Invoke(["Initial State", .. _ground.PrintAsStringArray(0)], false));
	}

	private static int Solution1(string[] input, Action<string[], bool>? visualise = null) {
		int tiles = _ground.Walk2dArrayWithValues().Count(g => g.Value is DRIED_SAND or WATER_AT_REST);
		return tiles;
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}

	private record ClayVein(Point Start, Point End) : IParsable<ClayVein> {
		public IEnumerable<Point> Tiles = (Start.X == End.X) switch
		{
			true  => Enumerable.Range(Start.Y, End.Y - Start.Y + 1).Select(y => new Point(Start.X, y)),
			false => Enumerable.Range(Start.X, End.X - Start.X + 1).Select(x => new Point(x, Start.Y)),
		};

		public static ClayVein Parse(string s)
		{
			Match match = InputRegEx().Match(s);
			if (match.Success) {
				string axis1 = match.Groups["axis1"].Value;
				Point start = axis1 switch
				{
					"x" => new(int.Parse(match.Groups["axis1number"].Value), int.Parse(match.Groups["axis2number1"].Value)),
					_ => new(int.Parse(match.Groups["axis2number1"].Value), int.Parse(match.Groups["axis1number"].Value)),
				};
				Point end = axis1 switch
				{
					"x" => new(int.Parse(match.Groups["axis1number"].Value), int.Parse(match.Groups["axis2number2"].Value)),
					_ => new(int.Parse(match.Groups["axis2number2"].Value), int.Parse(match.Groups["axis1number"].Value)),
				};
				return new(start, end);
			}
			return null!;
		}

		public static ClayVein Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out ClayVein result) => throw new NotImplementedException();
	}

	[GeneratedRegex("""(?<axis1>x|y)=(?<axis1number>[\+\-]*\d+), (?<axis2>x|y)=(?<axis2number1>[\+\-]*\d+)\.\.(?<axis2number2>[\+\-]*\d+)""")]
	private static partial Regex InputRegEx();
}
