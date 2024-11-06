using static AdventOfCode.Solutions._2017.Day01Constants;
using static AdventOfCode.Solutions._2017.Day01Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 01: Inverse Captcha
/// https://adventofcode.com/2016/day/01
/// </summary>
[Description("Inverse Captcha")]
public sealed partial class Day01 {

	public static string Part1(string[] input) => Solution1(input).ToString();
	public static string Part2(string[] input) => Solution2(input).ToString();

	private static int Solution1(string[] input) => 
		input[0].AsDigits<int>().ToList()
		.CheckSumPt1();

	private static int Solution2(string[] input) =>
		input[0].AsDigits<int>().ToList()
		.CheckSumPt2();
}

file static class Day01Extensions
{
	public static int CheckSumPt1(this List<int> digits)
	{
		return digits.Zip([.. digits[1..], digits[0]])
			.Where(pair => pair.First == pair.Second)
			.Sum(pair => pair.First);
	}

	public static int CheckSumPt2(this List<int> digits)
	{
		return digits.Zip([.. digits[(digits.Count/2)..], ..digits[..(digits.Count/2)]])
			.Where(pair => pair.First == pair.Second)
			.Sum(pair => pair.First);
	}

}

internal sealed partial class Day01Types
{
}

file static class Day01Constants
{
}
