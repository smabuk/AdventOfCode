namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 04: Printing Department
/// https://adventofcode.com/2025/day/04
/// </summary>
[Description("Printing Department")]
[GenerateVisualiser]
public partial class Day04 {

	[Init]
	public static void LoadDiagram(string[] input)
	{
		_diagram = input.To2dGrid<char>();
		VisualiseGrid(_diagram, "Initial state:");
	}

	private static Grid<char> _diagram = default!;

	const char PAPER_ROLL = '@';
	const char SPACE      = '.';
	const char REMOVE     = 'x';

	public static int Part1()
		=> _diagram
			.ForEachCell()
			.Where(position => position.Value is PAPER_ROLL &&
					_diagram
						.GetAdjacentCells(position, includeDiagonals: true)
						.Count(adjacent => adjacent.Value is PAPER_ROLL) < 4)
			.Count();

	public static int Part2()
	{
		Grid<char> diagram = _diagram.Copy();
		Grid<char> newDiagram;

		int count = 0;
		int newCount = 0;

		do {
			newDiagram = diagram.Copy();
			newCount = 0;

			IEnumerable<Cell<char>> accesible = diagram
				.ForEachCell()
				.Where(position => position.Value is PAPER_ROLL &&
					diagram
						.GetAdjacentCells(position, includeDiagonals: true)
						.Count(adjacent => adjacent.Value is PAPER_ROLL) < 4);

			foreach (Cell<char> roll in accesible) {
				newDiagram[roll.Index] = REMOVE;
				count++;
				newCount++;
			}

			VisualiseGrid(newDiagram, $"Remove {newCount} rolls of paper:", clearScreen: true);

			diagram = newDiagram.Replace(REMOVE, SPACE);
		} while (newCount > 0);

		return count;
	}
}
