using static AdventOfCode.Solutions._2017.Day10Constants;
using static AdventOfCode.Solutions._2017.Day10Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 10: Knot Hash
/// https://adventofcode.com/2016/day/10
/// </summary>
[Description("Knot Hash")]
public sealed partial class Day10 {

	public static string Part1(string[] input, params object[]? args)
	{
		int listSize = GetArgument<int>(args, argumentNumber: 1, defaultResult: 256);
		return Solution1(input, listSize).ToString();
	}

	public static string Part2(string[] input, params object[]? args) => Solution2(input).ToString();

	private static int Solution1(string[] input, int listSize) {
		List<int> sequenceOfLengths = [..input[0].TrimmedSplit(',').As<int>()];
		List<int> currentList = [.. Enumerable.Range(0, listSize)];

		int skipSize = 0;
		int currentPosition = 0;
		foreach (int currentLength in sequenceOfLengths) {
			currentList = currentList.ReverseElements(currentPosition, currentLength);
			currentPosition += currentLength + skipSize++;
		}

		return currentList[0] * currentList[1];
	}

	private static string Solution2(string[] input) {
		return NO_SOLUTION_WRITTEN_MESSAGE;
	}
}

file static class Day10Extensions
{
	public static List<int> ReverseElements(this List<int> list, int position, int noOfElementToReverse)
	{
		int listLength = list.Count;
		int[] newList = new int[listLength];
		for (int i = 0; i < listLength; i++)
		{
			int currentPosition = position + i;
			newList[currentPosition % listLength] = 
				currentPosition < position + noOfElementToReverse
				? list[(position + noOfElementToReverse - i - 1) % listLength] // reversed
				: list[currentPosition % listLength];
		}

		return [..newList];
	}
}

internal sealed partial class Day10Types
{
}

file static class Day10Constants
{
}
