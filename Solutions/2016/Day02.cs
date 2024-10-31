namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 02: Bathroom Security
/// https://adventofcode.com/2016/day/02
/// </summary>
[Description("Bathroom Security")]
public sealed partial class Day02 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static readonly Point _start = new(1, 1);
	private static readonly char[,] _keypad = """
		123
		456
		789
		""".ReplaceLineEndings().Split(Environment.NewLine).To2dArray();

	private static string Solution1(string[] input)
	{
		Point current = _start;
		string code = "";
		List<List<Direction>> instructions =
			input
			.Select(i => i.ToCharArray().Select(c => Enum.Parse<Direction>(c.ToString())).ToList())
			.ToList();

		foreach (List<Direction> directions in instructions) {
			foreach (Direction direction in directions) {
				current = direction switch
				{
					Direction.L => current with { X = Math.Clamp(current.X - 1, 0, 2) },
					Direction.R => current with { X = Math.Clamp(current.X + 1, 0, 2) },
					Direction.U => current with { Y = Math.Clamp(current.Y - 1, 0, 2) },
					Direction.D => current with { Y = Math.Clamp(current.Y + 1, 0, 2) },
					_ => throw new IndexOutOfRangeException(),
				};
			}
			code = $"{code}{_keypad[current.X, current.Y]}";
		}

		return code;
	}

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}

	private enum Direction
	{
		Up,
		Down,
		Left,
		Right,

		U = Up,
		D = Down,
		L = Left,
		R = Right,
	}

}
