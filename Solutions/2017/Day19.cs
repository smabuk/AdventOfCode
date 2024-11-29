using static Smab.Helpers.ArrayHelpers;
using static AdventOfCode.Solutions._2017.Day19Constants;
using static AdventOfCode.Solutions._2017.Day19Types;

using Direction = Smab.Helpers.Point;

namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 19: A Series of Tubes
/// https://adventofcode.com/2017/day/19
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
		Direction direction = new(DOWN);
		Point current = diagram.WalkWithValues().Single(cell => cell.Y == 0 && cell.Value == VERTICAL).Index + direction;

		while (diagram.TryGetValue(current, out char value) && value != SPACE) {
			steps++;

			if (char.IsAsciiLetter(value)) {
				path = $"{path}{value}";
			}

			direction = value switch
			{
				JOIN when direction.X is 0
					=> diagram.TryGetValue(current.Left(), out value) && value is not (VERTICAL or SPACE)
						? new(LEFT)
						: new(RIGHT),
				JOIN when direction.Y is 0
					=> diagram.TryGetValue(current.Up(), out value) && value is not (HORIZONTAL or SPACE)
						? new(UP)
						: new(DOWN),
				_ => direction,
			};

			current += direction;
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
