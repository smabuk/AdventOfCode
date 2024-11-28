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
				.ToUpperInvariant()
				.TrimmedSplit(COMMA)
				.Select(Enum.Parse<Direction>)];

	private static int Solution1() => _directions.Travel().Last().Distance;
	private static int Solution2() => _directions.Travel().Max(h => h.Distance);
}

file static class Day11Extensions
{
	public static IEnumerable<HexCoordinate> Travel(this IEnumerable<Direction> directions)
	{
		HexCoordinate current = new(0, 0, 0);

		foreach (Direction direction in directions) {
			yield return current = current.Step(direction);
		}
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

	/// <summary>
	/// 
	/// </summary>
	/// <param name="X">Represents the NW <--> SE axis</param>
	/// <param name="Y">Represents the  N <--> S  axis</param>
	/// <param name="Z">Represents the NE <--> SW axis</param>
	public record HexCoordinate(int X, int Y, int Z)
	{
		public int Distance => (int.Abs(X) + int.Abs(Y) + int.Abs(Z)) / 2;

		public HexCoordinate Step(Direction direction)
		{
			return direction switch
			{
				Direction.NW => this with { X = X - 1, Y = Y + 1, Z = Z + 0 },
				Direction.N  => this with { X = X + 0, Y = Y + 1, Z = Z - 1 },
				Direction.NE => this with { X = X + 1, Y = Y + 0, Z = Z - 1 },
				Direction.SW => this with { X = X - 1, Y = Y + 0, Z = Z + 1 },
				Direction.S  => this with { X = X + 0, Y = Y - 1, Z = Z + 1 },
				Direction.SE => this with { X = X + 1, Y = Y - 1, Z = Z + 0 },
				_ => throw new NotImplementedException(),
			};
		}
	}
}

file static class Day11Constants
{
	public const char COMMA = ',';
}
