using static AdventOfCode.Solutions._2025.Day06.Operator;

namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 06: Trash Compactor
/// https://adventofcode.com/2025/day/06
/// </summary>
[Description("Trash Compactor")]
public partial class Day06
{
	private const string OP_ADD = "+";

	[Init]
	public static void LoadOperators(string[] input)
		=> _operators = [.. input[^1].TrimmedSplit().Select(op => op is OP_ADD ? Add : Mul)];

	private static List<Operator> _operators = [];

	public static long Part1(string[] input)
	{
		return input[..^1]
			.AsNumbers<int>()
			.To2dArray()
			.Transpose()
			.Rows()
			.Index()
			.Select(numbers => new Problem(_operators[numbers.Index], [.. numbers.Item]))
			.Sum(problem => problem.Solve());
	}

	public static long Part2(string[] input)
	{
		char[,] homeworkAsChars = input[..^1].To2dArray();
		List<int> problemStartIndexes = [.. homeworkAsChars.FindProblemStarts()];

		return problemStartIndexes[..^1]
			.Index()
			.Select(indexedStarts => Enumerable
				.Sequence(indexedStarts.Item, problemStartIndexes[indexedStarts.Index + 1] - 2, 1)
				.Select(col => homeworkAsChars.ToCephalopodNumber(col)))
			.Index()
			.Select(indexedNumbers => new Problem(_operators[indexedNumbers.Index], [.. indexedNumbers.Item]))
			.Sum(problem => problem.Solve());
	}

	private record Problem(Operator Operator, List<long> Numbers)
	{
		public long Solve() => Operator switch
		{
			Add => Numbers.Sum(),
			Mul => Numbers.Aggregate(1L, (total, number) => total * number),
			_ => 0
		};
	}

	public enum Operator
	{
		Add,
		Mul,
	}
}

file static class Day06Helpers
{
	const char EMPTY = ' ';

	extension(char[,] homework)
	{
		public int ToCephalopodNumber(int col) => string.Join("", homework.Col(col)).As<int>();

		public IEnumerable<int> FindProblemStarts() =>
			[
				0,
				.. Enumerable.Range(1, homework.ColsCount()).Where(i => homework.Col(i - 1).All(x => x is EMPTY))
				, homework.ColsCount() + 1
			];
	}
}
