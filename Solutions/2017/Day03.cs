using static AdventOfCode.Solutions._2017.Day03Constants;
using static AdventOfCode.Solutions._2017.Day03Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 03: Spiral Memory
/// https://adventofcode.com/2016/day/03
/// </summary>
[Description("Spiral Memory")]
public sealed partial class Day03 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		int target = input[0].As<int>();
		int rows = ((int)Math.Sqrt(target - 1)) + (int.IsEvenInteger((int)Math.Sqrt(target - 1)) ? 1 : 2);
		int bottomRight = rows * rows;
		int spiralLength = bottomRight - ((rows - 2) * (rows - 2));
		int sideLengthMinus1 = (spiralLength / 4);
		int offset = int.Abs((sideLengthMinus1 / 2) - ((bottomRight - target) % sideLengthMinus1)) ;
		return (rows / 2) + offset;
	}

	private static int Solution2(string[] input) {
		int target = input[0].As<int>();
		int[,] grid = new int[21, 21];
		Point current = new(10, 10);
		grid[current.X, current.Y] = 1;

		for (int level = 2; ; level += 2) {
			// Step right, move up 1 level, left 1 level, down 1 level, right 1 level
			List<Point> points = [
				current += (1, 0),
				..Enumerable.Range(1, level - 1).Select(i => new Point(current.X, current.Y - i)),
				..Enumerable.Range(1, level).Select(i => new Point(current.X - i, current.Y - level + 1)),
				..Enumerable.Range(1, level).Select(i => new Point(current.X - level, current.Y - level + i + 1)),
				..Enumerable.Range(1, level).Select(i => new Point(current.X - level + i, current.Y + 1)),
			];
			foreach (var nextCell in points) {
				current = nextCell;
				int newValue = grid.GetAdjacentCells(current, includeDiagonals: true).Sum(cell => cell.Value);
				if (newValue > target) {
					return newValue;
				}
				grid[current.X, current.Y] = newValue;
			}
		}
	}
}

file static class Day03Extensions
{
}

internal sealed partial class Day03Types
{
}

file static class Day03Constants
{
}
