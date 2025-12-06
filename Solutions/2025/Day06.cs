namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 06: Trash Compactor
/// https://adventofcode.com/2025/day/06
/// </summary>
[Description("Trash Compactor")]
[GenerateVisualiser]
public partial class Day06 {

	[Init]
	public static void LoadInstructions(string[] input)
	{
		numbers = input[..^1].AsNumbers<long>().To2dArray();
		operators = [.. input[^1].TrimmedSplit()];
	}

	private static long[,] numbers = default!;
	private static string[] operators = [];

	public static long Part1()
	{
		long sum = 0;

		for (int i = 0; i < numbers.ColsCount(); i++) {
			string op = operators[i];
			sum += op switch
			{
				"+" => numbers.Col(i).Sum(),
				"*" => numbers.Col(i).Aggregate(1L, (long curr, long val) => curr * val),
				_ => 0
			};
		}

		return sum;
	}

	public static string Part2() => NO_SOLUTION_WRITTEN_MESSAGE;
}
