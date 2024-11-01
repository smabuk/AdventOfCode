internal partial class Program
{
	static void ShowHelp()
	{
		Console.WriteLine($"Calculates and displays solutions to the Advent of Code problems");
		Console.WriteLine();
		Console.WriteLine($"[YYYY [dd]] [/visual] [arg1 arg2 arg3 ...]");
		Console.WriteLine();
		Console.WriteLine($"   YYYY         Defaults to the latest year of puzzles.");
		Console.WriteLine($"     dd         The puzzle day to solve (e.g. 5).");
		Console.WriteLine($"     /download  Download the puzzle input");
		Console.WriteLine($"     /D         Show debug information (e.g. details of exceptions.");
		Console.WriteLine($"     /V         Uses the visualiser if one exists.");
	}
}
