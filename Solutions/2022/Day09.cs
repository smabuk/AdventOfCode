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
			for (int i = 0; i < instruction.Steps; i++) {
				head = MoveHead(head, instruction.Direction);
				tail = MoveTail(head, tail);
				tailPoints.Add(tail);
			}
		}

		return tailPoints.Count;
	}

	private static int Solution2(string[] input) {
		const int NO_OF_KNOTS = 10;
		const int HEAD = 0;
		const int TAIL = 9;
		List<Instruction> instructions = input.Select(i => ParseLine(i)).ToList();
		HashSet<Point> tailPoints = new();
		Point[] knots= new Point[NO_OF_KNOTS];
		for (int i = 0; i < NO_OF_KNOTS; i++) {
			knots[i] = new Point(0, 0);
		}
		tailPoints.Add(knots[TAIL]);

		foreach (Instruction instruction in instructions) {
			for (int i = 0; i < instruction.Steps; i++) {
				knots[HEAD] = MoveHead(knots[HEAD], instruction.Direction);
				for (int knot = 0; knot < NO_OF_KNOTS - 1; knot++) {
					knots[knot + 1] = MoveTail(knots[knot], knots[knot+1]);
				}
				tailPoints.Add(knots[TAIL]);
			}
		}

		return tailPoints.Count;
	}

	private static Point MoveHead(Point head, Direction direction) {
		return direction switch {
			Direction.Left => head with { X = head.X - 1 },
			Direction.Right => head with { X = head.X + 1 },
			Direction.Up => head with { Y = head.Y - 1 },
			Direction.Down => head with { Y = head.Y + 1 },
			_ => throw new NotImplementedException(),
		};
	}

	private static Point MoveTail(Point head, Point tail) {
		int xMagnitude = head.X - tail.X;
		int yMagnitude = head.Y - tail.Y;
		int dX = int.Clamp(xMagnitude, -1, 1);
		int dY = int.Clamp(yMagnitude, -1, 1);

		if (Math.Abs(xMagnitude) > 1 || Math.Abs(yMagnitude) > 1) {
			tail = tail with { X = tail.X + dX, Y = tail.Y + dY };
		}
		return tail;
	}

	private record struct Instruction(Direction Direction, int Steps);

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
