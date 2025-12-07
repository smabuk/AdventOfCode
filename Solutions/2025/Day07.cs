namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 07: Laboratories
/// https://adventofcode.com/2025/day/07
/// </summary>
[Description("Laboratories")]
[GenerateVisualiser]
public partial class Day07 {

	private const char BEAM     = '|';
	private const char SPACE    = '.';
	private const char SPLITTER = '^';
	private const char START    = 'S';

	public static int Part1(string[] input)
	{
		Grid<char> diagram = input.To2dGrid();

		int tachyonSplits = 0;

		Point tachyonStart = new(diagram.Find(START) ?? throw new ApplicationException("Start not found."));
		diagram[tachyonStart.Down] = BEAM;

		VisualiseGrid(diagram, "Initial:");

		VisualiseStrings(["Final:", diagram.RowAsString(0), diagram.RowAsString(1)]);

		for (int rowIdx = 2; rowIdx < diagram.RowsCount; rowIdx++) {
			for (int colIdx = 0; colIdx < diagram.ColsCount; colIdx++) {
				Point cell = new(colIdx, rowIdx);
				if (diagram[cell] is SPLITTER && diagram[cell.Up] is BEAM) {
					diagram[cell.Left] = BEAM;
					diagram[cell.Right] = BEAM;
					tachyonSplits++;
				}

				if (diagram[cell] is SPACE && diagram[cell.Up] is BEAM) {
					diagram[cell] = BEAM;
				}
			}

			VisualiseString(diagram.RowAsString(rowIdx));
		}

		VisualiseGridWithMarkup(diagram, "Final:", false, ($"{SPLITTER}", "[lime]"), ($"{BEAM}", "[red]"));
		return tachyonSplits;
	}

	public static long Part2(string[] input)
	{
		Grid<char> diagram = input.To2dGrid();
		Grid<long> timelineCounts = new(diagram.ColsCount, diagram.RowsCount);

		Point tachyonStart = new(diagram.Find(START) ?? throw new ApplicationException("Start not found."));
		timelineCounts[tachyonStart.Down] = 1;

		for (int rowIdx = 1; rowIdx < diagram.RowsCount - 1; rowIdx++) {
			for (int colIdx = 0; colIdx < diagram.ColsCount; colIdx++) {
				Point cell = new(colIdx, rowIdx);
				long currentTimelines = timelineCounts[cell];
				if (currentTimelines == 0) {
					continue;
				}

				Point nextCell = cell.Down;

				switch (diagram[nextCell]) {
					case SPLITTER:
						timelineCounts[nextCell.Left] += currentTimelines;
						timelineCounts[nextCell.Right] += currentTimelines;
						break;
					case SPACE:
						timelineCounts[nextCell] += currentTimelines;
						break;
					default:
						break;
				}
			}
		}

		return timelineCounts.BottomEdge().Sum();
	}
}
