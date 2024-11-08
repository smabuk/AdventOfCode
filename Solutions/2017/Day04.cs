using static AdventOfCode.Solutions._2017.Day04Constants;
using static AdventOfCode.Solutions._2017.Day04Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 04: High-Entropy Passphrases
/// https://adventofcode.com/2016/day/04
/// </summary>
[Description("High-Entropy Passphrases")]
public sealed partial class Day04 {

	public static string Part1(string[] input) => Solution1(input).ToString();
	public static string Part2(string[] input) => Solution2(input).ToString();

	private static int Solution1(string[] input) =>
		input.Count(passphrase => passphrase.HasNoRepeatingWords());

	private static int Solution2(string[] input) =>
		input.Count(passphrase => passphrase.HasNoRepeatingWords() && passphrase.HasNoAnagrams());
}

file static class Day04Extensions
{
	public static bool HasNoRepeatingWords(this string passphrase)
	{
		string[] words = passphrase.TrimmedSplit(SPACE);
		return words.Length == words.Distinct().Count();
	}

	public static bool HasNoAnagrams(this string passphrase)
	{
		List<string> words = [.. passphrase.TrimmedSplit(SPACE)];
		foreach (string word in words) {
			if (words.Count(w => word.Order().SequenceEqual(w.Order())) != 1) {
				return false;
			}
		}

		return true;
	}
}

internal sealed partial class Day04Types
{
}

file static class Day04Constants
{
	public const char SPACE = ' ';
}
