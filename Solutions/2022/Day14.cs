namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day XX: Title
/// https://adventofcode.com/2022/day/XX
/// </summary>
[Description("")]
public sealed partial class Day14 {

	//[Init]
	//public static    void Init(string[] input, params object[]? _) => LoadInstructions(input);
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static IEnumerable<LinePath> _instructions = Array.Empty<LinePath>();

	private static readonly Point START_POINT = new(500, 0);
	private static readonly int Y_DIRECTION = -1;

	private static char AIR = '.';
	private static char ROCK = '#';
	private static char SAND_SOURCE = '+';
	private static char SAND = 'o';


	private static void LoadInstructions(string[] input) {
		_instructions = input.Select(LinePath.Parse);
	}

	private static int Solution1(string[] input) {
		int unitsOfSand = 0;
		int floorDepth = 999;
		Point sandPosition= START_POINT;
		char[,] cave;
		List<List<Point>> linePaths = new ();
		int minX = int.MaxValue;
		int minY = 0;
		int maxX = int.MinValue;
		int maxY = int.MinValue;

		for (int i = 0; i < input.Length; i++) {
			List<Point> pathPoints = new();
			int[] coords = input[i].Replace("-", "").Split(new char[] { '>', ',' }).AsInts().ToArray();
			for (int j = 0; j < coords.Length; j+= 2) {
				pathPoints.Add(new (coords[j], coords[j+1]));
				minX = Math.Min(minX, coords[j]);
				maxX = Math.Max(maxX, coords[j]);
				maxY = Math.Max(maxY, coords[j+1]);
			}
			linePaths.Add(pathPoints);
		}

		int xOffset = minX;
		int yOffset = minY;

		cave = new char[maxX - minX + 1, maxY - minY + 1];
		foreach ((int X, int Y) in cave.Walk2dArray()) {
			cave[X, Y] = AIR;
		}
		cave[START_POINT.X - xOffset, START_POINT.Y - yOffset] = SAND_SOURCE;

		foreach (var linePath in linePaths) {
			for (int lp = 0; lp < linePath.Count - 1; lp++) {
				Point start = linePath[lp];
				Point end = linePath[lp+1];
				Point d = start - end;
				int steps = Math.Max(Math.Abs(d.X), Math.Abs(d.Y)) + 1;
				for (int i = 0; i < steps; i++) {
					int x = start.X + (i * Math.Sign(d.X) * -1);
					int y = start.Y + (i * Math.Sign(d.Y) * -1);
					cave[x - xOffset, y] = ROCK;
				}
			}
		}

		//Console.WriteLine(cave.PrintAsString(width: 0));
		floorDepth = maxY;
		Point startPoint = START_POINT with { X = START_POINT.X - xOffset };
		bool falling = false;
		while (!falling) {
			Point sand = startPoint;
			for (int y = 0; y <= floorDepth; y++) {
				if (sand.X <= 0 || y == floorDepth || sand.X >= maxX - xOffset) {
					falling = true;
					break;
				}
				int newY = sand.Y + 1;
				if (cave[sand.X, newY] == AIR) {
					sand = sand with { Y = newY };
				} else if (sand.X > 0 && cave[sand.X - 1, newY] == AIR) {
					sand = sand with {X = sand.X - 1, Y = newY };
				} else if ((sand.X < maxX - xOffset) && cave[sand.X + 1, newY] == AIR) {
					sand = sand with {X = sand.X + 1, Y = newY };
				} else {
					break;
				}
			}

			if (!falling) {
				unitsOfSand++;
				cave[sand.X, sand.Y] = SAND;
				sandPosition = sandPosition with { Y = sandPosition.Y + 1 };
			}
		}

		return unitsOfSand;
	}

	private static string Solution2(string[] input) {
		//string inputLine = input[0];
		//List<string> inputs = input.ToList();
		List<LinePath> instructions = input.Select(ParseLine).ToList();
		return "** Solution not written yet **";
	}

	private record LinePath(string Name, int Value) : IParsable<LinePath> {
		public static LinePath Parse(string s) => throw new NotImplementedException();
		public static LinePath Parse(string s, IFormatProvider? provider = null) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out LinePath result) => throw new NotImplementedException();
	}

	private static LinePath ParseLine(string input) {
		//MatchCollection match = InputRegEx().Matches(input);
		Match match = InputRegEx().Match(input);
		if (match.Success) {
			return new(match.Groups["opts"].Value, int.Parse(match.Groups["number"].Value));
		}
		return null!;
	}

	[GeneratedRegex("""(?<opts>opt1|opt2|opt3) (?<number>[\+\-]\d+)""")]
	private static partial Regex InputRegEx();
}
