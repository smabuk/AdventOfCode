using System.Text;

using static AdventOfCode.Solutions._2017.Day09Constants;
using static AdventOfCode.Solutions._2017.Day09Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 09: Stream Processing
/// https://adventofcode.com/2017/day/09
/// </summary>
[Description("Stream Processing")]
public sealed partial class Day09 {

	public static string Part1(string[] input) => Solution1(input).ToString();
	public static string Part2(string[] input) => Solution2(input).ToString();

	private static int Solution1(string[] input) => input[0].RemoveGarbage().ScoreStream();
	private static int Solution2(string[] input) => input[0].CountGarbage();
}

file static class Day09Extensions
{
	public static int CountGarbage(this string stream)
	{
		int count = 0;

		bool isInGarbage = false;

		for (int i = 0; i < stream.Length; i++) {
			char character = stream[i];
			if (isInGarbage) {
				if (character is CLOSE_GARBAGE) {
					isInGarbage = false;
				} else if (character is CANCEL) {
					i++;
				} else {
					count++;
				}
			} else if (character is OPEN_GARBAGE) {
				isInGarbage = true;
			}
		}

		return count;
	}

	public static string RemoveGarbage(this string stream)
	{
		StringBuilder sb = new();

		bool isInGarbage = false;

		for (int i = 0; i < stream.Length; i++) {
			char character = stream[i];
			if (isInGarbage) {
				if (character is CLOSE_GARBAGE) {
					isInGarbage = false;
				}
				if (character is CANCEL) {
					i++;
				}
			} else if (character is OPEN_GARBAGE) {
				isInGarbage = true;
			} else {
				sb = sb.Append(character);
			}
		}

		return sb.ToString();
	}

	public static int ScoreStream(this string stream)
	{
		int score = 0;
		int level = 1;
		for (int i = 0; i < stream.Length; i++) {
			char character = stream[i];
			if (character is OPEN_GROUP) {
				score += level++;
			} else  if (character is CLOSE_GROUP) {
				level--;
			}
		}

		return score;
	}
}

internal sealed partial class Day09Types
{
}

file static class Day09Constants
{
	public const char OPEN_GARBAGE  = '<';
	public const char CLOSE_GARBAGE = '>';
	public const char OPEN_GROUP    = '{';
	public const char CLOSE_GROUP   = '}';
	public const char CANCEL        = '!';
}
