namespace AdventOfCode.Solutions._2018;

/// <summary>
/// Day 13: Mine Cart Madness
/// https://adventofcode.com/2018/day/13
/// </summary>
[Description("Mine Cart Madness")]
public sealed partial class Day13 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private const char STRAIGHT_UP_DOWN = '|';
	private const char STRAIGHT_LEFT_RIGHT = '-';
	private const char CURVE_1 = '/';
	private const char CURVE_2 = '\\';
	private const char INTERSECTION = '+';

	private const char CART_UP = '^';
	private const char CART_DOWN = 'v';
	private const char CART_LEFT = '<';
	private const char CART_RIGHT = '>';
	private const char CART_CRASH = 'X';

	private static readonly char[] CART = [CART_UP, CART_DOWN, CART_LEFT, CART_RIGHT, CART_CRASH];
	private static readonly char[] TRACK = [STRAIGHT_UP_DOWN, STRAIGHT_LEFT_RIGHT, CURVE_1, CURVE_2, INTERSECTION];

	private static string Solution1(string[] input)
	{
		LoadMinesAndCarts(input, out List<Cart> carts, out char[,] tracks);

		int ticks = 0;
		bool isCrashed = false;
		Point crashLocation = new();
		do {
			List<Cart> orderedCarts = carts
				.OrderBy(c => c.Position.Y)
				.ThenBy(c => c.Position.X)
				.ToList();
			for (int cartIndex = 0; cartIndex < orderedCarts.Count; cartIndex++) {
				Cart cart = orderedCarts[cartIndex];
				cart = cart.Move(tracks);
				orderedCarts[cartIndex] = cart;
				isCrashed = orderedCarts.GroupBy(c => c.Position).Where(p => p.Count() > 1).Any();
				if (isCrashed) {
					crashLocation = cart.Position;
					break;
				}
			}
			carts = orderedCarts;
			ticks++;
		} while (!isCrashed);

		return $"{crashLocation.X},{crashLocation.Y}";
	}

	private static string Solution2(string[] input) {
		LoadMinesAndCarts(input, out List<Cart> carts, out char[,] tracks);

		int ticks = 0;
		bool isCrashed = false;
		do {
			List<Cart> orderedCarts = carts
				.OrderBy(c => c.Position.Y)
				.ThenBy(c => c.Position.X)
				.ToList();
			for (int cartIndex = 0; cartIndex < orderedCarts.Count; cartIndex++) {
				Cart cart = orderedCarts[cartIndex];
				cart = cart.Move(tracks);
				orderedCarts[cartIndex] = cart;
				isCrashed = orderedCarts.Where(c => c.IsCrashed is false).GroupBy(c => c.Position).Where(p => p.Count() > 1).Any();
				if (isCrashed) {
					for (int i = 0; i < orderedCarts.Count; i++) {
						if (orderedCarts[i].Position == cart.Position && orderedCarts[i].IsCrashed is false) {
							orderedCarts[i].IsCrashed = true;
						}
					}
				}
			}
			carts = orderedCarts.Where(c => c.IsCrashed is false).ToList();
			ticks++;
		} while (carts.Count > 1);

		Point remainingCart = carts.Single().Position;

		return $"{remainingCart.X},{remainingCart.Y}";
	}

	private static void LoadMinesAndCarts(string[] input, out List<Cart> carts, out char[,] tracks)
	{
		char[,] mines = String.Join("", input).To2dArray<char>(input[0].Length);
		carts = mines
			.Walk2dArrayWithValues()
			.Where(m => CART.Contains(m.Value))
			.Select(m => new Cart(
				new(m.X, m.Y),
				m.Value switch
				{
					CART_UP => Direction.Up,
					CART_DOWN => Direction.Down,
					CART_LEFT => Direction.Left,
					CART_RIGHT => Direction.Right,
					_ => throw new NotImplementedException(),
				}))
			.ToList();
		tracks = (char[,])mines.Clone();
		foreach (Cart cart in carts) {
			tracks[cart.Position.X, cart.Position.Y] = cart.Facing switch
			{
				Direction.Left or Direction.Right => STRAIGHT_LEFT_RIGHT,
				Direction.Up or Direction.Down => STRAIGHT_UP_DOWN,
				_ => throw new NotImplementedException(),
			};
		}
	}

	private record Cart(Point Position, Direction Facing) {

		public bool IsCrashed { get; set; } = false;
		public Turn NextIntersectionDirection { get; private set; } = Turn.Left;

		public Cart Move(char[,] tracks)
		{
			if (IsCrashed) {
				return this;
			}
			Point position = Facing switch
			{
				Direction.Left => Position.West(),
				Direction.Right => Position.East(),
				Direction.Up => Position.North(),
				Direction.Down => Position.South(),
				_ => throw new NotImplementedException(),
			};

			char track = tracks[position.X, position.Y];
			Direction facing = track switch
			{
				CURVE_1 => Facing switch
				{
					Direction.Left  => Direction.Down,
					Direction.Right => Direction.Up,
					Direction.Up    => Direction.Right,
					Direction.Down  => Direction.Left,
					_ => throw new NotImplementedException(),
				},
				CURVE_2 => Facing switch
				{
					Direction.Left  => Direction.Up,
					Direction.Right => Direction.Down,
					Direction.Up    => Direction.Left,
					Direction.Down  => Direction.Right,
					_ => throw new NotImplementedException(),
				},
				INTERSECTION => TurnAtIntersection(),
				_ => Facing,
			};

			return this with { Position = position, Facing = facing };
		}

		private Direction TurnAtIntersection()
		{
			Direction direction = Facing;

			direction = Facing switch
			{
				Direction.Left => NextIntersectionDirection switch
				{
					Turn.Left => Direction.Down,
					Turn.Right => Direction.Up,
					_ => direction,
				},
				Direction.Right => NextIntersectionDirection switch
				{
					Turn.Left => Direction.Up,
					Turn.Right => Direction.Down,
					_ => direction,
				},
				Direction.Up => NextIntersectionDirection switch
				{
					Turn.Left => Direction.Left,
					Turn.Right => Direction.Right,
					_ => direction,
				},
				Direction.Down => NextIntersectionDirection switch
				{
					Turn.Left => Direction.Right,
					Turn.Right => Direction.Left,
					_ => direction,
				},
				_ => direction,
			};

			NextIntersectionDirection = NextIntersectionDirection switch
			{
				Turn.Left => Turn.Straight,
				Turn.Straight => Turn.Right,
				Turn.Right => Turn.Left,
				_ => throw new NotImplementedException(),
			};

			return direction;
		}
	}

	private enum Direction { Left, Right, Up, Down }
	private enum Turn { Left, Straight, Right }
}
