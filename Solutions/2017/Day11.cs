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
	public static   void  Init(string[] input) => LoadAndTravel(input);
	public static string Part1(string[] _) => Solution1().ToString();
	public static string Part2(string[] _) => Solution2().ToString();

	private static List<HexCoordinate> _hexCoordinates = [];

	private static void LoadAndTravel(string[] input) => 
		_hexCoordinates = [
			.. input[0]
				.ToUpperInvariant()
				.TrimmedSplit(COMMA)
				.Select(Enum.Parse<Direction>)
				.Travel()
			];

	private static int Solution1() => _hexCoordinates.Last().Distance;
	private static int Solution2() => _hexCoordinates.Max(h => h.Distance);
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
	///    \  n  /
	///  nw +---+ ne
	/// nw /     \ ne
	///  -+       +-
	/// sw \     / se
	///  sw +---+ se
	///    /  s \
	/// 
	/// </summary>
	/// <param name="X">Represents the NW <--> SE axis</param>
	/// <param name="Y">Represents the  N <--> S  axis</param>
	/// <param name="Z">Represents the NE <--> SW axis</param>
	/// <seealso cref="https://www.redblobgames.com/grids/hexagons-v2/pre-index.html#coordinates"/>
	public record HexCoordinate(int X, int Y, int Z)
	{
		/// <summary>
		/// One step towards any other hexagon requires 2 values to change 
		/// so remember to divide by 2 to get the actual value
		/// </summary>
		public int Distance => (int.Abs(X) + int.Abs(Y) + int.Abs(Z)) / 2;

		public HexCoordinate Step(Direction direction)
		{
			return direction switch
			{
				Direction.N  => this with { X = X + 0, Y = Y + 1, Z = Z - 1 },
				Direction.S  => this with { X = X + 0, Y = Y - 1, Z = Z + 1 },

				Direction.NE => this with { X = X + 1, Y = Y + 0, Z = Z - 1 },
				Direction.SW => this with { X = X - 1, Y = Y + 0, Z = Z + 1 },

				Direction.NW => this with { X = X - 1, Y = Y + 1, Z = Z + 0 },
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
