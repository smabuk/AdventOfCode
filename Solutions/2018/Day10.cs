namespace AdventOfCode.Solutions._2018;

/// <summary>
/// Day 10: The Stars Align
/// https://adventofcode.com/2018/day/10
/// </summary>
[Description("The Stars Align")]
public sealed partial class Day10 {

	[Init]
	public static    void Init(string[] input, params object[]? _) => LoadTheStars(input);
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static readonly char[] _splitChars = ['<', '>', ','];
	private static IEnumerable<PointOfLight> _pointsOfLight = [];

	private static void LoadTheStars(string[] input) {
		_pointsOfLight = input.Select(PointOfLight.Parse);
	}

	private static string Solution1(string[] _)
	{
		List<PointOfLight> pointsOfLight = _pointsOfLight.ToList();

		int noOfSeconds = MoveTheStars(pointsOfLight, out int minY, out int maxY);

		int minX = pointsOfLight.Min(p => p.Position.X);
		int maxX = pointsOfLight.Max(p => p.Position.X);
		char[,] stars = new char[maxX - minX + 1, maxY - minY + 1];

		foreach ((int X, int Y) in stars.Walk2dArray()) {
			stars[X, Y] = ' ';
		}

		foreach (PointOfLight pointOfLight in pointsOfLight) {
			stars[pointOfLight.Position.X - minX, pointOfLight.Position.Y - minY] = '█';
		}

		string ocrString = OcrHelpers.IdentifyMessage(stars.PrintAsStringArray(width: 0), ' ', '█');
		string message = Environment.NewLine + String.Join(Environment.NewLine, stars.PrintAsStringArray(width: 0)); ;

		return message;
	}

	private static int Solution2(string[] input)
	{
		List<PointOfLight> pointsOfLight = _pointsOfLight.ToList();
		return MoveTheStars(pointsOfLight, out int _, out int _);
	}

	private static int MoveTheStars(List<PointOfLight> pointsOfLight, out int minY, out int maxY)
	{
		int letterHeight = pointsOfLight.Count < 35 ? 8 : 10;
		int noOfSeconds = 0;
		minY = int.MinValue;
		maxY = int.MaxValue;
		do {
			for (int i = 0; i < pointsOfLight.Count; i++) {
				pointsOfLight[i] = pointsOfLight[i].Move();
			}

			minY = pointsOfLight.Min(p => p.Position.Y);
			maxY = pointsOfLight.Max(p => p.Position.Y);
			noOfSeconds++;
		} while (maxY - minY > letterHeight);

		return noOfSeconds;
	}

	private record PointOfLight(Point Position, Point Velocity) : IParsable<PointOfLight> {

		public PointOfLight Move() => this with { Position = Position + Velocity };

		public static PointOfLight Parse(string s) => ParseLine(s);
		public static PointOfLight Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out PointOfLight result) => throw new NotImplementedException();
	}

	private static PointOfLight ParseLine(string input) {
		string[] tokens = input.Split(_splitChars, StringSplitOptions.TrimEntries);
		Point position = new(int.Parse(tokens[1]), int.Parse(tokens[2]));
		Point velocity = new(int.Parse(tokens[4]), int.Parse(tokens[5]));

		return new(position, velocity);
	}
}
