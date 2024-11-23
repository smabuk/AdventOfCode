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
	public static   void  Init(string[] input) => LoadRoutingDiagram(input);
	public static string Part1(string[] _, Action<string[], bool>? visualise = null) => Solution1(visualise).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static char[,] _diagram = new char[0,0];

	private static void LoadRoutingDiagram(string[] input) => _diagram = input.To2dArray();

	private static string Solution1(Action<string[], bool>? visualise = null) {
		_diagram.VisualiseGrid("Diagram", visualise);

		return _diagram.Path();
	}

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class Day19Extensions
{
	public static string Path(this char[,] diagram)
	{
		string path = "";
		Cell<char> current = diagram.WalkWithValues().Where(cell => cell.Y == 0 && cell.Value == VERTICAL).Single();
		Point direction = new(DOWN);

		while (diagram.IsInBounds(current)) {
			if (char.IsAsciiLetter(current.Value)) {
				path = $"{path}{current.Value}";
			}

			if (current.Value == JOIN) {
				Point possible = new();
				
				if (direction.X == 0) {
					possible = new(current.Index + LEFT);
					direction = diagram.IsInBounds(possible) && diagram[possible.X, possible.Y] is not (VERTICAL or SPACE)
						? new(LEFT)
						: new(RIGHT);
				} else {
					possible = new(current.Index + UP);
					direction = diagram.IsInBounds(possible) && diagram[possible.X, possible.Y] is not (HORIZONTAL or SPACE)
						? new(UP)
						: new(DOWN);
				}
			}

			if (diagram.IsOutOfBounds(current.Index + direction) || diagram[current.X, current.Y] == SPACE) {
				break;
			}

			current = diagram.GetAdjacentCells(current).Where(cell => cell.Index == current.Index + direction).Single();
		}

		return path;
	}
}

internal sealed partial class Day19Types
{
}

file static class Day19Constants
{
	public const char VERTICAL = '|';
	public const char HORIZONTAL = '-';
	public const char JOIN = '+';
	public const char SPACE = ' ';
}
