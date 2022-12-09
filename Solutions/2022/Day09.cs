namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 09: Rope Bridge
/// https://adventofcode.com/2022/day/9
/// </summary>
[Description("Rope Bridge")]
public sealed partial class Day09 {

	[Init] public static void Init(string[] input, params object[]? _) => LoadInstructions(input);
	public static string Part1(string[] input, params object[]? _) => Solution1().ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2().ToString();

	private static List<Instruction> instructions = new();

	private static void LoadInstructions(string[] input) {
		instructions = input.Select(i => Instruction.Parse(i)).ToList();
	}

	private static int Solution1() {
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

	private static int Solution2() {
		const int NO_OF_KNOTS = 10;
		const int HEAD = 0;
		const int TAIL = 9;
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
			Direction.Left  => head with { X = head.X - 1 },
			Direction.Right => head with { X = head.X + 1 },
			Direction.Up    => head with { Y = head.Y - 1 },
			Direction.Down  => head with { Y = head.Y + 1 },
			_ => throw new NotImplementedException(),
		};
	}

	private static Point MoveTail(Point head, Point tail) {
		int dX = head.X - tail.X;
		int dY = head.Y - tail.Y;

		if (Math.Abs(dX) > 1 || Math.Abs(dY) > 1) {
			tail = tail with { X = tail.X + Math.Sign(dX), Y = tail.Y + Math.Sign(dY) };
		}

		return tail;
	}

	private record struct Instruction(Direction Direction, int Steps) {

		public static Instruction Parse(string input) {
			string[] tokens = input.Split(' ');
			Direction direction = tokens[0] switch {
				"L" => Direction.Left,
				"R" => Direction.Right,
				"U" => Direction.Up,
				"D" => Direction.Down,
				_ => throw new ArgumentException(input),
			};
			return new(direction, tokens[1].AsInt());
		}

	};

	private enum Direction { Left, Right, Up, Down }
}
