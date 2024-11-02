using System.Text;

using static AdventOfCode.Solutions._2016.Day09Constants;
namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 09: Explosives in Cyberspace
/// https://adventofcode.com/2016/day/09
/// </summary>
[Description("Explosives in Cyberspace")]
public sealed partial class Day09 {

	public static string Part1(string[] input) => Solution1(input).ToString();
	public static string Part2(string[] input) => Solution2(input).ToString();


	private static long Solution1(string[] input) => input[0].DecompressV1();
	private static long Solution2(string[] input) => input[0].DecompressV2();

}

file static class Day09Extensions
{
	public static long DecompressV1(this string compressed)
	{
		StringBuilder uncompressed = new();

		for (int i = 0; i < compressed.Length; i++) {
			char current = compressed[i];
			if (current == OPEN_BRACKET) {
				int closeBracketIndex = compressed[i..].IndexOf(CLOSE_BRACKET) + i;
				string marker = compressed[(i + 1)..closeBracketIndex];
				string[] tokens = marker.Split('x');
				int chars = tokens[0].As<int>();
				int repeat = tokens[1].As<int>();
				for (int j = 0; j < repeat; j++) {
					_ = uncompressed.Append(compressed[(closeBracketIndex + 1)..(closeBracketIndex + 1 + chars)]);
				}

				i = closeBracketIndex + chars;
			} else {
				_ = uncompressed.Append(current);
			}
		}

		return uncompressed.Length;
	}

	public static long DecompressV2(this string compressed)
	{
		long decompressedLength = 0;

		for (int i = 0; i < compressed.Length; i++) {
			char current = compressed[i];
			if (current == OPEN_BRACKET) {
				int closeBracketIndex = compressed[i..].IndexOf(CLOSE_BRACKET) + i;
				string[] tokens = compressed[(i + 1)..closeBracketIndex].Split('x');
				int chars = tokens[0].As<int>();
				int repeat = tokens[1].As<int>();
				string slice = compressed[(closeBracketIndex + 1)..(closeBracketIndex + 1 + chars)];
				if (slice.Contains(OPEN_BRACKET)) {
					decompressedLength += slice.DecompressV2() * repeat;
				} else {
					decompressedLength += chars * repeat;
				}

				i = closeBracketIndex + chars;
			} else {
				decompressedLength++;
			}
		}

		return decompressedLength;
	}
}

file static class Day09Constants
{
	public const char OPEN_BRACKET = '(';
	public const char CLOSE_BRACKET = ')';
}
