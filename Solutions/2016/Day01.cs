using static AdventOfCode.Solutions._2016.Day01Types;
using static AdventOfCode.Solutions._2016.Day01Constants;
namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 01: No Time for a Taxicab
/// https://adventofcode.com/2016/day/01
/// </summary>
[Description("No Time for a Taxicab")]
public sealed partial class Day01 {

	public static string Part1(string[] input) => Solution1(input).ToString();
	public static string Part2(string[] input) => Solution2(input).ToString();

	private static int Solution1(string[] input) =>
		input[0]
		.TrimmedSplit(",")
		.As<Instruction>()
		.Aggregate(new Person(Direction.North, START), (person, instruction) => person.Move(instruction))
		.Position
		.ManhattanDistance(START);

	private static int Solution2(string[] input) {
		HashSet<Point> locations = [START];
		List<Instruction> instructions = [.. input[0].TrimmedSplit(",").As<Instruction>()];
		Person me = new(Direction.North, START);

		foreach (Instruction instruction in instructions) {
			Person newMe = me.Move(instruction);
			int dX = Math.Sign(newMe.Position.X - me.Position.X);
			int dY = Math.Sign(newMe.Position.Y - me.Position.Y);

			for (int i = 1; i <= instruction.Value; i++) {
				Point newLocation = me.Position with { X = me.Position.X + (dX * i) , Y = me.Position.Y +  (dY * i)};
				if (locations.Contains(newLocation)) {
					return newLocation.ManhattanDistance(START);
				}

				_ = locations.Add(newLocation);
			}

			me = newMe;
		}

		throw new ApplicationException("Should never reach here");
	}
}


file static class Day01Extensions
{
	public static Person Move(this Person me, Instruction instruction)
	{
		Direction newFacing = (me.Facing, instruction.TurnDirection) switch
		{
			(Direction.West, Direction.R) => Direction.North,
			(Direction.North, Direction.R) => Direction.East,
			(Direction.East, Direction.R) => Direction.South,
			(Direction.South, Direction.R) => Direction.West,
			(Direction.West, Direction.L) => Direction.South,
			(Direction.North, Direction.L) => Direction.West,
			(Direction.East, Direction.L) => Direction.North,
			(Direction.South, Direction.L) => Direction.East,
			_ => throw new NotImplementedException(),
		};

		return me with
		{
			Facing = newFacing,
			Position = newFacing switch
			{
				(Direction.West) => me.Position.MoveWest(instruction.Value),
				(Direction.East) => me.Position.MoveEast(instruction.Value),
				(Direction.North) => me.Position.MoveNorth(instruction.Value),
				(Direction.South) => me.Position.MoveSouth(instruction.Value),

				_ => throw new NotImplementedException(),
			},
		};
	}
}

file sealed partial class Day01Types
{
	public sealed record Instruction(Direction TurnDirection, int Value) : IParsable<Instruction>
	{
		public static Instruction Parse(string s, IFormatProvider? provider)
			=> new($"{s[0]}".AsEnum<Direction>(), s[1..].As<int>());

		public static Instruction Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Instruction result)
			=> ISimpleParsable<Instruction>.TryParse(s, provider, out result);
	}

	public record Person(Direction Facing, Point Position);
}

file static class Day01Constants
{
	public readonly static Point START = Point.Zero;
}
