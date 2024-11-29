using static AdventOfCode.Solutions._2017.Day14Constants;
using static AdventOfCode.Solutions._2017.Day14Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 14: Disk Defragmentation
/// https://adventofcode.com/2017/day/14
/// </summary>
[Description("Disk Defragmentation")]
public sealed partial class Day14 {

	[Init]
	public static void Init(string[] input) => LoadInstructions(input);
	public static string Part1(string[] _) => Solution1().ToString();
	public static string Part2(string[] _) => Solution2().ToString();

	private static List<string> _binaryKnotHashes = [];

	private static void LoadInstructions(string[] input)
	{
		_binaryKnotHashes = [
			..Enumerable.Range(0, 128)
			.Select(i => $"{input[0]}-{i}".KnotHash().AsBinaryFromHex()[..128])];
	}

	private static int Solution1()
	{
		return _binaryKnotHashes
			.Select(i => i.Count(c => c is ONE))
			.Sum();
	}

	private static int Solution2() {
		return _binaryKnotHashes
			.To2dArray()
			.CountRegions((char c) => c is ONE);
	}
}

file static class Day14Extensions
{
	public static int CountRegions(this char[,] grid, Predicate<char> predicate)
	{
		int count = 0;
		bool[,] visited = new bool[grid.ColsCount(), grid.RowsCount()];

		foreach (Cell<char> cell in grid.WalkWithValues()) {
			if (predicate(cell.Value) && !visited[cell.Col, cell.Row]) {
				DFS(grid, visited, cell.Col, cell.Row, predicate);
				count++;
			}
		}

		return count;
	}

	public static void DFS(char[,] grid, bool[,] visited, int col, int row, Predicate<char> predicate)
	{
		Stack<(int, int)> stack = [];
		stack.Push((col, row));
		visited[col, row] = true;

		while (stack.Count > 0) {
			(int currentCol, int currentRow) = stack.Pop();

			foreach (Cell<char> adjacentCell in grid.GetAdjacentCells(currentCol, currentRow)) {
				if (predicate(adjacentCell.Value) && !visited[adjacentCell.Col, adjacentCell.Row]) {
					stack.Push((adjacentCell.Col, adjacentCell.Row));
					visited[adjacentCell.Col, adjacentCell.Row] = true;
				}
			}
		}
	}

	public static string ToHexString(this List<byte> hash) => Convert.ToHexStringLower([.. hash]);

	public static string KnotHash(this string keyString)
	{
		const int LIST_SIZE = 256;
		const int NO_OF_ROUNDS = 64;
		byte[] LIST_APPEND = [17, 31, 73, 47, 23];

		List<int> sequenceOfLengths = [.. keyString.Select(c => (byte)c), ..LIST_APPEND];

		List<byte> currentList = [.. Enumerable.Range(0, LIST_SIZE).Select(i => (byte)i)];
		int skipSize = 0;
		int currentPosition = 0;
		for (int i = 0; i < NO_OF_ROUNDS; i++) {
			(currentList, currentPosition, skipSize) = currentList.KnotHashRound(sequenceOfLengths, currentPosition, skipSize);
		}

		List<byte> denseHash = [.. currentList.DenseHash()];

		return denseHash.ToHexString();
	}

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
		for (int i = 0; i < sparseHash.Count / 16; i++) {
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
		for (int i = 0; i < listLength; i++) {
			int currentPosition = position + i;
			newList[currentPosition % listLength] =
				currentPosition < position + noOfElementToReverse
				? list[(position + noOfElementToReverse - i - 1) % listLength] // reversed
				: list[currentPosition % listLength];
		}

		return [.. newList];
	}
}

internal sealed partial class Day14Types
{
}

file static class Day14Constants
{
	public const char ONE = '1'; 
}
