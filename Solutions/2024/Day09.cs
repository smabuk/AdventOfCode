using System.Runtime.InteropServices;

namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 09: Disk Fragmenter
/// https://adventofcode.com/2024/day/09
/// </summary>
[Description("Disk Fragmenter")]
public static partial class Day09 {

	public static long Part1(string[] input)
	{
		return input[0]
			.AsDigits<int>()
			.CreateDiskMap()
			.Compact()
			.FileChecksum();
	}

	public static long Part2(string[] input)
	{
		return input[0]
			.AsDigits<int>()
			.CreateDiskMapAsBlocks()
			.Defragment()
			.FileChecksum();
	}


	private static List<int> CreateDiskMap(this IEnumerable<int> diskMapAsInts)
	{
		 return [..
			 diskMapAsInts
			.Chunk(2)
			.SelectMany((sizes, index) =>
				sizes.Length == 2
				? (List<int>)[..Enumerable.Repeat(index, sizes[0]), .. Enumerable.Repeat(EMPTY, sizes[1])]
				: [..Enumerable.Repeat(index, sizes[0])])
			];
	}

	private static List<Block> CreateDiskMapAsBlocks(this IEnumerable<int> diskMapAsInts)
	{
		return [..
			diskMapAsInts
			.Chunk(2)
			.SelectMany((sizes, index) =>
				sizes.Length == 2
				? (List<Block>)[new FileBlock(index, sizes[0]), new EmptyBlock(sizes[1])]
				: [new FileBlock(index, sizes[0])])
			.Where(block => block.BlockSize != 0)
			];
	}

	private static List<int> Compact(this List<int> disk)
	{
		List<int> diskMap = [.. disk];
		Span<int> diskSpan = CollectionsMarshal.AsSpan(diskMap);

		int emptyPtr = 0;
		for (int ptr = diskSpan.Length - 1; ptr > 0; ptr--) {
			if (diskSpan[ptr] is not EMPTY) {
				emptyPtr += diskSpan[emptyPtr..].IndexOf(EMPTY);
				if (emptyPtr >= ptr) {
					break;
				}
				diskSpan[emptyPtr] = diskSpan[ptr];
				diskSpan[ptr] = EMPTY;
			}
		}

		return [.. diskSpan];
	}

	// I don't like this code
	private static List<Block> Defragment(this List<Block> map) 
	{
		List<Block> diskMap = [.. map];
		int maxBlockNo = diskMap[^1].Id;

		for (int blockNo = maxBlockNo; blockNo > 0; blockNo--) {
			int filePtr = diskMap.FindLastIndex(block => block.Id == blockNo);
			Block fileBlock = diskMap[filePtr];
			int firstFreeSpace = diskMap.FindIndex(block => block is EmptyBlock && block.BlockSize >= fileBlock.BlockSize);
			
			if (filePtr > firstFreeSpace && firstFreeSpace != NOT_FOUND) {
				MoveToFreeSpace(diskMap, fileBlock, filePtr, firstFreeSpace);
			}
		}

		return diskMap;

		// Local functions

		static void MoveToFreeSpace(List<Block> diskMap, Block fileBlock, int filePtr, int firstFreeSpace)
		{
			if (diskMap[filePtr - 1] is EmptyBlock || (filePtr + 1 < diskMap.Count && diskMap[filePtr + 1] is EmptyBlock)) {
				// Found empty space before or after the file block
				diskMap.RemoveAt(filePtr);
				CombineFreeSpace(diskMap, filePtr, fileBlock.BlockSize);
			} else {
				// otherwise we can just replace the file block with empty space
				diskMap[filePtr] = new EmptyBlock(fileBlock.BlockSize);
			}

			diskMap[firstFreeSpace] = new EmptyBlock(diskMap[firstFreeSpace].BlockSize - fileBlock.BlockSize);
			diskMap.Insert(firstFreeSpace, fileBlock);
		}

		static void CombineFreeSpace(List<Block> diskMap, int blockPtr, int freeSpace)
		{
			// Combine contiguous empty spaces into 1 block
			if (blockPtr < diskMap.Count && diskMap[blockPtr] is EmptyBlock) {
				freeSpace += diskMap[blockPtr].BlockSize;

				if (diskMap[blockPtr - 1] is EmptyBlock) {
					diskMap.RemoveAt(blockPtr);
				}
			}

			if (diskMap[blockPtr - 1] is EmptyBlock) {
				diskMap[blockPtr - 1] = new EmptyBlock(diskMap[blockPtr - 1].BlockSize + freeSpace);
			} else {
				diskMap[blockPtr] = new EmptyBlock(freeSpace);
			}

			return;
		}
	}

	private static long FileChecksum(this List<int> diskMap)
	{
		return diskMap
			.TakeWhile(id => id is not EMPTY)
			.Index()
			.Sum(block => (long)block.Index * (long)block.Item);
	}

	private static long FileChecksum(this List<Block> diskMap)
	{
		// I hate myself for this code
		long idx = 0;
		return diskMap
			.Sum(block
				=> Enumerable
					.Range(0, block.BlockSize)
					.Sum(i => (block is EmptyBlock ? 0 : block.Id) * idx++));
	}

	private abstract record Block(int Id, int BlockSize);
	private record FileBlock(int Id, int BlockSize) : Block(Id, BlockSize);
	private record EmptyBlock(int BlockSize) : Block(EMPTY, BlockSize);

	private const int  EMPTY      = int.MinValue;
	private const int  NOT_FOUND  = -1;
}
