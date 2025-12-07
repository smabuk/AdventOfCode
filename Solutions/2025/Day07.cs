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
	private static readonly Point NEXT_ROW    = new( 0,  1);
	private static readonly Point PREV_ROW    = new( 0, -1);
	private static readonly Point SPLIT_LEFT  = new(-1,  0);
	private static readonly Point SPLIT_RIGHT = new( 1,  0);
	private static readonly Point[] SPLIT_LEFT_AND_RIGHT = [SPLIT_LEFT, SPLIT_RIGHT];

	public static int Part1(string[] input)
	{
		Grid<char> diagram = input.To2dGrid();

		int tachyonSplits = 0;

		Point tachyonStart = new(diagram.Find(START) ?? throw new ApplicationException("Start not found."));
		diagram[tachyonStart + NEXT_ROW] = BEAM;

		VisualiseGrid(diagram, "Initial:");

		for (int rowIdx = 2; rowIdx < diagram.RowsCount; rowIdx++) {
			for (int colIdx = 0; colIdx < diagram.ColsCount; colIdx++) {
				Point cell = new(colIdx, rowIdx);
				if (diagram[cell] is SPLITTER && diagram[cell + PREV_ROW] is BEAM) {
					diagram[cell + SPLIT_LEFT] = BEAM;
					diagram[cell + SPLIT_RIGHT] = BEAM;
					tachyonSplits++;
				}

				if (diagram[cell] is SPACE && diagram[cell + PREV_ROW] is BEAM) {
					diagram[cell] = BEAM;
				}
			}

			VisualiseGridWithMarkup(diagram, "", true, ($"{SPLITTER}", "[lime]"), ($"{BEAM}", "[red]"));
		}

		return tachyonSplits;
	}

	public static long Part2(string[] input)
	{
		Grid<char> diagram = input.To2dGrid();
		Grid<long> timelineCounts = new(diagram.ColsCount, diagram.RowsCount);

		Point tachyonStart = new(diagram.Find(START) ?? throw new ApplicationException("Start not found."));
		timelineCounts[tachyonStart + NEXT_ROW] = 1;

		for (int rowIdx = 1; rowIdx < diagram.RowsCount - 1; rowIdx++) {
			for (int colIdx = 0; colIdx < diagram.ColsCount; colIdx++) {
				Point cell = new(colIdx, rowIdx);
				long currentTimelines = timelineCounts[cell];
				if (currentTimelines == 0) {
					continue;
				}

				Point nextCell = cell + NEXT_ROW;

				switch (diagram[nextCell]) {
					case SPLITTER:
						foreach (Point split in SPLIT_LEFT_AND_RIGHT) {
							if (timelineCounts.IsInBounds(nextCell + split)) {
								timelineCounts[nextCell + split] += currentTimelines;
							}
						}
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
