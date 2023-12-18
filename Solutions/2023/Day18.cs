using static AdventOfCode.Solutions._2023.Day18;
namespace AdventOfCode.Solutions._2023;

/// <summary>
/// Day 18: Lavaduct Lagoon
/// https://adventofcode.com/2023/day/18
/// </summary>
[Description("Lavaduct Lagoon")]
public sealed partial class Day18 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadInstructions(input);
	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	public const char EMPTY  = '.';
	public const char TRENCH = '#';


	private static IEnumerable<Instruction> _digPlan = [];

	private static void LoadInstructions(string[] input) {
		_digPlan = input.As<Instruction>();
	}

	private static int Solution1(string[] input) {
		List<Instruction> digPlan = [.. input.As<Instruction>()];
		List<Hole> trench = [];
		Point position = new(0, 0);
		foreach (Instruction instruction in digPlan) {
			List<Hole> newHoles = [.. Dig(position, instruction)];
			trench.AddRange(newHoles);
			position = newHoles[^1].Position;
		}

		List<Point> trenchRoute = [..trench.Select(t => t.Position)];
		char[,] lagoon = trenchRoute.To2dArray(EMPTY, TRENCH);

		Point start = new(lagoon.RowAsString(1).IndexOf(TRENCH) + 1, 1);
		lagoon.FloodFill(start, [EMPTY], '@');
		int cubicMetersOfLava = trenchRoute.Count + lagoon.Walk2dArrayWithValues().Count(hole => hole == '@');

		return cubicMetersOfLava;
	}

	private static string Solution2(string[] input) {
		List<Instruction> instructions = [.. input.As<Instruction>()];
		return "** Solution not written yet **";
	}

	private static IEnumerable<Hole> Dig(Point position, Instruction instruction)
	{
		for (int i = 1; i <= instruction.Value; i++) {
			(int dX, int dY) dig = (instruction.DigDirection.dX * i, instruction.DigDirection.dY * i);
			yield return new(new(position + dig), instruction.RgbValue);
		}
	}

	public record struct Hole(Point Position, string RgbValue);

	private sealed record Instruction(string Direction, int Value, string RgbValue) : IParsable<Instruction> {

		public (int dX, int dY) DigDirection = Direction switch
		{
			"U" => ArrayHelpers.UP,
			"D" => ArrayHelpers.DOWN,
			"L" => ArrayHelpers.LEFT,
			"R" => ArrayHelpers.RIGHT,
			_ => throw new NotImplementedException(),
		};
		public static Instruction Parse(string s, IFormatProvider? provider)
		{
			string[] splitBy = [" ", "(", ")"];
			string[] tokens = s.TrimmedSplit(splitBy);
			return new(tokens[0], tokens[1].As<int>(), tokens[2]);
		}
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Instruction result)
			=> ISimpleParsable<Instruction>.TryParse(s, provider, out result);
	}
}

file static class Day18Helpers {
	public static void FloodFill(this char[,] grid, Point start, char[] cellTypesToFill, char fillValue)
	{
		Queue<Point> queue = [];
		queue.Enqueue(start);
		while (queue.Count != 0) {
			Point point = queue.Dequeue();
			if (!cellTypesToFill.Contains(grid[point.X, point.Y])) {
				continue;
			}
			grid[point.X, point.Y] = fillValue;
			foreach (Cell<char> adjacent in grid.GetAdjacentCells(point)) {
				queue.Enqueue(adjacent.Index);
			}
		}
	}
}

