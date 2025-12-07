namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 07: Laboratories
/// https://adventofcode.com/2025/day/07
/// </summary>
[Description("Laboratories")]
[GenerateVisualiser]
public partial class Day07 {

	private const char BEAM = '|';
	private const char SPACE = '.';
	private const char SPLITTER = '^';
	private const char START = 'S';
	private static readonly Point SPLIT_LEFT = new(-1, 1);
	private static readonly Point SPLIT_RIGHT = new(1, 1);
	private static readonly Point NEXT = new(0, 1);
	private static readonly Point RIGHT = new(1, 0);

	public static int Part1(string[] input)
	{
		Grid<char> diagram = input.To2dGrid();

		int tachyonSplits = 0;

		Point tachyonStart = new(diagram.Row(0).Index().First(x => x.Item == START).Index, 1);
		diagram[tachyonStart] = BEAM;

		VisualiseGrid(diagram, "Initial:");

		for (int rowIdx = 2; rowIdx < diagram.RowsCount; rowIdx++) {
			for (int colIdx = 0; colIdx < diagram.ColsCount; colIdx++) {
				if (diagram[colIdx, rowIdx] is SPLITTER && diagram[colIdx, rowIdx - 1] is BEAM) {
					diagram[colIdx - 1, rowIdx] = BEAM;
					diagram[colIdx + 1, rowIdx] = BEAM;
					tachyonSplits++;
				}

				if (diagram[colIdx, rowIdx] is SPACE && diagram[colIdx, rowIdx - 1] is BEAM) {
					diagram[colIdx, rowIdx] = BEAM;
				}
			}

			VisualiseGrid(diagram, "");
		}

		return tachyonSplits;
	}

	public static long Part2(string[] input)
	{

		Grid<char> diagram = input.To2dGrid();
		Grid<long> timelineCounts = new(diagram.ColsCount, diagram.RowsCount);

		Point tachyonStart = new(diagram.Row(0).Index().First(x => x.Item == START).Index, 1);
		timelineCounts[tachyonStart] = 1;

		for (int rowIdx = 1; rowIdx < diagram.RowsCount - 1; rowIdx++) {
			for (Point current = new(0, rowIdx); current.Y < diagram.ColsCount; current += RIGHT) {

				long currentTimelines = timelineCounts[current];
				if (currentTimelines == 0) {
					continue;
				}

				char nextCell = diagram[current + NEXT];

				if (nextCell is SPLITTER) {
					if (current.X > 0) {
						timelineCounts[current + SPLIT_LEFT] += currentTimelines;
					}

					if (current.X < diagram.ColsCount - 1) {
						timelineCounts[current + SPLIT_RIGHT] += currentTimelines;
					}
				} else if (nextCell is SPACE) {
					timelineCounts[current + NEXT] += currentTimelines;
				}
			}
		}

		long timelines = timelineCounts.Row(diagram.RowsCount - 1).Sum();

		return timelines;
	}
}
