namespace AdventOfCode.Solutions._2025;

/// <summary>
/// Day 02: Gift Shop
/// https://adventofcode.com/2025/day/02
/// </summary>
[Description("Gift Shop")]
public partial class Day02 {

	private static List<LongRange> _ranges = [];

	[Init]
	public static void LoadInstructions(string[] input) => _ranges = [
		.. input[0]
		.TrimmedSplit(',')
		.Select(s => ParseToLongRange(s))
		];

	public static long Part1()
	{
		return _ranges
			.SelectMany(range => Enumerable.Sequence(range.Start, range.End, 1))
			.Where(productId => IsInvalidProductIdForPart1(productId.ToString()))
			.Sum();
	}

	public static long Part2()
	{
		return _ranges
			.SelectMany(range => Enumerable.Sequence(range.Start, range.End, 1))
			.Where(productId => IsInvalidProductIdForPart2(productId.ToString()))
			.Sum();
	}

	private static bool IsInvalidProductIdForPart1(string productId)
	{
		int length = productId.Length;

		if (length.IsOdd()) {
			return false;
		}

		length /= 2;

		for (int i = 0; i < length; i++) {
			if (productId[i] != productId[i+length]) {
				return false;
			}
		}

		return true;
	}

	private static bool IsInvalidProductIdForPart2(string productId)
	{
		int length = productId.Length;

		for (int multiple = 1; multiple <= length / 2; multiple++) {

			if (length % multiple != 0) {
				continue;
			}

			if (productId[0] != productId[multiple]) {
				continue;
			}

			List<string> chunks = [.. productId.Chunk(multiple).Select(chunk => new string(chunk))];
			if (chunks.All(chunk => chunk == chunks[0])) {
				return true;
			}

		}

		return false;
	}


	private static LongRange ParseToLongRange(string s) {
		string[] parts = s.TrimmedSplit('-');
		return new LongRange(parts[0].As<long>(), parts[1].As<long>());
	}
}
