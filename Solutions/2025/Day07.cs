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

	public static string Part2() => NO_SOLUTION_WRITTEN_MESSAGE;
}
