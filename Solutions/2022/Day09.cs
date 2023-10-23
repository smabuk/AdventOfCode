namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 09: Rope Bridge
/// https://adventofcode.com/2022/day/9
/// </summary>
[Description("Rope Bridge")]
public sealed partial class Day09 {

	[Init]
	public static    void Init(string[] input, params object[]? _) => LoadInstructions(input);
	public static string Part1(string[] input, params object[]? _) => Solution(2).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution(10).ToString();

	private static IEnumerable<Instruction> _instructions = Array.Empty<Instruction>();

	private static void LoadInstructions(string[] input) {
		_instructions = input.Select(i => Instruction.Parse(i));
	}

	private static int Solution(int noOfKnots) {
		const int HEAD = 0;
		HashSet<Point> tailPoints = new();
		Point[] knots= new Point[noOfKnots];
		
		int tailKnot = noOfKnots - 1;
		_ = tailPoints.Add(knots[tailKnot]);

		foreach (Instruction instruction in _instructions) {
			for (int i = 0; i < instruction.Steps; i++) {
				knots[HEAD] = MoveHead(knots[HEAD], instruction.Direction);
				for (int knot = 0; knot < noOfKnots - 1; knot++) {
					knots[knot + 1] = MoveTail(knots[knot], knots[knot+1]);
				}
				_ = tailPoints.Add(knots[tailKnot]);
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

	private enum Direction {
		Left,
		Right,
		Up,
		Down
	}
}
