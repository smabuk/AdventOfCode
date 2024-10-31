namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day XX: Title
/// https://adventofcode.com/2016/day/01
/// </summary>
[Description("No Time for a Taxicab")]
public sealed partial class Day01 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();


	private static int Solution1(string[] input) {
		List<Instruction> instructions = [.. input[0].TrimmedSplit(",").As<Instruction>()];

		Point start = new(0, 0);
		Person me = new(Direction.Up,	start);

		foreach (var instruction in instructions) {
			Direction newFacing = (me.Facing, instruction.TurnDirection) switch
			{
				(Direction.Left,  Direction.R) => Direction.Up,
				(Direction.Up,    Direction.R) => Direction.Right,
				(Direction.Right, Direction.R) => Direction.Down,
				(Direction.Down,  Direction.R) => Direction.Left,
				(Direction.Left,  Direction.L) => Direction.Down,
				(Direction.Up,    Direction.L) => Direction.Left,
				(Direction.Right, Direction.L) => Direction.Up,
				(Direction.Down,  Direction.L) => Direction.Right,
				_ => throw new NotImplementedException(),
			};
			me = me with {
				Facing = newFacing,
				Position = newFacing switch
				{
					(Direction.Left)  => me.Position with { X = me.Position.X - instruction.Value },
					(Direction.Right) => me.Position with { X = me.Position.X + instruction.Value },
					(Direction.Up)    => me.Position with { Y = me.Position.Y - instruction.Value },
					(Direction.Down)  => me.Position with { Y = me.Position.Y + instruction.Value },

					_ => throw new NotImplementedException(),
				},
			};
		}

		return me.Position.ManhattanDistance(start);
	}

	private static string Solution2(string[] input) {
		//List<Instruction> instructions = [.. input[0].TrimmedSplit(",").As<Instruction>()];
		return "** Solution not written yet **";
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
		Left,
		Right,
		Up,
		Down,

		L = Left,
		R = Right,
	}

}
