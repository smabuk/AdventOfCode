using static AdventOfCode.Solutions._2017.Day10Constants;
using static AdventOfCode.Solutions._2017.Day10Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 10: Knot Hash
/// https://adventofcode.com/2017/day/10
/// </summary>
[Description("Knot Hash")]
public sealed partial class Day10 {

	public static string Part1(string[] input, params object[]? args)
	{
		int listSize = GetArgument<int>(args, argumentNumber: 1, defaultResult: 256);
		return Solution1(input, listSize).ToString();
	}

	public static string Part2(string[] input) => Solution2(input).ToString();

	private static int Solution1(string[] input, int listSize)
	{
		List<int> sequenceOfLengths = [.. input[0].TrimmedSplit(',').As<int>()];
		List<byte> currentList = [.. Enumerable.Range(0, listSize).Select(i => (byte)i)];

		int skipSize = 0;
		int currentPosition = 0;
		(currentList, _, _) = currentList.KnotHashRound(sequenceOfLengths, currentPosition, skipSize);

		return currentList[0] * currentList[1];
	}


	private static string Solution2(string[] input) {
		const int NO_OF_ROUNDS = 64;
		const int LIST_SIZE = 256;
		byte[] LIST_APPEND = [17, 31, 73, 47, 23];

		List<int> sequenceOfLengths = input.Length == 0
			? [.. LIST_APPEND]
			: [.. input[0]?.Select(c => (byte)c), .. LIST_APPEND];
		List<byte> currentList = [.. Enumerable.Range(0, LIST_SIZE).Select(i => (byte)i)];

		int skipSize = 0;
		int currentPosition = 0;
		for (int i = 0; i < NO_OF_ROUNDS; i++) {
			(currentList, currentPosition, skipSize) = currentList.KnotHashRound(sequenceOfLengths, currentPosition, skipSize);
		}

		List<byte> denseHash = [.. currentList.DenseHash()];

		return denseHash.ToHexString();
	}
}

file static class Day10Extensions
{
	public static string ToHexString(this List<byte> hash) => Convert.ToHexStringLower([.. hash]);

	public static (List<byte>, int, int) KnotHashRound(this List<byte> currentList, List<int> sequenceOfLengths, int position, int skip)
	{
		int skipSize = skip;
		int currentPosition = position;
		foreach (int currentLength in sequenceOfLengths) {
			currentList = currentList.ReverseElements(currentPosition, currentLength);
			currentPosition += currentLength + skipSize++;
		}

		return (currentList, currentPosition % currentList.Count, skipSize % currentList.Count);
	}

	public static IEnumerable<byte> DenseHash(this List<byte> sparseHash)
	{
		for (int i = 0; i < 16; i++) {
			int offset = i * 16;

			yield return (byte)(
				sparseHash[offset]
				^ sparseHash[offset + 1]
				^ sparseHash[offset + 2]
				^ sparseHash[offset + 3]
				^ sparseHash[offset + 4]
				^ sparseHash[offset + 5]
				^ sparseHash[offset + 6]
				^ sparseHash[offset + 7]
				^ sparseHash[offset + 8]
				^ sparseHash[offset + 9]
				^ sparseHash[offset + 10]
				^ sparseHash[offset + 11]
				^ sparseHash[offset + 12]
				^ sparseHash[offset + 13]
				^ sparseHash[offset + 14]
				^ sparseHash[offset + 15]);
		}
	}

	public static List<byte> ReverseElements(this List<byte> list, int position, int noOfElementToReverse)
	{
		int listLength = list.Count;
		byte[] newList = new byte[listLength];
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
