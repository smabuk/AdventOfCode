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
	private static readonly Point SPLIT_LEFT  = new(-1, 0);
	private static readonly Point SPLIT_RIGHT = new( 1, 0);
	private static readonly Point NEXT_ROW    = new( 0, 1);
	private static readonly Point[] SPLIT_LEFT_AND_RIGHT = [SPLIT_LEFT, SPLIT_RIGHT];

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

		foreach (Cell<char> cell in diagram.ForEachCell().Where(cell => cell.Row > 0 && cell.Row < diagram.ColsCount)) {
			long currentTimelines = timelineCounts[cell.Index];
			if (currentTimelines == 0) {
				continue;
			}

			Point nextCell = cell.Index + NEXT_ROW;

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

		return timelineCounts.Row(diagram.RowsCount - 1).Sum();
	}
}
