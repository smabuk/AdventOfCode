using static AdventOfCode.Solutions._2017.Day01Constants;
using static AdventOfCode.Solutions._2017.Day01Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 01: Inverse Captcha
/// https://adventofcode.com/2016/day/01
/// </summary>
[Description("Inverse Captcha")]
public sealed partial class Day01 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static int Solution1(string[] input) => 
		input[0].AsDigits<int>().ToList()
		.CheckSum();

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class Day01Extensions
{
	public static int CheckSum(this List<int> digits)
	{
		return digits.Zip([.. digits[1..], digits[0]])
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
