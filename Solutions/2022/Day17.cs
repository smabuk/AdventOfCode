namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 17: Pyroclastic Flow
/// https://adventofcode.com/2022/day/17
/// </summary>
[Description("Pyroclastic Flow")]
public sealed partial class Day17 {

	private static readonly int CHAMBER_WIDTH = 7;
	private static readonly int NO_OF_SHAPES = 5;
	private static readonly int START_HEIGHT_GAP = 3;
	private static readonly char LEFT = '<';
	private static readonly char ROCK = '#';
	private static readonly char FALLING_ROCK = '@';
	private static readonly char SPACE = '.';

	private static readonly Point[] ROCK_BOTTOM_0 = { new(0, 0), new(1, 0), new(2, 0), new(3, 0) };
	private static readonly Point[] ROCK_BOTTOM_1 = { new(0, 1), new(1, 2), new(2, 1) };
	private static readonly Point[] ROCK_BOTTOM_2 = { new(0, 2), new(1, 2), new(2, 2) };
	private static readonly Point[] ROCK_BOTTOM_3 = { new(0, 3) };
	private static readonly Point[] ROCK_BOTTOM_4 = { new(0, 1), new(1, 1) };

	private static readonly Point[] ROCK_SHAPE_0 = { new(0, 0), new(1, 0), new(2, 0), new(3, 0) };
	private static readonly Point[] ROCK_SHAPE_1 = { new(0, 1), new(1, 2), new(2, 1), new(1, 0), new(1, 1) };
	private static readonly Point[] ROCK_SHAPE_2 = { new(0, 2), new(1, 2), new(2, 2), new(2, 0), new(2, 1) };
	private static readonly Point[] ROCK_SHAPE_3 = { new(0, 3), new(0, 2), new(0, 1), new(0, 0) };
	private static readonly Point[] ROCK_SHAPE_4 = { new(0, 1), new(1, 1), new(0, 0), new(1, 0)};

	private static Rock[] ROCK_SHAPES = {
		new(4, 1, ROCK_SHAPE_0, ROCK_BOTTOM_0),
		new(3, 3, ROCK_SHAPE_1, ROCK_BOTTOM_1),
		new(3, 3, ROCK_SHAPE_2, ROCK_BOTTOM_2),
		new(1, 4, ROCK_SHAPE_3, ROCK_BOTTOM_3),
		new(2, 2, ROCK_SHAPE_4, ROCK_BOTTOM_4),
	};

	public static string Part1(string input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string input) {
		return new Chamber(input)
			.DropRocks(2022)
			.TowerHeight;
	}

	private static long Solution2(string input) {
		return new Chamber(input)
			.DropManyRocks(1_000_000_000_000)
			.CalculatedTowerHeight(1_000_000_000_000);
	}

	private sealed record Chamber {
		private readonly HashSet<Point> rocks = new();
		private readonly List<JetDirection> jetDirections = new();
		private readonly string directions = "";
		private int nextShape = -1;
		private int nextDirection = -1;
		private long repeatShapeCount = 0;
		private long repeatTowerHeight = 0;

		public Chamber(string directions) {
			this.directions = directions;
			jetDirections = directions
				.Select(i => i == LEFT ? JetDirection.Left : JetDirection.Right)
				.ToList();
			for (int x = 0; x < CHAMBER_WIDTH; x++) {
				rocks.Add(new(x, -1));
			}
		}

		public Chamber DropRocks(long noOfrocks) {

			Rock fallingRock = NextRock();

			for (long i = 0; i < noOfrocks; i++) {
				while (true) {
					JetDirection direction = NextJetDirection();
					if (WillNotCollide(fallingRock, direction)) {
						fallingRock = fallingRock.MoveSideways(direction);
					}
					if (UnableToDrop(fallingRock)) {
						rocks.UnionWith(fallingRock.Rocks);
						fallingRock = NextRock();
						break;
					} else {
						fallingRock = fallingRock.DropDownOne();
					}
				}
			}

			return this;
		}

		public Chamber DropManyRocks(long noOfrocks) {

			Rock fallingRock = NextRock();

			for (int tryShape = 0; tryShape < NO_OF_SHAPES; tryShape++) {
				long firstShapeIteration = 0;
				long firstTowerHeight = 0;
				repeatShapeCount = 0;
				repeatTowerHeight = 0;
				bool firstTime = true;
				long i;
				for (i = 0; i < long.Clamp(noOfrocks, 1, NO_OF_SHAPES * jetDirections.Count); i++) {
					while (true) {
						JetDirection direction = NextJetDirection();

						if (nextDirection == 1 && nextShape == tryShape) {
							if (firstTime is true) {
								if (i > 3) {
									firstShapeIteration = i;
									firstTowerHeight = TowerHeight;
									firstTime = false;
								}
							} else if (repeatShapeCount == 0) {
								// my real input repeats here
								repeatShapeCount = i - firstShapeIteration;
								repeatTowerHeight = TowerHeight - firstTowerHeight;
								return this;
							}
						}

						if (WillNotCollide(fallingRock, direction)) {
							fallingRock = fallingRock.MoveSideways(direction);
						}
						if (UnableToDrop(fallingRock)) {
							rocks.UnionWith(fallingRock.Rocks);
							fallingRock = NextRock();
							break;
						} else {
							fallingRock = fallingRock.DropDownOne();
						}
					}
				}
				if (i >= noOfrocks) {
					return this;
				}
			}

			return this;
		}

		public bool UnableToDrop(Rock rock) =>
			rock.RockBottoms.Select(r => r with { Y = r.Y - 1 }).Intersect(rocks).Any();

		public bool WillNotCollide(Rock rock, JetDirection direction) =>
			!rock.Rocks.Select(r => r with { X = r.X + (int)direction }).Intersect(rocks).Any();

		public long CalculatedTowerHeight(long noOfrocks) {
			long value = noOfrocks / repeatShapeCount * repeatTowerHeight;
			long moreDrops = noOfrocks % repeatShapeCount;
			return value + new Chamber(directions).DropRocks(moreDrops).TowerHeight;
		}

		private Rock NextRock() {
			nextShape = (nextShape + 1) % NO_OF_SHAPES;
			Rock rock = ROCK_SHAPES[nextShape];
			return rock with { Y = TowerHeight + START_HEIGHT_GAP + (rock.Height - 1) };
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

	private sealed record Rock(int X, int Y) {
		List<Point> _rocks = new();
		public List<Point> _rockBottoms = new();
		public int Width  = 0;
		public int Height = 0;

		public Rock(int Width, int Height, Point[] RockPoints, Point[] RockBottomPoints) : this(2, 0) {
			this.Width  = Width;
			this.Height = Height;
			_rocks = RockPoints.ToList();
			_rockBottoms = RockBottomPoints.ToList();
		}

		public IEnumerable<Point> Rocks       => _rocks      .Select(r => r with { X = r.X + X, Y = Y - r.Y });
		public IEnumerable<Point> RockBottoms => _rockBottoms.Select(r => r with { X = r.X + X, Y = Y - r.Y });

		public Rock MoveSideways(JetDirection direction) => this with { X = int.Clamp(X + (int)direction, 0, CHAMBER_WIDTH - Width) };
		public Rock DropDownOne() => this with { Y = Y - 1 };
	};

	private enum JetDirection {
		Left = -1,
		Right = 1,
	}
}
