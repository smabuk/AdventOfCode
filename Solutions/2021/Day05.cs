using System.ComponentModel;

namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 05: Hydrothermal Venture
/// https://adventofcode.com/2021/day/5
/// </summary>
[Description("Hydrothermal Venture")]
public class Day05 {

	record Line(Point Start, Point End);
	record Point(int X, int Y);

	private static int Solution1(string[] input) {
		IEnumerable<Line> lines = input.Select(i => ParseLine(i))
			.Where(l => l.Start.X == l.End.X || l.Start.Y == l.End.Y);

		IEnumerable<Point> points = GetPointsFromLine(lines);

		return points
			.GroupBy(p => p)
			.Select(g => new { Count = g.Count() })
			.Count(x => x.Count > 1);
	}

	private static int Solution2(string[] input) {
		IEnumerable<Line> lines = input.Select(i => ParseLine(i)).ToList();

		IEnumerable<Point> points = GetPointsFromLine(lines);

		return points
			.GroupBy(p => p)
			.Select(g => new { Count = g.Count() })
			.Count(x => x.Count > 1);
	}


	private static IEnumerable<Point> GetPointsFromLine(IEnumerable<Line> lines) {
		foreach ((Point start, Point end) in lines) {
			int dX = Math.Sign(end.X - start.X);
			int dY = Math.Sign(end.Y - start.Y);
			for (Point p = start; p != end; p = p with { X = p.X + dX, Y = p.Y + dY }) {
				yield return p;
			}
			yield return end;
		}
	}

	private static Line ParseLine(string input) {
		Match match = Regex.Match(input, @"(\d+),(\d+) -> (\d+),(\d+)");
		if (match.Success) {
			return new(new(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)), new(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value)));
		}
		throw new ArgumentException($"Invalid input line: {input}", nameof(input));
	}




	#region Problem initialisation
	public static string Part1(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution1(input).ToString();
	}
	public static string Part2(string[]? input, params object[]? args) {
		if (input is null) { return "Error: No data provided"; }
		input = input.StripTrailingBlankLineOrDefault();
		return Solution2(input).ToString();
	}
	#endregion

}
