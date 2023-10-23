namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 24: Blizzard Basin
/// https://adventofcode.com/2022/day/24
/// </summary>
[Description("Blizzard Basin")]
public sealed partial class Day24 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();


	const char EXPEDITION = 'E';
	const char GROUND = '.';
	const char WALL = '#';
	const char UP = '^';
	const char DOWN = 'v';
	const char LEFT = '<';
	const char RIGHT = '>';
	private static readonly char[] WIND_DIRECTION = { UP, DOWN, LEFT, RIGHT, 'w' };


	private static int Solution1(string[] input) {
		Basin basin = Basin.Parse(input);

		int minutes = int.MaxValue;
		for (int i = 0; i < 100; i++) {
			minutes = Math.Min(DrunkenWalk(basin, basin.Start).Count(), minutes);
			Debug.WriteLine($"{i} - Minutes = {minutes}");
		}
		return minutes;
	}

	private static string Solution2(string[] input) {
		Basin basin = Basin.Parse(input);
		return "** Solution not written yet **";
	}

	private static IEnumerable<Basin> DrunkenWalk(Basin basin, Point expedition) {
		Random TrueOrFalse = new();

		List<Point> oldWinds = new();
		while (expedition != basin.End
			&& basin.Minute < 800 /* Too slow */
			) {
			oldWinds = basin.Winds.Select(w => w.Position).ToList();
			basin = basin.NextMinute();

			foreach (Point newPosition in expedition.Adjacent()) {
				if (CanWeMove(newPosition)
					&& ShouldWeMoveRandomly(expedition, newPosition)
					) {
						expedition = newPosition;
						break;
				}
			}
			//basin.DebugPrint(expedition);
			yield return basin with { Minute = basin.Minute + 1};

		}

		bool CanWeMove(Point newPosition) {
			if (newPosition == basin.End) {
				return true;
			}

			if (newPosition.X < 1
				|| newPosition.X > basin.Width - 2
				|| newPosition.Y < 1
				|| newPosition.Y > basin.Height - 2
				) {
				return false;
			}

			if (oldWinds.Contains(newPosition)) {
				return false;
			}
			return true;
		}

		bool ShouldWeMoveRandomly (Point expedition, Point newPosition, int weight = 2) {
			if (newPosition == basin.End) {
				return true;
			}
			return (newPosition - expedition) switch {
				{ X: 1 } or { Y: 1 } => TrueOrFalse.Next(2) == 1,
				_ => TrueOrFalse.Next(10) == 1,
			};
		}

	}




	private record Basin(Point Start, Point End, int Width, int Height, IEnumerable<Wind> Winds, int Minute, Dictionary<int, HashSet<Wind>>? WindStates = null) : IParsable<Basin> {
		private int RightWallPosition  => Width  - 1;
		private int BottomWallPosition => Height - 1;
		private int InteriorSpace => (Width - 2) * (Height - 2);

		public Basin NextMinute() {
			if (WindStates is not null) {
				return this with { Winds = WindStates[(Minute + 1) % InteriorSpace], Minute = Minute + 1 };
			}
			throw new NotImplementedException();
		}

		public Basin NextUncachedMinute() {
			List<Wind> newWinds = new();
				foreach (Wind wind in Winds.ToList()) {
				Wind newWind = wind with { Position = wind.Position + Movement(wind.Direction) };
				newWind = newWind.Position switch {
						{ X: 0, Y: int y } => newWind with { Position = new Point(RightWallPosition - 1, y) },
						{ Y: 0, X: int x } => newWind with { Position = new Point(x, BottomWallPosition - 1) },
						{ X: int x, Y: int y} when x == RightWallPosition  => newWind with { Position = new Point(1, y) },
						{ X: int x, Y: int y} when y == BottomWallPosition => newWind with { Position = new Point(x, 1) },
						_ => newWind
					};
				newWinds.Add(newWind);
			}

			return this with { Winds = newWinds, Minute = Minute + 1 };
		}


		public static Basin Parse(string[] s) {
			int height = s.Length;
			int width  = s[0].Length;
			Point start = new(s[0].IndexOf(GROUND), 0);
			Point end   = new(s[^1].IndexOf(GROUND), height - 1);
			IEnumerable<Wind> winds = s
			.SelectMany(
				(i, y) => i.Select((Tile, x) => (Tile, x, y))
				.Where(item => WIND_DIRECTION.Contains(item.Tile))
				.Select(item => new Wind(Position: new Point(item.x, item.y), Direction: item.Tile switch {
					UP    => Direction.up,
					DOWN  => Direction.down,
					LEFT  => Direction.left,
					RIGHT => Direction.right,
					_ => throw new NotImplementedException(),
				}))
			);

			Basin basin = new(start, end, width, height, winds, 0);
			Dictionary<int, HashSet<Wind>> windStates = new() {
				[0] = basin.MakeState
			};
			for (int i = 0; i < (height - 2) * (width - 2); i++) {
				basin = basin.NextUncachedMinute() with { Minute = i};
				windStates[i + 1] = basin.MakeState;
			}

			return new Basin(start, end, width, height, winds, 0, windStates);
		}

		public static Basin Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Basin result) => throw new NotImplementedException();

		public void DebugPrint(Point? expedition = null) {

			Debug.Write($"Minute: {Minute}");
			for (int y =0; y < Height; y++) {
				Debug.WriteLine("");
				for (int x = 0; x < Width; x++) {
					if (x == 0 
						|| x == Width - 1 
						|| (y == 0 && x != Start.X) 
						|| (y == Height - 1 && x != End.X) 
						) {
						Debug.Write(WALL);
					} else {
						if (expedition is not null && expedition == new Point(x, y)) {
							Debug.Write(EXPEDITION);
						} else {
							List<Wind> winds = Winds.Where(w => w.Position == new Point(x, y)).ToList();
							if (winds.Count == 0) {
								Debug.Write(GROUND);
							} else {
								if (winds.Count == 1) {
									Debug.Write(WIND_DIRECTION[(int)winds[0].Direction]);
								} else {
									Debug.Write(winds.Count);
								}
							}
						}
					}
				}
			}
			Debug.WriteLine("");
			Debug.WriteLine("");
		}

		public HashSet<Wind> MakeState => Winds.Select(w => w with { Direction = Direction.who_cares }).ToHashSet();
	}

	private record Wind (Point Position, Direction Direction) {
	}

	private static Point Movement(Direction direction) {
		return direction switch {
			Direction.up    => new( 0, -1),
			Direction.down  => new( 0,  1),
			Direction.left  => new(-1,  0),
			Direction.right => new( 1,  0),
			_ => throw new NotImplementedException(),
		};
	}

	private enum Direction {
		up    = 0,
		down ,
		left ,
		right,
		who_cares,
	}
}
