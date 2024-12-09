namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 09: Disk Fragmenter
/// https://adventofcode.com/2024/day/09
/// </summary>
[Description("Disk Fragmenter")]
public partial class Day09 {

	public static long Part1(string[] input)
	{
		string denseDiskMap = input[0];
		List<int> diskMapAsInts = [.. denseDiskMap.AsDigits<int>()];
		int blockCount = diskMapAsInts.Sum();

		List<int> diskMap = new (blockCount);
		for (int i = 0; i < blockCount; i++) {
			diskMap.Add(EMPTY);
		}

		bool fileOrFreeSpace = FILE;
		int diskMapPtr = 0;
		int blockNo = 0;
		foreach (int item in diskMapAsInts) {
			if (fileOrFreeSpace is FILE) {
				for (int i = 0; i < item; i++) {
					diskMap[diskMapPtr++] = blockNo;
				}
				blockNo++;
				fileOrFreeSpace = FREE_SPACE;
			} else {
				for (int i = 0; i < item; i++) {
					diskMap[diskMapPtr++] = EMPTY;
				}
				fileOrFreeSpace = FILE;
			}
		}

		// Defragment
		for (int i = 0; i < blockCount; i++) {
			if (diskMap[^(i+1)] is not EMPTY) {
				int ptr = diskMap.IndexOf(EMPTY);
				if (ptr >= blockCount - i) {
					break;
				}
				diskMap[ptr] = diskMap[^(i+1)];
				diskMap[^(i+1)] = EMPTY;
			}
		}

		long checkSum = 0;
		for (int i = 0; i < blockCount; i++) {
			if (diskMap[i] is EMPTY) {
				break;
			}
			checkSum += diskMap[i] * i;
		}


		return checkSum;
	}

	public static long Part2(string[] input)
	{
		string denseDiskMap = input[0];
		List<int> diskMapAsInts = [.. denseDiskMap.AsDigits<int>()];
		int blockCount = diskMapAsInts.Sum();

		List<Block> diskMap = [];

		bool fileOrFreeSpace = FILE;
		int blockNo = 0;
		int maxBlockNo = 0;
		foreach (int item in diskMapAsInts) {
			if (fileOrFreeSpace is FILE) {
				maxBlockNo = blockNo;
				diskMap.Add(new Block(blockNo++, item));
				fileOrFreeSpace = FREE_SPACE;
			} else {
				diskMap.Add(new Block(EMPTY, item));
				fileOrFreeSpace = FILE;
			}
		}


		// Defragment
		for (blockNo = maxBlockNo; blockNo > 0; blockNo--) {
			int blockPtr = diskMap.FindIndex(b => b.Id == blockNo);
			Block fileBlock = diskMap[blockPtr];
			int firstFreeSpace = diskMap.FindIndex(b => b.Id == EMPTY && b.BlockSize >= fileBlock.BlockSize);
			if (blockPtr > firstFreeSpace && firstFreeSpace > 0) {
				if (diskMap[blockPtr - 1].Id is EMPTY) {
					diskMap[blockPtr - 1] = diskMap[blockPtr - 1] with { BlockSize = diskMap[blockPtr - 1].BlockSize + fileBlock.BlockSize };
					diskMap.RemoveAt(blockPtr);
				} else {
					diskMap[blockPtr] = fileBlock with { Id = EMPTY };
				}
				Block freeSpaceBlock = diskMap[firstFreeSpace];
				diskMap[firstFreeSpace] = freeSpaceBlock with { BlockSize = freeSpaceBlock.BlockSize - fileBlock.BlockSize };
				diskMap.Insert(firstFreeSpace, fileBlock);
			}
			for (int i = diskMap.Count - 1; i > 0; i--) {
				if (diskMap[i].Id is EMPTY && diskMap[i - 1].Id is EMPTY) {
					diskMap[i - 1] = diskMap[i - 1] with { BlockSize = diskMap[i - 1].BlockSize + diskMap[i].BlockSize };
					diskMap.RemoveAt(i);
				}

			}
		}

		_ = diskMap.RemoveAll(b => b.BlockSize is 0);

		long checkSum = 0;
		int idx = 0;
		for (int i = 0; i < diskMap.Count; i++) {
			if (diskMap[i].Id is not EMPTY) {
				for (int j = 0; j < diskMap[i].BlockSize; j++) {
					checkSum += diskMap[i].Id * idx++;
				}
			} else {
				idx += diskMap[i].BlockSize;
			}
		}


		return checkSum;
	}

	private sealed record Block(int Id, int BlockSize);

	private const bool FILE = true;
	private const bool FREE_SPACE = false;
	private const int EMPTY = int.MinValue;
	private const int NOT_FOUND = -1;
}
