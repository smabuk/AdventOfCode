namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 15: Warehouse Woes
/// https://adventofcode.com/2024/day/15
/// </summary>
[Description("Warehouse Woes")]
public static partial class Day15 {

	private static char[,] _map = default!;
	private static List<Direction> _directions = [];
	private static Action<string[], bool>? _visualise = null;

	[Init]
	public static void Load(string[] input, Action<string[], bool>? visualise = null)
	{
		_map = input
			.TakeWhile(i => !string.IsNullOrWhiteSpace(i))
			.To2dArray();

		_directions = [..
			string.Join("", input.Skip(_map.RowsCount()))
			.Select(c => c.ToDirection())
			];

		_visualise = visualise;
		_map.VisualiseMap("Initial state:");
	}

	public static int Part1(string[] _)
	{
		char[,] map = (char[,])_map.Clone();

		Robot robot = new(map.ForEachCell().Single(c => c.Value is ROBOT).Index);

		foreach (Direction direction in _directions) {
			if (robot.TryMovePart1(direction, map, out Thing newRobot)) {
				robot = (Robot)newRobot;
			}

			map.VisualiseMap($"Move {direction.FromDirection()}:", clearScreen: true);
		}

		map.VisualiseMap("Final state:");

		return map.SumOfGpsCoordinates();
	}

	private static bool TryMovePart1(this Thing thing, Direction direction, char[,] map, out Thing newThing)
	{
		newThing = thing with { Location = thing.Location + direction.Delta() };
		char value = newThing.GetTheValue(map);

		if (value is BOX) {
			Box box = new(newThing.Location);
			_ = box.TryMovePart1(direction, map, out Thing _);
			value = newThing.GetTheValue(map);
		}

		if (value is EMPTY) {
			map.UpdateMap(thing, EMPTY);
			map.UpdateMap(newThing, thing is Robot ? ROBOT : BOX);
			return true;
		}

		return false;
	}




	public static int Part2(string[] _)
	{
		char[,] map = _map.MakeWide();
		map.VisualiseMap("Wide initial state:");
		Robot robot = new(map.ForEachCell().Single(c => c.Value is ROBOT).Index);

		foreach (Direction direction in _directions) {
			if (robot.TryMoveRobot(direction, map, out Thing newRobot)) {
				robot = (Robot)newRobot;
			}

			map.VisualiseMap($"Move {direction.FromDirection()}:", clearScreen: true);
		}

		map.VisualiseMap("Final state:");

		return map.SumOfGpsCoordinates();
	}

	private static bool TryMoveRobot(this Robot robot, Direction direction, char[,] map, out Thing newRobot)
	{
		newRobot = robot with { Location = robot.Location + direction.Delta() };

		char value = newRobot.GetTheValue(map);

		if (value is BOX_LEFT) {
			WideBox box = new(newRobot.Location);
			_ = box.TryMoveBox(direction, map);
			value = newRobot.GetTheValue(map);
		} else if (value is BOX_RIGHT) {
			WideBox box = new(newRobot.Location.MoveLeft());
			_ = box.TryMoveBox(direction, map);
			value = newRobot.GetTheValue(map);
		}

		if (value is EMPTY) {
			map.UpdateMap(robot,    EMPTY);
			map.UpdateMap(newRobot, ROBOT);
			return true;
		}

		return false;
	}

	private static bool TryMoveBox(this WideBox box, Direction direction, char[,] map)
	{
		WideBox newBox = box with { Location = box.Location + direction.Delta() };

		return direction switch
		{
			Direction.Left or Direction.Right => newBox.TryMoveBoxHorizontally(box, direction, map),
			Direction.Up   or Direction.Down  => newBox.TryMoveBoxVertically(box, direction, map),
			_ => throw new NotImplementedException(),
		};
	}

	private static bool TryMoveBoxHorizontally(this WideBox newBox, WideBox box, Direction direction, char[,] map)
	{
		char value = newBox.GetTheValue(map);

		if (direction is Direction.Left && value is BOX_RIGHT) {
			WideBox box1 = newBox with { Location = newBox.Location.MoveLeft() };
			_ = box1.TryMoveBox(direction, map);
			value = newBox.GetTheValue(map);
		} else if (direction is Direction.Right) {
			value = newBox.Location.MoveRight().GetTheValue(map);
			if (value is BOX_RIGHT) {
				_ = newBox.TryMoveBox(direction, map);
				value = newBox.GetTheValue(map);
			} else if (value is BOX_LEFT) {
				WideBox box1 = newBox with { Location = newBox.Location.MoveRight() };
				_ = box1.TryMoveBox(direction, map);
				value = box1.GetTheValue(map);
			}
		}

		if (value is EMPTY) {
			map.UpdateMap(box, EMPTY, EMPTY);
			map.UpdateMap(newBox, BOX_LEFT, BOX_RIGHT);
			return true;
		}

		return false;
	}

	private static bool TryMoveBoxVertically(this WideBox newBox, WideBox box, Direction direction, char[,] map)
	{
		char value1 = newBox.L.GetTheValue(map);
		char value2 = newBox.R.GetTheValue(map);

		if (value1 is WALL || value2 is WALL) {
			return false;
		}

		if (value1 is BOX_LEFT or BOX_RIGHT || value2 is BOX_LEFT or BOX_RIGHT) {
			(bool canIMove, List<WideBox> boxes1) = box.CanIMove(direction, map);
			if (canIMove) {
				List<WideBox> boxes = direction switch
				{
					Direction.Up   => [.. boxes1.OrderBy(b => b.Location)],
					Direction.Down => [.. boxes1.OrderByDescending(b => b.Location)],
					_ => throw new NotImplementedException(),
				};

				foreach (WideBox boxToMove in boxes) {
					newBox = box with { Location = boxToMove.Location + direction.Delta() };
					map.UpdateMap(boxToMove, EMPTY, EMPTY);
					map.UpdateMap(newBox, BOX_LEFT, BOX_RIGHT);
				}

				return true;
			}
		}

		if (value1 is EMPTY && value2 is EMPTY) {
			newBox = box with { Location = box.Location + direction.Delta() };
			map.UpdateMap(box, EMPTY, EMPTY);
			map.UpdateMap(newBox, BOX_LEFT, BOX_RIGHT);
			return true;
		}

		return false;
	}

	private static (bool Yes, List<WideBox> WideBoxes) CanIMove(this WideBox? box, Direction direction, char[,] map)
	{
		if (box is null) {
			return (true, []);
		}

		WideBox newBox = box with { Location = box.Location + direction.Delta() };
		char value1 = newBox.L.GetTheValue(map);
		char value2 = newBox.R.GetTheValue(map);

		if (value1 is EMPTY && value2 is EMPTY) {
			return (true, []);
		}

		if (value1 is WALL || value2 is WALL) {
			//map.VisualiseMap($"Can't Move: {direction} {box}");
			return (false, []);
		}

		bool canIMove = true;
		List<WideBox> boxes = [box];

		WideBox? box1 = value1 switch {
			BOX_RIGHT => newBox with { Location = newBox.Location.MoveLeft() },
			BOX_LEFT  => newBox with { },
			_ => null,
		};

		WideBox? box2 = value2 switch {
			BOX_LEFT => newBox with { Location = newBox.Location.MoveRight() },
			_ => null,
		};

		if (box1 is not null) {
			(bool result1, List<WideBox> boxes1) = box1.CanIMove(direction, map);
			canIMove = canIMove && result1;
			boxes = [.. boxes, box1, .. boxes1];
		}

		if (box2 is not null) {
			(bool result2, List<WideBox> boxes2) = box2.CanIMove(direction, map);
			canIMove = canIMove && result2;
			boxes = [.. boxes, box2, .. boxes2];
		}

		return (canIMove, [.. boxes.Distinct()]);
	}

	private static int GpsCoordinate(this Point location) => (location.Y * 100) + location.X;
	private static int SumOfGpsCoordinates(this char[,] map)
	{
		return map
			.ForEachCell()
			.Where(c => c.Value is BOX or BOX_LEFT)
			.Sum(c => c.Index.GpsCoordinate());
	}


	private static void UpdateMap(this char[,] map, Thing thing, char value) => map[thing.X, thing.Y] = value;
	private static void UpdateMap(this char[,] map, WideBox widebox, char value1, char value2)
	{
		map[widebox.L.X, widebox.L.Y] = value1;
		map[widebox.R.X, widebox.R.Y] = value2;
	}

	private static char GetTheValue(this Point location, char[,] map) => map[location.X, location.Y];
	private static char GetTheValue(this Thing thing, char[,] map) => map[thing.Location.X, thing.Location.Y];


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

	private static char[,] MakeWide(this char[,] map)
	{
		char[,] newMap = new char[map.ColsCount() * 2, map.RowsCount()];
		newMap = newMap.Fill('.');

		foreach (Cell<char> cell in map.ForEachCell()) {
			if (cell.Value is ROBOT) {
				newMap[cell.Index.X * 2, cell.Index.Y] = ROBOT;
			} else if (cell.Value is WALL) {
				newMap[cell.Index.X * 2, cell.Index.Y]       = WALL;
				newMap[(cell.Index.X * 2) + 1, cell.Index.Y] = WALL;
			} else if (cell.Value is BOX) {
				newMap[cell.Index.X * 2, cell.Index.Y]       = BOX_LEFT;
				newMap[(cell.Index.X * 2) + 1, cell.Index.Y] = BOX_RIGHT;
			}
		}

		return newMap;
	}


	private static void VisualiseMap(this char[,] map, string title, bool clearScreen = false)
	{
		if (_visualise is null) {
			return;
		}

		string[] output = ["", title];
		if (map.ColsCount() > 20) {
			output = [.. output, .. map.AsStrings().Select(s => s.Replace(EMPTY, ' '))];
		} else {
			output = [.. output, .. map.AsStrings()];
		}

		_visualise?.Invoke(output, clearScreen);
	}



	private abstract record Thing(Point Location)
	{
		public int X => Location.X;
		public int Y => Location.Y;
	};

	private record Robot(Point Location) : Thing(Location);
	private record Box(Point Location) : Thing(Location);
	private record WideBox(Point Location) : Thing(Location)
	{
		public Point L => Location;
		public Point R => Location.MoveRight();
	}

	private const char BOX        = 'O';
	private const char BOX_LEFT   = '[';
	private const char BOX_RIGHT  = ']';
	private const char EMPTY      = '.';
	private const char ROBOT      = '@';
	private const char WALL       = '#';
}
