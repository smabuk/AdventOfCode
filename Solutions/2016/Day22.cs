using static AdventOfCode.Solutions._2016.Day22Constants;
using static AdventOfCode.Solutions._2016.Day22Types;
namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 22: Grid Computing
/// https://adventofcode.com/2016/day/22
/// </summary>
[Description("Grid Computing")]
public sealed partial class Day22 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, Action<string[], bool>? visualise = null, params object[]? args) => Solution2(input, visualise).ToString();
	private static int Solution1(string[] input) {
		return input
			.Skip(2)
			.As<Node>()
			.Permute(2)
			.Count(pair => pair.IsViablePair());
	}

	private static string Solution2(string[] input, Action<string[], bool>? visualise = null) {
		List<Node> nodes = [..input
			.Skip(2)
			.As<Node>()];

		int colsX = nodes.Max(n => n.Position.X) + 1;
		int colsY = nodes.Max(n => n.Position.Y) + 1;
		Point goal = new(colsX - 1, 0);
		Node[,] grid = new Node[colsX, colsY];
		foreach (Node node in nodes) {
			grid[node.Position.X, node.Position.Y] = node;
		}

		grid.VisualiseGrid(goal, "Initial", visualise);

		// Solve

		grid.VisualiseGrid(goal, "Final", visualise);
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class Day22Extensions
{
	public static bool IsViablePair(this Node[] nodes)
	{
		const int A = 0;
		const int B = 1;

		return nodes[A].Used != 0
			&& nodes[A].Name != nodes[B].Name
			&& nodes[A].Used <= nodes[B].Available;
	}

	[HasVisualiser]
	public static void VisualiseGrid(this Node[,] grid, Point goal, string title, Action<string[], bool>? visualise)
	{
		const char GOAL = 'G';
		const char EMPTY_NODE = '_';
		const char SMALL_ENOUGH_NODE = '.';
		const char VERY_FULL_NODE = '#';

		if (visualise is not null) {
			string[] gridPlan = [];

			int veryLargeVeryFull = grid.WalkWithValues().Max(cell => cell.Value.Size);
			for (int y = 0; y < grid.RowsCount(); y++) {
				string row = "";
		
				for (int x = 0; x < grid.ColsCount(); x++) {
					Node node = grid[x, y];
					char c = SMALL_ENOUGH_NODE;
					if (node.Position == goal) {
						c = GOAL;
					} else if (node.Used == 0) {
						c = EMPTY_NODE;
					} else if (node.Size == veryLargeVeryFull) {
						c = VERY_FULL_NODE;
					}
					row = node.Position == Point.Zero ? $"{row}({c})" : $"{row} {c} ";
				}

				gridPlan = [.. gridPlan.Append(row)];
			}

			string[] output = ["", title, .. gridPlan];
			_ = Task.Run(() => visualise?.Invoke(output, false));
		}
	}

}

internal sealed partial class Day22Types
{

	public sealed record Node(string Name, Point Position, int Size, int Used, int Available, int UsePercentage) : IParsable<Node>
	{
		public static Node Parse(string s, IFormatProvider? provider)
		{
			if (s[0] == '/') {
				string[] tokens = s.TrimmedSplit([' ','T','%', '/']);
				string name = tokens[2];
				string[] nameTokens = name.TrimmedSplit(['-','x','y']);
				Point position = new(nameTokens[1].As<int>(), nameTokens[2].As<int>());
				return new(name,
					position,
					tokens[3].As<int>(),
					tokens[4].As<int>(),
					tokens[5].As<int>(),
					tokens[6].As<int>());
			}
			return null!;
		}

		public static Node Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Node result)
			=> ISimpleParsable<Node>.TryParse(s, provider, out result);
	}
}

file static class Day22Constants
{
}
