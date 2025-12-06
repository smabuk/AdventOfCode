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
				OP_MUL => homeworkAsNumbers.Col(i).Aggregate(1L, (curr, val) => curr * val),
				_ => 0
			});
	}

	public static long Part2(string[] input)
	{
		char[,] homeworkAsChars = input[..^1].To2dArray();
		string[] operators = [.. input[^1].TrimmedSplit()];

		List<int> problemStarts = [.. homeworkAsChars.FindProblemStarts()];

		return problemStarts
			.Take(problemStarts.Count - 1)
			.Select((problemStart, i)
				=> Enumerable.Sequence(problemStart, problemStarts[i + 1] - 2, 1)
					.Aggregate(operators[i] == OP_MUL ? 1L : 0, (curr, idx) =>
					{
						long cephalapod_number = string.Join("", homeworkAsChars.Col(idx)).As<long>();
						return operators[i] switch
						{
							OP_ADD => curr + cephalapod_number,
							OP_MUL => curr * cephalapod_number,
							_ => 0
						};
					}))
			.Sum();
	}
}

file static class Day06Helpers
{
	extension(char[,] homework)
	{
		public IEnumerable<int> FindProblemStarts()
		{
			const char EMPTY = ' ';
			yield return 0;

			int i = 0;
			for (i = 0; i < homework.ColsCount(); i++) {
				if (homework.Col(i).All(x => x is EMPTY)) {
					yield return (i + 1);
				}
			}

			yield return (i + 1);
		}
	}
}
