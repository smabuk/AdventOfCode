
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
		_diagram = input.To2dArray();
		VisualiseGrid(_diagram, "Initial state:");
	}

	private static char[,] _diagram = default!;

	const char PAPER_ROLL = '@';
	const char SPACE      = '.';
	const char REMOVE     = 'x';

	public static int Part1()
		=> _diagram
			.ForEachCell()
			.Where(position => position.Value is PAPER_ROLL &&
					_diagram
						.GetAdjacentCells(position, includeDiagonals: true)
						.Count(cell => cell.Value is PAPER_ROLL) < 4)
			.Count();

	public static int Part2()
	{
		char[,] diagram = (char[,])_diagram.Clone();

		int count = 0;
		int newCount = 0;

		do {
			char[,] newDiagram = (char[,])diagram.Clone();
			newCount = 0;

			IEnumerable<Cell<char>> accesible = diagram
				.ForEachCell()
				.Where(position => position.Value is PAPER_ROLL &&
					diagram
						.GetAdjacentCells(position, includeDiagonals: true)
						.Count(cell => cell.Value is PAPER_ROLL) < 4);

			foreach (Cell<char> roll in accesible) {
				newDiagram[roll.X, roll.Y] = REMOVE;
				count++;
				newCount++;
			}

			VisualiseGrid(newDiagram, $"Remove {newCount} rolls of paper:");

			foreach (Cell<char> cell in newDiagram.ForEachCell().Where(cell => cell.Value is REMOVE)) {
				newDiagram[cell.X, cell.Y] = SPACE;
			}

			diagram = (char[,])newDiagram.Clone();
		} while (newCount > 0);

		return count;
	}
}
