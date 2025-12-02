namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 02: Gift Shop
/// https://adventofcode.com/2025/day/02
/// </summary>
[Description("Gift Shop")]
public partial class Day02 {

	private static List<LongRange> _ranges = [];
	private static readonly Dictionary<int, int[]> _divisors = Enumerable.Range(1, 10)
		.ToDictionary(
			length => length,
			length => Enumerable.Range(1, length / 2)
				.Where(patternLength => length % patternLength == 0)
				.ToArray()
		);

	[Init]
	public static void LoadInstructions(string[] input)
		=> _ranges = [.. input[0].Split(',').Select(ParseToRange)];

	private static LongRange ParseToRange(string s)
	{
		string[] parts = s.Split('-');
		return new LongRange(parts[0].As<long>(), parts[1].As<long>());
	}

	public static long Part1()
	{
		return _ranges
			.SelectMany(range => range.Values)
			.Where(productId => int.IsEvenInteger(QuickLength(productId)))
			.Where(productId => productId / Pow10[QuickLength(productId) / 2] == productId % Pow10[QuickLength(productId) / 2])
			.Sum();
	}

	public static long Part2()
	{
		return _ranges
			.SelectMany(range => range.Values)
			.Where(IsInvalidProductIdUsingLongs)
			.Sum();
	}

	// Twice as fast as the string-based approach
	private static bool IsInvalidProductIdUsingLongs(long productId)
	{
		int length = QuickLength(productId);

		return _divisors[length]
			// Quick first-digit check: extract first digit of productId and first digit at pattern position
			.Where(patternLength => productId / Pow10[length - 1] % 10 == productId / Pow10[length - patternLength - 1] % 10)
			.Any(patternLength => {
				long divisor = Pow10[patternLength];
				long firstPattern = productId / Pow10[length - patternLength];

				return Enumerable.Range(1, (length / patternLength) - 1)
					.All(rep => {
						int position = length - ((rep + 1) * patternLength);
						long currentPattern = productId / Pow10[position] % divisor;
						return currentPattern == firstPattern;
					});
			});
	}

	//private static bool IsInvalidProductIdUsingStrings(string productId)
	//{
	//	return Enumerable.Range(1, productId.Length / 2)
	//		.Where(multiple => productId.Length % multiple == 0)
	//		.Where(multiple => productId[0] == productId[multiple])
	//		.Any(multiple => {
	//			string firstchunk = productId[0..multiple];
	//			return productId.Chunk(multiple).Skip(1).All(chunk => chunk.SequenceEqual(firstchunk));
	//		});
	//}

	static int QuickLength(long value)
	{
		return value switch
		{
			< 10 => 1,
			< 100 => 2,
			< 1_000 => 3,
			< 10_000 => 4,
			< 100_000 => 5,
			< 1_000_000 => 6,
			< 10_000_000 => 7,
			< 100_000_000 => 8,
			< 1_000_000_000 => 9,
			< 10_000_000_000 => 10,
			_ => throw new ArgumentOutOfRangeException(nameof(value))
		};
	}

	// Precomputed powers of 10 for speed (supports up to 10 digits)
	private static readonly int[] Pow10 = [
		1, 10, 100, 1_000, 10_000, 100_000,1_000_000, 10_000_000, 100_000_000, 1_000_000_000
	];
}
