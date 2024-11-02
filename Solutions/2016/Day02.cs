using static AdventOfCode.Solutions._2016.Day02Constants;
using static AdventOfCode.Solutions._2016.Day02Types;
namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 02: Bathroom Security
/// https://adventofcode.com/2016/day/02
/// </summary>
[Description("Bathroom Security")]
public sealed partial class Day02 {

	public static string Part1(string[] input) => Solution1(input).ToString();
	public static string Part2(string[] input) => Solution2(input).ToString();

	private static string Solution1(string[] input) =>
		input
		.Select(i => i.ToDirections())
		.GetCode(KEYPAD1, START_POS1, MIN1, MAX1);

	private static string Solution2(string[] input) =>
		input
		.Select(i => i.ToDirections())
		.GetCode(KEYPAD2, START_POS2, MIN2, MAX2);
}

file static class Day02Extensions
{
	public static IEnumerable<Direction> ToDirections(this string input) => input.ToCharArray().Select(c => Enum.Parse<Direction>($"{c}"));

	public static string GetCode(this IEnumerable<IEnumerable<Direction>> instructions, string keypad, Point start, int[] clampMin, int[] clampMax)
	{
		Point current = start;
		char[,] keypadArray = keypad.GetKeypad();
		string code = "";
		foreach (IEnumerable<Direction> directions in instructions) {
			foreach (Direction direction in directions) {
				current = direction switch
				{
					Direction.L => current with { X = Math.Clamp(current.X - 1, clampMin[current.Y], clampMax[current.Y]) },
					Direction.R => current with { X = Math.Clamp(current.X + 1, clampMin[current.Y], clampMax[current.Y]) },
					Direction.U => current with { Y = Math.Clamp(current.Y - 1, clampMin[current.X], clampMax[current.X]) },
					Direction.D => current with { Y = Math.Clamp(current.Y + 1, clampMin[current.X], clampMax[current.X]) },
					_ => throw new IndexOutOfRangeException(),
				};
			}

			code = $"{code}{keypadArray[current.X, current.Y]}";
		}

		return code;
	}
	public static char[,] GetKeypad(this string keypad) => keypad.ReplaceLineEndings().Split(Environment.NewLine).To2dArray();
}

file class Day02Types
{
	public enum Direction
	{
		U,
		D,
		L,
		R,
	}
}

file static class Day02Constants
{

	public const string KEYPAD1 = """
		123
		456
		789
		""";
	public readonly static Point START_POS1 = new(1, 1);
	public readonly static int[] MIN1 = [0, 0, 0];
	public readonly static int[] MAX1 = [2, 2, 2];

	public const string KEYPAD2 = """
		..1..
		.234.
		56789
		.ABC.
		..D..
		""";
	public readonly static Point START_POS2 = new(0, 2);
	public readonly static int[] MIN2 = [2, 1, 0, 1, 2];
	public readonly static int[] MAX2 = [2, 3, 4, 3, 2];
}
