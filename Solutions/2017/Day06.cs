using static AdventOfCode.Solutions._2017.Day06Constants;
using static AdventOfCode.Solutions._2017.Day06Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 06: Memory Reallocation
/// https://adventofcode.com/2016/day/06
/// </summary>
[Description("Memory Reallocation")]
public sealed partial class Day06 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		HashSet<string> seen = [];
		int cycles;
		List<int> banks = [.. input[0].TrimmedSplit(TAB).As<int>()];
		_ = seen.Add(banks.AsHashString());

		for (cycles = 1; ; cycles++) {
			int max = banks.Max();
			int redistributionBank = banks.FindIndex(b => b == max);
			banks[redistributionBank] = 0;
			for (int i = 0; i < banks.Count; i++) {
				int bank = (i + redistributionBank + 1) % banks.Count;
				banks[bank] += max / banks.Count;
				banks[bank] += i < (max % banks.Count) ? 1 : 0;
			}

			if (!seen.Add(banks.AsHashString())) {
				break;
			}
		}

		return cycles;
	}

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class Day06Extensions
{
	public static string AsHashString(this IEnumerable<int> ints) => string.Join(",", ints);

}

internal sealed partial class Day06Types
{
}

file static class Day06Constants
{
	public const char TAB = '\t';
}
