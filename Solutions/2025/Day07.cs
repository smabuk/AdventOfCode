namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 07: Laboratories
/// https://adventofcode.com/2025/day/07
/// </summary>
[Description("Laboratories")]
[GenerateVisualiser]
public partial class Day07
{

	private const char BEAM = '|';
	private const char SPACE = '.';
	private const char SPLITTER = '^';
	private const char START = 'S';

	public static int Part1(string[] input)
	{
		// Can ignore all of the input lines that don't contain a splitter or the start as they are irrelevant.
		Grid<char> diagram = input.Where(line => line.Contains(SPLITTER) || line.Contains(START)).To2dGrid();

		int tachyonSplits = 0;

		VisualiseGridWithMarkup(diagram, "Initial:", true);

		//VisualiseStrings(["Final:", diagram.RowAsString(0)]);
		//VisualiseStringWithMarkup(diagram.RowAsString(0).Replace(START, BEAM), ($"{START}", $"{BEAM}"), ($"{BEAM}", "[red]"));

		for (int rowIdx = 1; rowIdx < diagram.RowsCount; rowIdx++) {
			for (int colIdx = 0; colIdx < diagram.ColsCount; colIdx++) {
				Point cell = new(colIdx, rowIdx);
				if (diagram[cell] is SPLITTER && diagram[cell.Up] is BEAM or START) {
					diagram[cell.Left] = BEAM;
					diagram[cell.Right] = BEAM;
					tachyonSplits++;
				}

				if (diagram[cell] is SPACE && diagram[cell.Up] is BEAM or START) {
					diagram[cell] = BEAM;
				}
			}

			VisualiseGridWithMarkup(diagram, "Final:", true, ($"{SPLITTER}", "[lime]"), ($"{BEAM}", "[red]"));
			//VisualiseStringWithMarkup(diagram.RowAsString(rowIdx), ($"{SPLITTER}", "[lime]"), ($"{BEAM}", "[red]"));
			//VisualiseStringWithMarkup(diagram.RowAsString(rowIdx).Replace(SPLITTER, SPACE), ($"{BEAM}", "[red]"));
		}

		return tachyonSplits;
	}

	public static long Part2(string[] input)
	{
		Grid<char> diagram = input.Where(line => line.Contains(SPACE) || line.Contains(START)).To2dGrid();
		Grid<long> timelineCounts = new(diagram.ColsCount, diagram.RowsCount);

		Point tachyonStart = new(diagram.Find(START) ?? throw new ApplicationException("Start not found."));
		timelineCounts[tachyonStart] = 1;

		for (int rowIdx = 0; rowIdx < diagram.RowsCount - 1; rowIdx++) {
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
