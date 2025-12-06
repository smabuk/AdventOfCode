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
			.Zip(_operators)
			.Select(zip => new Problem(zip.Second, [.. zip.First]))
			.Sum(problem =>
			{
				VisualiseString(problem.ToString());
				return problem.Result;
			});
	}

	public static long Part2(string[] input)
	{
		VisualiseStrings([$"{_operators.Count} problems:", ""]);

		return input[..^1]
			.To2dArray()
			.ColsAsStrings()
			.ChunkByEmpty()
			.Zip(_operators)
			.Select(zip => new Problem(zip.Second, [.. zip.First.Select(n => (long)n).Reverse()]))
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
			=> $"{string.Join(Operator is Add ? " + " : " * ", Numbers)} = {Result}";
	}

	public enum Operator
	{
		Add,
		Mul,
	}
}

file static class Day06Helpers
{
	extension(IEnumerable<string> numbers)
	{
		public IEnumerable<List<int>> ChunkByEmpty()
		{
			List<int> currentChunk = [];

			foreach (string number in numbers) {
				if (number.IsWhiteSpace()) {
					if (currentChunk.Count > 0) {
						yield return currentChunk;
						currentChunk = [];
					}
				} else {
					currentChunk.Add(number.As<int>());
				}
			}

			if (currentChunk.Count > 0) {
				yield return currentChunk;
			}
		}
	}
}
