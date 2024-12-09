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
			.CompactDisk()
			.FileChecksum();
	}

	public static long Part2(string[] input)
	{
		return
			input[0]
			.AsDigits<int>()
			.CreateDiskMapAsBlocks()
			.Defragment()
			.FileChecksum();
	}


	private static List<int> CreateDiskMap(this IEnumerable<int> diskMapAsInts)
	{
		List<int> diskMap = [.. Enumerable.Repeat(EMPTY, diskMapAsInts.Sum())];

		int diskMapPtr = 0;
		int blockNo = 0;
		bool fileOrFreeSpace = FILE;

		foreach (int blockSize in diskMapAsInts) {
			if (fileOrFreeSpace is FILE) {
				for (int i = 0; i < blockSize; i++) {
					diskMap[diskMapPtr++] = blockNo;
				}

				blockNo++;
				fileOrFreeSpace = FREE_SPACE;
			} else {
				for (int i = 0; i < blockSize; i++) {
					diskMap[diskMapPtr++] = EMPTY;
				}

				fileOrFreeSpace = FILE;
			}
		}

		return diskMap;
	}

	private static List<Block> CreateDiskMapAsBlocks(this IEnumerable<int> diskMapAsInts)
	{
		return [.. diskMapAsInts
			.Chunk(2)
			.SelectMany((sizes, index) =>
				sizes.Length == 2
				? (List<Block>)[new FileBlock(index, sizes[0]), new EmptyBlock(sizes[1])]
				: (List<Block>)[new FileBlock(index, sizes[0])])
			];
	}

	private static List<int> CompactDisk(this List<int> map)
	{
		List<int> diskMap = [.. map];

		for (int ptr = diskMap.Count - 1; ptr > 0; ptr--) {
			if (diskMap[ptr] is not EMPTY) {
				int emptyPtr = diskMap.IndexOf(EMPTY);
				if (emptyPtr >= ptr) {
					break;
				}
				diskMap[emptyPtr] = diskMap[ptr];
				diskMap[ptr] = EMPTY;
			}
		}

		return diskMap;
	}

	// I don't like this code
	private static List<Block> Defragment(this List<Block> map) 
	{
		List<Block> diskMap = [.. map];

		int maxBlockNo = diskMap[^1].Id;
		for (int blockNo = maxBlockNo; blockNo > 0; blockNo--) {
			
			int blockPtr = diskMap.FindIndex(block => block.Id == blockNo);
			Block fileBlock = diskMap[blockPtr];
			int firstFreeSpace = diskMap.FindIndex(block => block is EmptyBlock && block.BlockSize >= fileBlock.BlockSize);
			
			if (blockPtr > firstFreeSpace && firstFreeSpace > 0) {
				int ptrToPreceding = blockPtr - 1;
				if (diskMap[ptrToPreceding] is EmptyBlock) {
					diskMap.RemoveAt(blockPtr);
					diskMap[ptrToPreceding] = diskMap[ptrToPreceding] with { BlockSize = diskMap[ptrToPreceding].BlockSize + fileBlock.BlockSize };
					// Combine contiguous empty spaces into 1 block
					ptrToPreceding--;
					if (diskMap[ptrToPreceding] is EmptyBlock) {
						for (int i = ptrToPreceding; diskMap[i] is EmptyBlock; i--) {
							diskMap[i] = diskMap[i] with { BlockSize = diskMap[i].BlockSize + diskMap[i + 1].BlockSize };
							diskMap.RemoveAt(i + 1);
						}
					}
				} else {
					diskMap[blockPtr] = new EmptyBlock(fileBlock.BlockSize);
				}

				Block freeSpaceBlock = diskMap[firstFreeSpace];
				diskMap[firstFreeSpace] = freeSpaceBlock with { BlockSize = freeSpaceBlock.BlockSize - fileBlock.BlockSize };
				diskMap.Insert(firstFreeSpace, fileBlock);
			}
		}

		return diskMap;
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
		long checkSum = 0;
		int idx = 0;
		for (int i = 0; i < diskMap.Count; i++) {
			if (diskMap[i] is FileBlock fileBlock) {
				checkSum += Enumerable.Range(0, fileBlock.BlockSize).Sum(i => (long)fileBlock.Id * (long)idx++);
			} else {
				idx += diskMap[i].BlockSize;
			}
		}

		return checkSum;
	}

	private abstract record Block(int Id, int BlockSize);
	private record FileBlock(int Id, int BlockSize) : Block(Id, BlockSize);
	private record EmptyBlock(int BlockSize) : Block(EMPTY, BlockSize);

	private const bool FILE       = true;
	private const bool FREE_SPACE = false;
	private const int  EMPTY      = int.MinValue;
}
