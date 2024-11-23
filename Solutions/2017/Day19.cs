using static Smab.Helpers.ArrayHelpers;
using static AdventOfCode.Solutions._2017.Day19Constants;
using static AdventOfCode.Solutions._2017.Day19Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 19: A Series of Tubes
/// https://adventofcode.com/2016/day/19
/// </summary>
[Description("A Series of Tubes")]
public sealed partial class Day19 {

	[Init]
	public static   void  Init(string[] input, Action<string[], bool>? visualise = null) => LoadRoutingDiagram(input, visualise);
	public static string Part1(string[] _) => Solution1().ToString();
	public static string Part2(string[] _) => Solution2().ToString();

	private static char[,] _diagram = new char[0,0];

	private static void LoadRoutingDiagram(string[] input, Action<string[], bool>? visualise = null)
	{
		_diagram = input.To2dArray();
		_diagram.VisualiseGrid("Diagram", visualise);
	}

	private static string Solution1() => _diagram.Path().Path;

	private static int Solution2() => _diagram.Path().Steps;
}

file static class Day19Extensions
{
	public static (string Path, int Steps) Path(this char[,] diagram)
	{
		int steps = 1;
		string path = "";
		Cell<char> current = diagram.WalkWithValues().Single(cell => cell.Y == 0 && cell.Value == VERTICAL);
		Point direction = new(DOWN);

		while (diagram.IsInBounds(current)) {
			if (char.IsAsciiLetter(current.Value)) {
				path = $"{path}{current.Value}";
			}

			if (current.Value == JOIN) {
				if (direction.X == 0) {
					Point possible = current.Index.Left();
					direction = diagram.IsInBounds(possible) && diagram[possible.X, possible.Y] is not (VERTICAL or SPACE)
						? new(LEFT)
						: new(RIGHT);
				} else {
					Point possible = current.Index.Up();
					direction = diagram.IsInBounds(possible) && diagram[possible.X, possible.Y] is not (HORIZONTAL or SPACE)
						? new(UP)
						: new(DOWN);
				}
			}

			Point next = current.Index + direction;

			if (diagram.IsOutOfBounds(next)) {
				break;
			}

			current = diagram.GetAdjacentCells(current).Single(cell => cell.Index == next);

			if (diagram[current.X, current.Y] == SPACE) {
				break;
			}

			steps++;
		}

		return (path, steps);
	}
}

internal sealed partial class Day19Types
{
}

file static class Day19Constants
{
	public const char VERTICAL   = '|';
	public const char HORIZONTAL = '-';
	public const char JOIN       = '+';
	public const char SPACE      = ' ';
}
