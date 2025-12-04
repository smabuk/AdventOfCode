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
		VisualiseGrid(_diagram, "Initial");
	}

	private static char[,] _diagram = default!;

	const char PAPER_ROLL = '@';

	public static int Part1()
		=> _diagram
		.ForEachCell()
		.Where(position =>
			position.Value is PAPER_ROLL &&
			position.Index.AllAdjacent().Count(cell =>
				_diagram.IsInBounds(cell) && _diagram[cell.X, cell.Y] is PAPER_ROLL) < 4)
		.Count();

	public static string Part2() => NO_SOLUTION_WRITTEN_MESSAGE;

}
