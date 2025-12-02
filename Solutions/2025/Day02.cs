namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 02: Gift Shop
/// https://adventofcode.com/2025/day/02
/// </summary>
[Description("Gift Shop")]
public partial class Day02
{

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
			.Where(productId => int.IsEvenInteger(productId.Length))
			.Where(productId => productId / (productId.Length / 2).Pow10 == productId % (productId.Length / 2).Pow10)
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
		int length = productId.Length;

		return _divisors[length]
			// Quick first-digit check: extract first digit of productId and first digit at pattern position
			.Where(patternLength => productId / (length - 1).Pow10 % 10 == productId / (length - patternLength - 1).Pow10 % 10)
			.Any(patternLength =>
			{
				long divisor = patternLength.Pow10;
				long firstPattern = productId / (length - patternLength).Pow10;

				return Enumerable.Range(1, (length / patternLength) - 1)
					.All(rep =>
					{
						int position = length - ((rep + 1) * patternLength);
						long currentPattern = productId / position.Pow10 % divisor;
						return currentPattern == firstPattern;
					});
			});
	}
}
