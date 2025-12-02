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
	{
		_ranges = [.. input[0].TrimmedSplit(',').Select(s => ParseToLongRange(s))];
	}

	public static long Part1()
	{
		long sum = 0;

		foreach (LongRange productIdRange in _ranges) {
			for (long l = productIdRange.Start; l <= productIdRange.End; l++) {
				if (IsInvalidProductId(l.ToString())) {
					sum += l;
				}
			}
		}

		return sum;
	}

	public static string Part2() => NO_SOLUTION_WRITTEN_MESSAGE;


	private static bool IsInvalidProductId(string productId)
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


	private static LongRange ParseToLongRange(string s) {
		string[] parts = s.TrimmedSplit('-');
		return new LongRange(parts[0].As<long>(), parts[1].As<long>());
	}
}
