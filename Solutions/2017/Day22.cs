using static AdventOfCode.Solutions._2017.Day22Constants;
using static AdventOfCode.Solutions._2017.Day22Types;
using static AdventOfCode.Solutions._2017.Day22Types.Direction;

namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 22: Sporifica Virus
/// https://adventofcode.com/2016/day/22
/// </summary>
[Description("Sporifica Virus")]
public sealed partial class Day22 {

	public static string Part1(string[] input, params object[]? args)
	{
		int noOfBursts = GetArgument<int>(args, argumentNumber: 1, defaultResult: 10000);
		return Solution1(input, noOfBursts).ToString();
	}

	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static int Solution1(string[] input, int noOfBursts) {
		Dictionary<Point, bool> nodes =
			input
			.Index()
			.SelectMany(row =>
					row.Item
					.Index()
					.Select(col => new Node(new(col.Index, row.Index), col.Item == INFECTED)))
			.ToDictionary(n => n.Position, n => n.Infected);

		int mid = (input.Length - 1) / 2;
		Carrier current = new(new Point(mid, mid), Up);
		
		int infectionBursts = 0;
		for (int burst = 0; burst < noOfBursts; burst++) {
			bool infected = nodes.GetValueOrDefault(current.Position, false);
			current = infected
				? current.TurnRight()
				: current.TurnLeft();

			if (infected) {
				nodes[current.Position] = false;
			} else {
				nodes[current.Position] = true;
				infectionBursts++;
			}

			current = current.Move();
		}

		return infectionBursts;
	}

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class Day22Extensions
{
	public static Carrier Move(this Carrier carrier)
	{
		return carrier with
		{
			Position = carrier.Direction switch
			{
				Up => carrier.Position.Up() ,
				Right => carrier.Position.Right() ,
				Down => carrier.Position.Down() ,
				Left => carrier.Position.Left() ,
				_ => throw new NotImplementedException(),
			}
		};
	}

	public static Carrier TurnRight(this Carrier carrier)
	{
		return carrier with
		{
			Direction = carrier.Direction switch
			{
				Up => Right,
				Right => Down,
				Down => Left,
				Left => Up,
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
				Up => Left,
				Right => Up,
				Down => Right,
				Left => Down,
				_ => throw new NotImplementedException(),
			}
		};
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

	public sealed record Node(Point Position, bool Infected);
	public sealed record Carrier(Point Position, Direction Direction);
}

file static class Day22Constants
{
	public const char INFECTED = '#';
}
