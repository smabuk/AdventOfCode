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
		input.ToDigits().Captcha(1);

	private static int Solution2(string[] input) =>
		input.ToDigits().Captcha(input[0].Length / 2);
}

file static class Day01Extensions
{
	public static int Captcha(this List<int> digits, int slicePoint)
	{
		return digits.Zip([.. digits[slicePoint..], ..digits[..slicePoint]])
			.Where(pair => pair.First == pair.Second)
			.Sum(pair => pair.First);
	}

	public static List<int> ToDigits(this string[] input) => [..input[0].AsDigits<int>()];
}
