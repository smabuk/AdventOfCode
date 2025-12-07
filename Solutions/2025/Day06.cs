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
		=> _operators = [.. input[^1].TrimmedSplit()
			.Select(op => op switch {"+" => Add, "*" => Mul, _ => None })];

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
			.ChunkBy(string.IsNullOrWhiteSpace, long.Parse)
			.Zip(_operators)
			.Select(zip => new Problem(zip.Second, [.. zip.First.AsEnumerable().Reverse()]))
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
			=> $"{string.Join(Operator is Add ? " + " : " * ", Numbers.Select(n => $"{n,4}")),-25} = {Result}";
	}

	public enum Operator
	{
		None,
		Add,
		Mul,
	}
}

file static class Day06Helpers
{
	// Wrote this and made it generic so it is now in Smab.Helpers v1.9.12
	extension<TSource>(IEnumerable<TSource> items)
	{
		public IEnumerable<List<TOut>> ChunkBy<TOut>(Func<TSource, bool> predicate, Func<TSource, TOut> conversion)
		{
			List<TOut> currentChunk = [];

			foreach (TSource item in items) {
				if (predicate(item)) {
					if (currentChunk.Count > 0) {
						yield return currentChunk;
						currentChunk = [];
					}
				} else {
					currentChunk.Add(conversion(item));
				}
			}

			if (currentChunk.Count > 0) {
				yield return currentChunk;
			}
		}
	}
}
