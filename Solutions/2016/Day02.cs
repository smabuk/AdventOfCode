namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 02: Bathroom Security
/// https://adventofcode.com/2016/day/02
/// </summary>
[Description("Bathroom Security")]
public sealed partial class Day02 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static readonly char[,] _keypad1 = """
		123
		456
		789
		""".ReplaceLineEndings().Split(Environment.NewLine).To2dArray();

	private static readonly char[,] _keypad2 = """
		  1  
		 234 
		56789
		 ABC 
		  D  
		""".ReplaceLineEndings().Split(Environment.NewLine).To2dArray();

	private static string Solution1(string[] input)
	{
		Point current = new(1, 1);
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
			code = $"{code}{_keypad1[current.X, current.Y]}";
		}

		return code;
	}

	private static string Solution2(string[] input) {

		int[] clampMin = [2, 1, 0, 1, 2];
		int[] clampMax = [2, 3, 4, 3, 2];

		Point current = new(0, 2);
		string code = "";
		List<List<Direction>> instructions =
			input
			.Select(i => i.ToCharArray().Select(c => Enum.Parse<Direction>(c.ToString())).ToList())
			.ToList();

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
			code = $"{code}{_keypad2[current.X, current.Y]}";
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
