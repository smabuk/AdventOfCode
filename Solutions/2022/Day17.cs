using System.Runtime.CompilerServices;

namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 17: Pyroclastic Flow
/// https://adventofcode.com/2022/day/17
/// </summary>
[Description("Pyroclastic Flow")]
public sealed partial class Day17 {

	private static readonly int CHAMBER_WIDTH = 7;
	private static readonly int NO_OF_SHAPES = 5;
	private static readonly char LEFT = '<';
	private static readonly char ROCK = '#';
	private static readonly char FALLING_ROCK = '@';
	private static readonly char SPACE = '.';
	private static readonly Point[] ROCK_BOTTOM_0 = { new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0) };
	private static readonly Point[] ROCK_BOTTOM_1 = { new Point(0, 1), new Point(1, 2), new Point(2, 1) };
	private static readonly Point[] ROCK_BOTTOM_2 = { new Point(0, 2), new Point(1, 2), new Point(2, 2) };
	private static readonly Point[] ROCK_BOTTOM_3 = { new Point(0, 3) };
	private static readonly Point[] ROCK_BOTTOM_4 = { new Point(0, 1), new Point(1, 1)};
	private static readonly string ROCK_SHAPE_PATTERN_0 = "####";
	private static readonly string ROCK_SHAPE_PATTERN_1 = """
														.#.
														###
														.#.
														""";
	private static readonly string ROCK_SHAPE_PATTERN_2 = """
														..#
														..#
														###
														""";
	private static readonly string ROCK_SHAPE_PATTERN_3 = """
														#
														#
														#
														#
														""";
	private static readonly string ROCK_SHAPE_PATTERN_4 = """
														##
														##
														""";
	private static Rock[] ROCK_SHAPES = {
		new(ROCK_SHAPE_PATTERN_0, ROCK_BOTTOM_0),
		new(ROCK_SHAPE_PATTERN_1, ROCK_BOTTOM_1),
		new(ROCK_SHAPE_PATTERN_2, ROCK_BOTTOM_2),
		new(ROCK_SHAPE_PATTERN_3, ROCK_BOTTOM_3),
		new(ROCK_SHAPE_PATTERN_4, ROCK_BOTTOM_4),
	};


	public static string Part1(string input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string input) {
		Chamber chamber = new(input);
		chamber = chamber.DropRocks(2022);

		return chamber.TowerHeight;
	}

	private static string Solution2(string input) {
		//string inputLine = input[0];
		//List<string> inputs = input.ToList();
		//List<Instruction> instructions = input.Select(ParseLine).ToList();
		return "** Solution not written yet **";
	}

	private record Chamber {
		private HashSet<Point> rocks = new();
		private List<JetDirection> jetDirections = new();
		private int nextShape = -1;
		private int nextDirection = -1;

		public Chamber(string directions) {
			for (int x = 0; x < CHAMBER_WIDTH; x++) {
				rocks.Add(new (x, -1));
			}
			jetDirections = directions
				.Select(i => i == LEFT ? JetDirection.Left : JetDirection.Right)
				.ToList();
		}

		public Chamber DropRocks(int noOfrocks) {

			Rock fallingRock = NextRock();

			for (int i = 0; i < noOfrocks; i++) {
				while (true) {
					JetDirection direction = NextJetDirection();
					if (fallingRock.Rocks.Select(r => r with { X = r.X + (int)direction }).Intersect(rocks).Count() == 0) {
						fallingRock = fallingRock.Move(direction);
					}
					if (fallingRock.RockBottoms.Select(r => r with { Y = r.Y - 1 }).Intersect(rocks).Any()) { 
						rocks.UnionWith(fallingRock.Rocks);
						fallingRock = NextRock();
						break;
					} else {
						fallingRock = fallingRock.Fall();
					}
				}
			}

			return this;
		}

		private Rock NextRock() {
			nextShape = (nextShape + 1) % NO_OF_SHAPES;
			Rock rock = ROCK_SHAPES[nextShape];
			return rock with { Y = TowerHeight + 3 + (rock.Height - 1) };
		}

		private JetDirection NextJetDirection() {
			nextDirection = (nextDirection + 1) % jetDirections.Count;
			return jetDirections[nextDirection];
		}

		public int TowerHeight => rocks.Max(r => r.Y) + 1;

		public List<string> Print(Rock rock) {
			List<string> message = new();
			Console.WriteLine();
			for (int y = TowerHeight + 5; y >= 0; y--) {
				string line = "";
				for (int x = 0; x < CHAMBER_WIDTH; x++) {
					line += $"{(rocks.Contains(new(x, y)) ? ROCK : rock.Rocks.Contains(new(x, y)) ? FALLING_ROCK : SPACE)}";
				}
				message.Add(line);
				Console.WriteLine(line);
			}
			return message;
		}
	}

	private record Rock(int X, int Y) {
		HashSet<Point> _rocks = new();
		public HashSet<Point> _rockBottoms = new();
		public int Height = 0;
		public int Width = 0;
		char[,] rocksArray = default!;
		public Rock(string ShapeString, Point[] RockBottomPoints) : this(2, 0) {
			Width = ShapeString.Split(Environment.NewLine)[0].Length;
			rocksArray = ShapeString.Replace(Environment.NewLine, "").To2dArray(Width);
			Height = rocksArray.NoOfRows();
			_rocks = rocksArray
				.Walk2dArrayWithValues()
				.Where(p => p.Value == ROCK)
				.Select(p => p.Index)
				.ToHashSet();
			_rockBottoms = RockBottomPoints.ToHashSet();
		}

		public Rock Move(JetDirection direction) => this with { X = int.Clamp(X + (int)direction, 0, CHAMBER_WIDTH - Width) };
		public Rock Fall() => this with { Y = Y - 1 };
		public IEnumerable<Point> Rocks => _rocks.Select(r => new Point(r.X + X, Y - r.Y));
		public IEnumerable<Point> RockBottoms => _rockBottoms.Select(r => new Point(r.X + X, Y - r.Y));


	};


	private enum JetDirection {
		Left = -1,
		Right = 1,
	}
}
