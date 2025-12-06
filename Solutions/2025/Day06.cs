namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 06: Trash Compactor
/// https://adventofcode.com/2025/day/06
/// </summary>
[Description("Trash Compactor")]
public partial class Day06
{
	private const string OP_ADD = "+";
	private const string OP_MUL = "*";

	public static long Part1(string[] input)
	{
		long[,] homeworkAsNumbers = input[..^1].AsNumbers<long>().To2dArray();
		string[] operators = [.. input[^1].TrimmedSplit()];

		return Enumerable.Range(0, homeworkAsNumbers.ColsCount())
			.Sum(i => operators[i] switch
			{
				OP_ADD => homeworkAsNumbers.Col(i).Sum(),
				OP_MUL => homeworkAsNumbers.Col(i).Aggregate(1L, (total, number) => total * number),
				_ => 0
			});
	}

	public static long Part2(string[] input)
	{
		char[,] homeworkAsChars = input[..^1].To2dArray();
		string[] operators = [.. input[^1].TrimmedSplit()];

		List<int> problemStartIndexes = [.. homeworkAsChars.FindProblemStarts()];

		return problemStartIndexes[..^1]
			.Index()
			.Select(start
				=> Enumerable.Sequence(start.Item, problemStartIndexes[start.Index + 1] - 2, 1)
					.Aggregate(operators[start.Index] is OP_MUL ? 1L : 0, (total, idx) =>
						operators[start.Index] switch
						{
							OP_ADD => total + homeworkAsChars.Col(idx).ToCephalopodNumber(),
							OP_MUL => total * homeworkAsChars.Col(idx).ToCephalopodNumber(),
							_ => 0
						}
					))
			.Sum();
	}
}

file static class Day06Helpers
{
	const char EMPTY = ' ';

	extension(IEnumerable<char> chars)
	{
		public long ToCephalopodNumber() => string.Join("", chars).As<long>();
	}

	extension(char[,] homework)
	{
		public IEnumerable<int> FindProblemStarts() =>
			[
				0,
				.. Enumerable.Range(1, homework.ColsCount()).Where(i => homework.Col(i - 1).All(x => x is EMPTY))
				, homework.ColsCount() + 1
			];
	}
}
