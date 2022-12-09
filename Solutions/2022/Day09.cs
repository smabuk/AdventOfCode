namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 09: Rope Bridge
/// https://adventofcode.com/2022/day/9
/// </summary>
[Description("Rope Bridge")]
public sealed partial class Day09 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		List<Instruction> instructions = input.Select(i => ParseLine(i)).ToList();
		HashSet<Point> tailPoints = new();
		Point head = new(0, 0);
		Point tail = new(0, 0);
		tailPoints.Add(tail);

		foreach (Instruction instruction in instructions) {
			for (int i = 0; i < instruction.Value; i++) {
				head = instruction.Direction switch {
					Direction.Left => head with { X = head.X - 1 },
					Direction.Right => head with { X = head.X + 1 },
					Direction.Up => head with { Y = head.Y - 1 },
					Direction.Down => head with { Y = head.Y + 1 },
					_ => throw new NotImplementedException(),
				};
				tail = MoveTail(head, tail);
				tailPoints.Add(tail);
			}
		}

		return tailPoints.Count;
	}

	private static string Solution2(string[] input) {
		//List<string> inputs = input.ToList();
		List<Instruction> instructions = input.Select(i => ParseLine(i)).ToList();
		return "** Solution not written yet **";
	}

	private static Point MoveTail(Point head, Point tail) {
		int dX = head.X - tail.X;
		int dY = head.Y - tail.Y;
		if (Math.Abs(dX) > 1 || Math.Abs(dY) > 1) {
			if (dX == 0) {
				tail = tail with { Y = dY > 0 ? tail.Y + 1 : tail.Y - 1 };
			} else if (dY == 0) {
				tail = tail with { X = dX > 0 ? tail.X + 1 : tail.X - 1 };
			} else {
				tail = tail with { X = dX > 0 ? tail.X + 1 : tail.X - 1, Y = dY > 0 ? tail.Y + 1 : tail.Y - 1 };
			}
		}
		return tail;
	}

	private record struct Instruction(Direction Direction, int Value);

	private static Instruction ParseLine(string input) {
		string[] tokens = input.Split(' ');
		Direction direction = tokens[0] switch {
			"L" => Direction.Left,
			"R" => Direction.Right,
			"U" => Direction.Up,
			"D" => Direction.Down,
			_ => throw new ArgumentException(input),
		};
		return new(direction, int.Parse(tokens[1]));
	}

	private enum Direction { Left, Right, Up, Down }
}
