namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 18: Lavaduct Lagoon
/// https://adventofcode.com/2023/day/18
/// </summary>
[Description("Lavaduct Lagoon")]
public sealed partial class Day18 {

	public static string Part1(string[] input, params object[]? args) => Solution(input, Instruction.Parse).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution(input, Instruction.SwappedParse).ToString();

	private static long Solution(string[] input, Func<string, Instruction> parse) {
		List<Instruction> digPlan = [.. input.Select(parse)];

		Point current = new(0, 0);
		List<Point> trench = [current];

		int distance = 0;
		foreach (Instruction instruction in digPlan) {
			(int dX, int dY) dig = (instruction.DigDirection.dX * instruction.Value, instruction.DigDirection.dY * instruction.Value);
			current += dig;
			trench.Add(current);
			distance += int.Abs(dig.dX + dig.dY);
		}

		// Twice the are of an irregular polygon
		long area = trench
			.Take(trench.Count - 1)
			.Select((p, i) => (long)(trench[i + 1].X - p.X) * (long)(trench[i + 1].Y + p.Y))
			.Sum() / 2;

		// Allow for the width of the lines as well
		long cubicMetersOfLava = Math.Abs(area) + (distance / 2) + 1;

		return cubicMetersOfLava;
	}

	private sealed record Instruction(string Direction, int Value) {

		public (int dX, int dY) DigDirection = Direction switch
		{
			"U" => ArrayHelpers.UP,
			"D" => ArrayHelpers.DOWN,
			"L" => ArrayHelpers.LEFT,
			"R" => ArrayHelpers.RIGHT,
			_ => throw new NotImplementedException(),
		};

		public static Instruction Parse(string s)
		{
			string[] splitBy = [" ", "(", ")"];
			string[] tokens = s.TrimmedSplit(splitBy);
			return new(tokens[0], tokens[1].As<int>());
		}

		public static Instruction SwappedParse(string s)
		{
			string hexValues = s.TrimmedSplit([" ", "(", ")"])[2];
			string direction = hexValues[^1] switch
			{
				'0' => "R",
				'1' => "D",
				'2' => "L",
				'3' => "U",
				_ => throw new NotImplementedException(),
			};

			return new(direction, int.Parse(hexValues[1..6], System.Globalization.NumberStyles.HexNumber));
		}
	}
}

