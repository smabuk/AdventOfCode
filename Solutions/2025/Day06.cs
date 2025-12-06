using static AdventOfCode.Solutions._2025.Day06.Operator;

namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 06: Trash Compactor
/// https://adventofcode.com/2025/day/06
/// </summary>
[Description("Trash Compactor")]
[GenerateVisualiser]
public partial class Day06
{
	[Init]
	public static void LoadOperators(string[] input)
		=> _operators = [.. input[^1].TrimmedSplit().Select(op => op is "+" ? Add : Mul)];

	private static List<Operator> _operators = [];

	public static long Part1(string[] input)
	{
		VisualiseStrings([$"{_operators.Count} problems:", ""]);

		return input[..^1]
			.AsNumbers<int>()
			.To2dArray()
			.Transpose()
			.Rows()
			.Index()
			.Select(numbers => new Problem(_operators[numbers.Index], [.. numbers.Item]))
			.Sum(problem =>
			{
				VisualiseString(problem.ToString());
				return problem.Result;
			});
	}

	public static long Part2(string[] input)
	{
		VisualiseStrings([$"{_operators.Count} problems:", ""]);

		char[,] homeworkAsChars = input[..^1].To2dArray();
		List<int> problemStartIndexes = [.. homeworkAsChars.FindProblemStarts()];

		return problemStartIndexes[..^1]
			.Index()
			.Select(indexedStarts => Enumerable
				.Sequence(indexedStarts.Item, problemStartIndexes[indexedStarts.Index + 1] - 2, 1)
				.Select(col => homeworkAsChars.ToCephalopodNumber(col)))
			.Index()
			.Select(indexedNumbers => new Problem(_operators[indexedNumbers.Index], [.. indexedNumbers.Item.Reverse()]))
			.Reverse()
			.Sum(problem =>
			{
				VisualiseString(problem.ToString());
				return problem.Result;
			});
	}

	private record Problem(Operator Operator, List<long> Numbers)
	{
		public long Result = Operator switch
		{
			Add => Numbers.Sum(),
			Mul => Numbers.Aggregate(1L, (total, number) => total * number),
			_ => 0
		};

		public override string ToString()
			=> $"{string.Join(Operator is Add ? " + ": " * ", Numbers)} = {Result}";
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
