using static AdventOfCode.Solutions._2017.Day03Constants;
using static AdventOfCode.Solutions._2017.Day03Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 03: Spiral Memory
/// https://adventofcode.com/2017/day/03
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

		int[,] grid = new int[GRID_SIZE, GRID_SIZE];
		Point start = new(GRID_SIZE / 2, GRID_SIZE / 2);
		grid[start.X, start.Y] = 1;

		foreach (Point point in start.SpiralPoints()) {
			grid[point.X, point.Y] = grid
				.GetAdjacentCells(point, includeDiagonals: true)
				.Sum(cell => cell.Value);

			if (grid[point.X, point.Y] > target) {
				return grid[point.X, point.Y];
			}
		}

		throw new ApplicationException("Should never reach here!");
	}
}

file static class Day03Extensions
{
	public static IEnumerable<Point> SpiralPoints(this Point start)
	{
		Point current = start;

		for (int level = 2; ; level += 2) {
			// Move right
			current += (1, 0);
			yield return current;

			// Move up 1 level, left 1 level, down 1 level, right 1 level
			List<Point> points = [
				..Enumerable.Range(1, level - 1).Select(i => new Point(current.X, current.Y - i)),
				..Enumerable.Range(1, level).Select(i => new Point(current.X - i, current.Y - level + 1)),
				..Enumerable.Range(1, level).Select(i => new Point(current.X - level, current.Y - level + i + 1)),
				..Enumerable.Range(1, level).Select(i => new Point(current.X - level + i, current.Y + 1)),
			];

			foreach (var point in points) {
				yield return point;
			}

			current = points[^1];
		}
	}
}

internal sealed partial class Day03Types
{
}

file static class Day03Constants
{
	public const int GRID_SIZE = 21;
}
