namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 01: No Time for a Taxicab
/// https://adventofcode.com/2016/day/01
/// </summary>
[Description("No Time for a Taxicab")]
public sealed partial class Day01 {

	public static string Part1(string[] input) => Solution1(input).ToString();
	public static string Part2(string[] input) => Solution2(input).ToString();

	private static readonly Point _start = new(0, 0);

	private static int Solution1(string[] input) =>
		input[0]
		.TrimmedSplit(",")
		.As<Instruction>()
		.Aggregate(new Person(Direction.North, _start), (person, instruction) => Move(person, instruction))
		.Position
		.ManhattanDistance(_start);

	private static int Solution2(string[] input) {
		HashSet<Point> locations = [_start];
		List<Instruction> instructions = [.. input[0].TrimmedSplit(",").As<Instruction>()];
		Person me = new(Direction.North, _start);

		foreach (Instruction instruction in instructions) {
			Person newMe = Move(me, instruction);
			int dX = Math.Sign(newMe.Position.X - me.Position.X);
			int dY = Math.Sign(newMe.Position.Y - me.Position.Y);

			for (int i = 1; i <= instruction.Value; i++) {
				Point newLocation = me.Position with { X = me.Position.X + (dX * i) , Y = me.Position.Y +  (dY * i)};
				if (locations.Contains(newLocation)) {
					return newLocation.ManhattanDistance(_start);
				}
				_ = locations.Add(newLocation);
			}
			me = newMe;
		}

		throw new ApplicationException("Should never reach here");
	}

	private static Person Move(Person me, Instruction instruction)
	{
		Direction newFacing = (me.Facing, instruction.TurnDirection) switch
		{
			(Direction.West,  Direction.R) => Direction.North,
			(Direction.North, Direction.R) => Direction.East,
			(Direction.East,  Direction.R) => Direction.South,
			(Direction.South, Direction.R) => Direction.West,
			(Direction.West,  Direction.L) => Direction.South,
			(Direction.North, Direction.L) => Direction.West,
			(Direction.East,  Direction.L) => Direction.North,
			(Direction.South, Direction.L) => Direction.East,
			_ => throw new NotImplementedException(),
		};

		return me with
		{
			Facing = newFacing,
			Position = newFacing switch
			{
				(Direction.West)  => me.Position with { X = me.Position.X - instruction.Value },
				(Direction.East)  => me.Position with { X = me.Position.X + instruction.Value },
				(Direction.North) => me.Position with { Y = me.Position.Y - instruction.Value },
				(Direction.South) => me.Position with { Y = me.Position.Y + instruction.Value },

				_ => throw new NotImplementedException(),
			},
		};
	}

	private sealed record Instruction(Direction TurnDirection, int Value) : IParsable<Instruction> {
		public static Instruction Parse(string s, IFormatProvider? provider)
		{
			return new((Direction)Enum.Parse(typeof(Direction), s[0].ToString()), s[1..].As<int>());
		}

		public static Instruction Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Instruction result)
			=> ISimpleParsable<Instruction>.TryParse(s, provider, out result);
	}


	private record Person(Direction Facing, Point Position);

	private enum Direction
	{
		West,
		East,
		North,
		South,

		L,
		R,
	}

}
