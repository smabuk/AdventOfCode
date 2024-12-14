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

		List<Robot> robots = [.. _robots];
		
		robots.VisualiseMap(width, height, "Initial state:", visualise);

		for (int noOfSeconds = 1; noOfSeconds <= NO_OF_SECONDS; noOfSeconds++) {
			robots = [.. robots.Select(r => r.MoveNext(width, height))];
			if (noOfSeconds < 6) {
				robots.VisualiseMap(width, height, $"After {noOfSeconds} seconds:", visualise);
			}
		}

		robots.VisualiseMap(width, height, "Final state:", visualise);

		return robots.SafetyFactor(width, height);
	}

	public static string Part2(string[] input, params object[]? args) => NO_SOLUTION_WRITTEN_MESSAGE;

	private static Robot MoveNext(this Robot robot, int width, int height)
	{
		(int x, int y) = (robot.Position + robot.Velocity);
		if (x >= width ) { x %= width; }
		if (y >= height) { y %= height; }
		if (x < 0) { x = width + x; }
		if (y < 0) { y = height + y; }

		return robot with { Position = new(x, y) };
	}

	public static int SafetyFactor(this IEnumerable<Robot> robots, int width, int height)
	{
		int midX = width / 2;
		int midY = height / 2;

		int quad1 = robots.Count(robot => robot.Position.X < midX  && robot.Position.Y < midY);
		int quad2 = robots.Count(robot => robot.Position.X > midX  && robot.Position.Y < midY);
		int quad3 = robots.Count(robot => robot.Position.X < midX  && robot.Position.Y > midY);
		int quad4 = robots.Count(robot => robot.Position.X > midX  && robot.Position.Y > midY);

		return quad1 * quad2 * quad3 * quad4;

	}

	public static void VisualiseMap(this IEnumerable<Robot> robots, int width, int height, string title, Action<string[], bool>? visualise)
	{
		const char EMPTY = '.';

		int[,] map = new int[width, height];
		//map = map.Fill(0);

		foreach (Robot robot in robots) {
			map[robot.Position.X, robot.Position.Y] += 1;
		}

		if (visualise is not null) {
			string[] output = ["", title, .. map.AsStrings().Select(s => s.Replace('0', EMPTY))];
			_ = Task.Run(() => visualise?.Invoke(output, false));
		}
	}

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

	private static int TilesWide(this object[]? args) => GetArgument(args, 1, 101);
	private static int TilesTall(this object[]? args) => GetArgument(args, 2, 103);

}
