namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 05: Hydrothermal Venture
/// https://adventofcode.com/2021/day/5
/// </summary>
public class Day05 {

	record Line(Point Start, Point End);
	record Point(int X, int Y);

	private static int Solution1(string[] input) {
		List<Line> lines = input.Select(i => ParseLine(i)).ToList();
		List<Point> points = new();

		foreach (Line line in lines) {
			Point start, end;
			if (line.Start.X == line.End.X) {
				if (line.Start.Y <= line.End.Y) {
					start = line.Start;
					end = line.End;
				} else {
					start = line.End;
					end = line.Start;
				}
				for (int y = start.Y; y <= end.Y; y++) {
					points.Add(new(start.X, y));
				}
			} else if (line.Start.Y == line.End.Y) {
				if (line.Start.X <= line.End.X) {
					start = line.Start;
					end = line.End;
				} else {
					start = line.End;
					end = line.Start;
				}
				for (int x = start.X; x <= end.X; x++) {
					points.Add(new(x, start.Y));
				}
			}
		}

		return points.GroupBy(p => p).Select(g => new { Count = g.Count() }).Count(x => x.Count > 1);
	}

	private static int Solution2(string[] input) {
		List<Line> lines = input.Select(i => ParseLine(i)).ToList();
		List<Point> points = new();

		foreach (Line line in lines) {
			Point start, end;
			int direction;
			if (line.Start.X == line.End.X) {
				if (line.Start.Y <= line.End.Y) {
					start = line.Start;
					end = line.End;
				} else {
					start = line.End;
					end = line.Start;
				}
				for (int y = start.Y; y <= end.Y; y++) {
					points.Add(new(start.X, y));
				}
			} else if (line.Start.Y == line.End.Y) {
				if (line.Start.X <= line.End.X) {
					start = line.Start;
					end = line.End;
				} else {
					start = line.End;
					end = line.Start;
				}
				for (int x = start.X; x <= end.X; x++) {
					points.Add(new(x, start.Y));
				}
			} else {
				if (line.Start.X <= line.End.X) {
					start = line.Start;
					end = line.End;
				} else {
					start = line.End;
					end = line.Start;
				}
				if (start.Y <= end.Y) {
					direction = 1;
				} else {
					direction = -1;
				}
				for ((int x, int y) = start; x <= end.X; x++, y += direction) {
					points.Add(new(x, y));
				}
			}
		}

		return points.GroupBy(p => p).Select(g => new { Count = g.Count() }).Count(x => x.Count > 1);
	}

	private static Line ParseLine(string input) {
		Match match = Regex.Match(input, @"(\d+),(\d+) -> (\d+),(\d+)");
		if (match.Success) {
			return new(new(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)), new(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value)));
		}
		return null!;
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
