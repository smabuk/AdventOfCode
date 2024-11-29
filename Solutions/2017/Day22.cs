using static AdventOfCode.Solutions._2017.Day22Constants;
using static AdventOfCode.Solutions._2017.Day22Types;
using static AdventOfCode.Solutions._2017.Day22Types.Direction;
using static AdventOfCode.Solutions._2017.Day22Types.State;

namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 22: Sporifica Virus
/// https://adventofcode.com/2017/day/22
/// </summary>
[Description("Sporifica Virus")]
public sealed partial class Day22 {

	public static string Part1(string[] input, params object[]? args)
	{
		int noOfBursts = GetArgument<int>(args, argumentNumber: 1, defaultResult: 10_000);
		return Solution1(input, noOfBursts).ToString();
	}

	public static string Part2(string[] input, params object[]? args)
	{
		int noOfBursts = GetArgument<int>(args, argumentNumber: 1, defaultResult: 10_000_000);
		return Solution2(input, noOfBursts).ToString();
	}

	private static int Solution1(string[] input, int noOfBursts)
	{
		return input.ParseInput().InfectedBursts(
			noOfBursts,
			new(new Point((input.Length - 1) / 2, (input.Length - 1) / 2), Up), 
			(State state) => state.FlipState(),
			(Carrier carrier, State state) => carrier.Turn(state)
			);
	}


	private static int Solution2(string[] input, int noOfBursts) {
		return input.ParseInput().InfectedBursts(
			noOfBursts,
			new(new Point((input.Length - 1) / 2, (input.Length - 1) / 2), Up),
			(State state) => state.NextState(),
			(Carrier carrier, State state) => carrier.Turn(state)
			);
	}
}

file static class Day22Extensions
{
	public static int InfectedBursts(
		this Dictionary<Point, State> nodes,
		int noOfBursts,
		Carrier current,
		Func<State, State> nextState,
		Func<Carrier, State, Carrier> nextDirection)
	{
		int infectionBursts = 0;
		for (int burst = 0; burst < noOfBursts; burst++) {
			State state = nodes.GetValueOrDefault(current.Position, Clean);
			current = nextDirection(current, state);

			State newState = nextState(state);
			if (newState is Infected) {
				infectionBursts++;
			}

			nodes[current.Position] = newState;
			current = current.Move();
		}

		return infectionBursts;
	}

	public static Carrier Move(this Carrier carrier)
	{
		return carrier with
		{
			Position = carrier.Direction switch
			{
				Up    => carrier.Position.Up() ,
				Right => carrier.Position.Right() ,
				Down  => carrier.Position.Down() ,
				Left  => carrier.Position.Left() ,
				_ => throw new NotImplementedException(),
			}
		};
	}


	public static Carrier Turn(this Carrier carrier, State state)
	{
		return state switch
		{
			Clean    => carrier.TurnLeft(),
			Weakened => carrier,
			Infected => carrier.TurnRight(),
			Flagged  => carrier.TurnAround(),
			_ => throw new NotImplementedException(),
		};
	}

	public static Carrier TurnRight(this Carrier carrier)
	{
		return carrier with
		{
			Direction = carrier.Direction switch
			{
				Up    => Right,
				Right => Down,
				Down  => Left,
				Left  => Up,
				_ => throw new NotImplementedException(),
			}
		};
	}

	public static Carrier TurnLeft(this Carrier carrier)
	{
		return carrier with
		{
			Direction = carrier.Direction switch
			{
				Up    => Left,
				Right => Up,
				Down  => Right,
				Left  => Down,
				_ => throw new NotImplementedException(),
			}
		};
	}

	public static Carrier TurnAround(this Carrier carrier)
	{
		return carrier with
		{
			Direction = carrier.Direction switch
			{
				Up    => Down,
				Right => Left,
				Down  => Up,
				Left  => Right,
				_ => throw new NotImplementedException(),
			}
		};
	}
	public static State FlipState(this State state)
	{
		return state switch
			{
				Clean    => Infected,
				Infected => Clean,
				_ => throw new NotImplementedException(),
			};
	}

	public static State NextState(this State state)
	{
		return state switch
			{
				Clean    => Weakened,
				Weakened => Infected,
				Infected => Flagged,
				Flagged  => Clean,
				_ => throw new NotImplementedException(),
			};
	}

	public static Dictionary<Point, State> ParseInput(this string[] input)
	{
		return input
			.Index()
			.SelectMany(row =>
					row.Item
					.Index()
					.Select(col => new Node(new(col.Index, row.Index), col.Item == INFECTED ? Infected : Clean)))
			.ToDictionary(n => n.Position, n => n.State);
	}
}

internal sealed partial class Day22Types
{
	public enum Direction
	{
		Up,
		Right,
		Down,
		Left,
	}

	public enum State
	{
		Clean,
		Weakened,
		Infected,
		Flagged,
	}

	public sealed record Node(Point Position, State State);
	public sealed record Carrier(Point Position, Direction Direction);
}

file static class Day22Constants
{
	public const char INFECTED = '#';
}
