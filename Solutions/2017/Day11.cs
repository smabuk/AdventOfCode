using static AdventOfCode.Solutions._2017.Day11Constants;
using static AdventOfCode.Solutions._2017.Day11Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 11: Hex Ed
/// https://adventofcode.com/2016/day/11
/// </summary>
[Description("Hex Ed")]
public sealed partial class Day11 {

	[Init]
	public static   void  Init(string[] input) => LoadDirections(input);
	public static string Part1(string[] _) => Solution1().ToString();
	public static string Part2(string[] _) => Solution2().ToString();

	private static List<Direction> _directions = [];

	private static void LoadDirections(string[] input) => 
		_directions = [
			.. input[0]
				.TrimmedSplit(',')
				.Select(d => Enum.Parse<Direction>(d.ToUpperInvariant()))];

	private static int Solution1() => _directions.Move().Last().Distance;

	private static string Solution2() {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class Day11Extensions
{
	public static IEnumerable<HexCoordinate> Move(this IEnumerable<Direction> directions)
	{
		HexCoordinate current = new(0, 0, 0);

		foreach (var direction in directions) {
			current = current.Move(direction);
			yield return current;
		}
	} 

	public static HexCoordinate Move(this HexCoordinate hexCoordinate, Direction direction)
	{
		return direction switch
		{
			Direction.NW => hexCoordinate with { X = hexCoordinate.X - 1, Y = hexCoordinate.Y + 1, Z = hexCoordinate.Z + 0 },
			Direction.N  => hexCoordinate with { X = hexCoordinate.X + 0, Y = hexCoordinate.Y + 1, Z = hexCoordinate.Z - 1 },
			Direction.NE => hexCoordinate with { X = hexCoordinate.X + 1, Y = hexCoordinate.Y + 0, Z = hexCoordinate.Z - 1 },
			Direction.SW => hexCoordinate with { X = hexCoordinate.X - 1, Y = hexCoordinate.Y + 0, Z = hexCoordinate.Z + 1 },
			Direction.S  => hexCoordinate with { X = hexCoordinate.X + 0, Y = hexCoordinate.Y - 1, Z = hexCoordinate.Z + 1 },
			Direction.SE => hexCoordinate with { X = hexCoordinate.X + 1, Y = hexCoordinate.Y - 1, Z = hexCoordinate.Z + 0 },
			_ => throw new NotImplementedException(),
		};
	} 
}

internal sealed partial class Day11Types
{
	public enum Direction
	{
		None = 0,
		NW,
		N,
		NE,
		SE,
		S,
		SW
	}

	public record HexCoordinate(int X, int Y, int Z)
	{
		public int Distance => (int.Abs(X) + int.Abs(Y) + int.Abs(Z)) / 2;
	}
}

file static class Day11Constants
{
}
