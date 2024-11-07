using static AdventOfCode.Solutions._2017.Day03Constants;
using static AdventOfCode.Solutions._2017.Day03Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 03: Spiral Memory
/// https://adventofcode.com/2016/day/03
/// </summary>
[Description("Spiral Memory")]
public sealed partial class Day03 {

	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		int target = input[0].As<int>();
		int rows = ((int)Math.Sqrt(target - 1)) + (int.IsEvenInteger((int)Math.Sqrt(target - 1)) ? 1 : 2);
		int bottomRight = rows * rows;
		int spiralLength = bottomRight - ((rows - 2) * (rows - 2));
		int sideLengthMinus1 = (spiralLength / 4);
		int offset = int.Abs((sideLengthMinus1 / 2) - ((bottomRight - target) % sideLengthMinus1)) ;
		return (rows / 2) + offset;
	}

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class Day03Extensions
{
}

internal sealed partial class Day03Types
{
}

file static class Day03Constants
{
}
