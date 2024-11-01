namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 02: Bathroom Security
/// https://adventofcode.com/2016/day/02
/// </summary>
[Description("Bathroom Security")]
public sealed partial class Day02 {

	public static string Part1(string[] input) => Solution1(input).ToString();
	public static string Part2(string[] input) => Solution2(input).ToString();

	private static string Solution1(string[] input)
	{
		char[,] keypad = """
			123
			456
			789
			"""
			.ReplaceLineEndings().Split(Environment.NewLine).To2dArray();

		List<List<Direction>> instructions =
			input
			.Select(i => i.ToCharArray().Select(c => Enum.Parse<Direction>(c.ToString())).ToList())
			.ToList();

		return GetCode(keypad, instructions, new(1, 1), [0, 0, 0], [2, 2, 2]);
	}

	private static string Solution2(string[] input)
	{
		char[,] keypad = """
			..1..
			.234.
			56789
			.ABC.
			..D..
			"""
			.ReplaceLineEndings().Split(Environment.NewLine).To2dArray();

		List<List<Direction>> instructions =
			input
			.Select(i => i.ToCharArray().Select(c => Enum.Parse<Direction>(c.ToString())).ToList())
			.ToList();

		return GetCode(keypad, instructions, new(0, 2), [2, 1, 0, 1, 2], [2, 3, 4, 3, 2]);
	}

	private static string GetCode(char[,] keypad, List<List<Direction>> instructions, Point start, int[] clampMin, int[] clampMax)
	{
		Point current = start;
		string code = "";
		foreach (List<Direction> directions in instructions) {
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
			code = $"{code}{keypad[current.X, current.Y]}";
		}
		return code;
	}

	private enum Direction
	{
		U,
		D,
		L,
		R,
	}

}
