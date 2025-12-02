namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 02: Gift Shop
/// https://adventofcode.com/2025/day/02
/// </summary>
[Description("Gift Shop")]
public partial class Day02 {

	private static List<LongRange> _ranges = [];

	[Init]
	public static void LoadInstructions(string[] input)
		=> _ranges = [.. input[0].Split(',').Select(ParseToLongRange)];

	public static long Part1()
	{
		return _ranges
			.SelectMany(range => range.Values)
			.Select(productId => ( ProductId: productId, IdString: productId.ToString(), productId.ToString().Length))
			.Where(idInfo => idInfo.Length.IsEven())
			.Where(idInfo => idInfo.IdString[0..(idInfo.Length / 2)] == idInfo.IdString[(idInfo.Length / 2)..])
			.Sum(idInfo => idInfo.ProductId);
	}

	public static long Part2()
	{
		return _ranges
			.SelectMany(range => range.Values)
			.Where(productId => IsInvalidProductIdForPart2(productId.ToString()))
			.Sum();
	}

	private static bool IsInvalidProductIdForPart2(string productId)
	{
		return Enumerable.Range(1, productId.Length / 2)
			.Where(multiple => productId.Length % multiple == 0)
			.Where(multiple => productId[0] == productId[multiple])
			.Any(multiple => {
				string firstchunk = productId[0..multiple];
				return productId.Chunk(multiple).Skip(1).All(chunk => chunk.SequenceEqual(firstchunk));
			});
	}

	private static LongRange ParseToLongRange(string s) {
		string[] parts = s.Split('-');
		return new LongRange(parts[0].As<long>(), parts[1].As<long>());
	}
}
