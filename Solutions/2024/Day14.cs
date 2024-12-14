namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 14: Restroom Redoubt
/// https://adventofcode.com/2024/day/14
/// </summary>
[Description("Restroom Redoubt")]
public static partial class Day14 {

	private static IEnumerable<Robot> _robots = [];

	[Init]
	public static void LoadRobots(string[] input) => _robots = [.. input.As<Robot>()];


	public static int Part1(string[] _, Action<string[], bool>? visualise = null, params object[]? args)
	{
		const int NO_OF_SECONDS = 100;

		int width  = args.TilesWide();
		int height = args.TilesTall();

		List<Robot> robots;

		_robots.VisualiseMap(width, height, "Initial state:", visualise);

		if (height < 50) { // Tests visualisation
			robots = [.. _robots];
			for (int noOfSeconds = 1; noOfSeconds < 6; noOfSeconds++) {
				robots = [.. robots.Select(r => r.MoveNext(width, height))];
				robots.VisualiseMap(width, height, $"After {noOfSeconds} seconds:", visualise);
			}
		}

		robots = [.. _robots.Select(r => r.MoveNext(width, height, NO_OF_SECONDS))];

		robots.VisualiseMap(width, height, "Final state:", visualise);

		return robots.SafetyFactor(width, height);
	}

	private static Robot MoveNext(this Robot robot, int width, int height, int noOfSeconds = 1)
	{
		(int x, int y) = (robot.Position + (robot.Velocity * noOfSeconds));

		if (x < 0) { x = width  + (x % width ); }
		if (y < 0) { y = height + (y % height); }

		return robot with { Position = new(x % width, y % height) };
	}

	public static int SafetyFactor(this IEnumerable<Robot> robots, int width, int height)
	{
		int midX = width  / 2;
		int midY = height / 2;

		int quad1 = robots.Count(robot => robot.Position.X < midX && robot.Position.Y < midY);
		int quad2 = robots.Count(robot => robot.Position.X > midX && robot.Position.Y < midY);
		int quad3 = robots.Count(robot => robot.Position.X < midX && robot.Position.Y > midY);
		int quad4 = robots.Count(robot => robot.Position.X > midX && robot.Position.Y > midY);

		return quad1 * quad2 * quad3 * quad4;
	}


	public static int Part2(string[] _, Action<string[], bool>? visualise = null, params object[]? args)
	{
		int width  = args.TilesWide();
		int height = args.TilesTall();

		int noOfSeconds = 101;

		while (noOfSeconds++ < 50_000
				&& !_robots
					.Select(r => r.MoveNext(width, height, noOfSeconds))
					.IsChristmasTree()) {

			_robots
				.Select(r => r.MoveNext(width, height, noOfSeconds))
				.VisualiseMap(width, height, $"Looking for Christmas Tree: {noOfSeconds} seconds", visualise, true);
		}

		_robots
			.Select(r => r.MoveNext(width, height, noOfSeconds))
			.VisualiseMap(width, height, $"Christmas Tree at:  {noOfSeconds} seconds", visualise);

		return noOfSeconds;
	}

	public static bool IsChristmasTree(this IEnumerable<Robot> robots)
	{
		const int THRESHOLD = 10;

		int count = 0;
		Point prev = Point.Zero;
		foreach (Robot robot in robots.OrderBy(robots => robots.Position)) {
			count = (robot.Position.Y == prev.Y && robot.Position.X == prev.X + 1)
				? count + 1
				: 0;

			if (count > THRESHOLD) {
				return true;
			}

			prev = robot.Position;
		}

		return false;
	}

	public static void VisualiseMap(this IEnumerable<Robot> robots, int width, int height, string title, Action<string[], bool>? visualise, bool clearScreen = false)
	{
		if (visualise is null) {
			return;
		}

		int[,] map = new int[width, height];

		foreach (Robot robot in robots) {
			map[robot.Position.X, robot.Position.Y] += 1;
		}

		string[] output = ["", title, .. map.AsStrings().Select(s => s.Replace('0', '.'))];
		visualise?.Invoke(output, clearScreen);
	}

	private static int TilesWide(this object[]? args) => GetArgument(args, 1, 101);
	private static int TilesTall(this object[]? args) => GetArgument(args, 2, 103);


	public sealed record Robot(Point Position, Point Velocity) : IParsable<Robot>
	{
		public static Robot Parse(string s, IFormatProvider? provider)
		{
			Match match = InputRegEx().Match(s);
			return match.Success
				? new(new(match.As<int>("positionx"), match.As<int>("positiony")),
				      new(match.As<int>("velocityx"), match.As<int>("velocityy")))
				: null!;
		}

		public static Robot Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Robot result)
			=> ISimpleParsable<Robot>.TryParse(s, provider, out result);
	}

	[GeneratedRegex("""p=(?<positionx>[\+\-]?\d+),(?<positiony>[\+\-]?\d+) v=(?<velocityx>[\+\-]?\d+),(?<velocityy>[\+\-]?\d+)""")]
	public static partial Regex InputRegEx();
}
