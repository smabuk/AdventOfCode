namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 09: Disk Fragmenter
/// https://adventofcode.com/2024/day/09
/// </summary>
[Description("Disk Fragmenter")]
public partial class Day09 {

	public static long Part1(string[] input)
	{
		string densediskMap = input[0];
		List<int> diskMapAsInts = [.. densediskMap.AsDigits<int>()];
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

	public static string Part2(string[] input) => NO_SOLUTION_WRITTEN_MESSAGE;


	private sealed record Block(int id, int BlockSize);

	private const bool FILE = true;
	private const bool FREE_SPACE = false;
	private const int EMPTY = int.MinValue;
}
