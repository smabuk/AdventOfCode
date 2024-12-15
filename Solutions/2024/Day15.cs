namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 15: Warehouse Woes
/// https://adventofcode.com/2024/day/15
/// </summary>
[Description("Warehouse Woes")]
public static partial class Day15 {

	private static char[,] _map = default!;
	private static IEnumerable<Direction> _directions = [];

	[Init]
	public static void Load(string[] input, Action<string[], bool>? visualise = null)
	{
		_map = input
			.TakeWhile(i => !string.IsNullOrWhiteSpace(i))
			.To2dArray();
		_directions = string.Join("", input.Skip(_map.RowsCount()))
			.Select(c => c.ToDirection());
	}

	public static int Part1(string[] _, Action<string[], bool>? visualise = null, params object[]? args)
	{
		char[,] map = (char[,])_map.Clone();

		map.VisualiseMap("Initial state:", visualise);

		Robot robot = new(map.ForEachCell().Single(c => c.Value is ROBOT).Index);

		foreach (var direction in _directions) {
			if (robot.TryMove(direction, map, out Thing newRobot)) {
				robot = (Robot)newRobot;
			}

			map.VisualiseMap($"Move {direction.FromDirection()}:", visualise);
		}

		map.VisualiseMap("Final state:", visualise);

		return map.SumOfGpsCoordinates();
	}

	private static bool TryMove(this Thing thing, Direction direction, char[,] map, out Thing newThing)
	{
		newThing = thing with { Location = thing.Location + direction.Delta() };
		if (map.TryGetValue(newThing.Location, out char value) && value is WALL) {
			return false;
		}

		if (value is BOX) {
			Box box = new(newThing.Location);
			_ = box.TryMove(direction, map, out Thing _);
			_ = map.TryGetValue(newThing.Location, out value);
		}

		if (value is EMPTY) {
			map[thing.Location.X, thing.Location.Y] = EMPTY;
			thing = newThing;
			map[thing.Location.X, thing.Location.Y] = thing is Box ? BOX : ROBOT;
			return true;
		}

		return false;
	}

	private static int SumOfGpsCoordinates(this char[,] map)
	{
		return map
			.ForEachCell()
			.Where(c => c.Value is BOX)
			.Sum(c => c.Index.GpsCoordinate());
	}

	private static int GpsCoordinate(this Point location) => (location.Y * 100) + location.X;


	public static string Part2(string[] input, params object[]? args) => NO_SOLUTION_WRITTEN_MESSAGE;

	public static Direction ToDirection(this char c)
	{
		return c switch
		{
			'^' => Direction.Up,
			'>' => Direction.Right,
			'v' => Direction.Down,
			'<' => Direction.Left,
			_ => throw new NotImplementedException(),
		};
	}

	public static char FromDirection(this Direction direction)
	{
		return direction switch
		{
			Direction.Up    => '^',
			Direction.Right => '>',
			Direction.Down  => 'v',
			Direction.Left  => '<',
			_ => throw new NotImplementedException(),
		};
	}

	private static void VisualiseMap(this char[,] map, string title, Action<string[], bool>? visualise, bool clearScreen = false)
	{
		if (visualise is null) {
			return;
		}

		string[] output = ["", title, .. map.AsStrings().Select(s => s.Replace('0', '.'))];
		visualise?.Invoke(output, clearScreen);
	}



	private abstract record Thing(Point Location);
	private record Robot(Point Location) : Thing(Location);
	private record Box(Point Location) : Thing(Location);

	private const char WALL  = '#';
	private const char EMPTY = '.';
	private const char BOX   = 'O';
	private const char ROBOT = '@';
}
