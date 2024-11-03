using System.Security.Cryptography;
using System.Text;
using static AdventOfCode.Solutions._2016.Day14Constants;
using static AdventOfCode.Solutions._2016.Day14Types;
namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Day 14: One-Time Pad
/// https://adventofcode.com/2016/day/14
/// </summary>
[Description("One-Time Pad")]
public sealed partial class Day14 {

	public static string Part1(string[] input) => Solution1(input).ToString();
	public static string Part2(string[] input) => Solution2(input).ToString();

	private static long Solution1(string[] input) {
		string salt = input[0];
		int count = 0;
		foreach (HashCache hash in salt.GetNextHash()) {
			count++;
			if (count == ONETIME_PAD_INDEX) {
				return hash.Index;
			}
		}

		throw new ApplicationException("Should never reach here!");
	}

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class Day14Extensions
{	public static IEnumerable<HashCache> GetNextHash(this string salt)
	{
		Queue<HashCache> values = new(SLIDING_WINDOW + 1);
		
		long i = 0;
		while (true) {
			string hash = Convert.ToHexStringLower(MD5.HashData(Encoding.UTF8.GetBytes($"{salt}{i}")));
			values.Enqueue(new HashCache(i, hash.TripleChar(), hash));
			if (values.Count == SLIDING_WINDOW + 1) {
				HashCache cache = values.Dequeue();
				if (cache.Repeat is not null) {
					string quintet = new((char)cache.Repeat, 5);
					if (values.Any(v => v.Hash.Contains($"{quintet}"))) {
						yield return cache;
					}
				}
			}

			i++;
		}
	}

	public static char? TripleChar(this string hash) => TripleRegEx().Matches(hash).FirstOrDefault()?.Value[0];

}

internal sealed partial class Day14Types
{
	internal record struct HashCache(long Index, char? Repeat, string Hash)
	{
		public static implicit operator (long index, char? repeat, string hash)(HashCache value) => (value.Index, value.Repeat, value.Hash);
		public static implicit operator HashCache((long index, char? repeat, string hash) value) => new(value.index, value.repeat, value.hash);
	}
	
	[GeneratedRegex("""(?<triple>([a-f0-9])\1{2})""")]
	public static partial Regex TripleRegEx();
}

file static class Day14Constants
{
	public const int ONETIME_PAD_INDEX = 64;
	public const int SLIDING_WINDOW = 1000;
}
