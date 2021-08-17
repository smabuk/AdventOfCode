namespace AdventOfCode.Solutions.Year2020;

/// <summary>
/// Day 3: Toboggan Trajectory
/// https://adventofcode.com/2020/day/3
/// </summary>
public static class Day03 {
	public static long Part1(string[]? input = null) {
		input = input.StripTrailingBlankLineOrDefault();
		return CalculateNoOfTrees(input, 3, 1);
	}

	public static long Part2(string[]? input = null) {
		input = input.StripTrailingBlankLineOrDefault();

		List<long> resultArray = new();
		resultArray.Add(CalculateNoOfTrees(input, 1, 1));
		resultArray.Add(CalculateNoOfTrees(input, 3, 1));
		resultArray.Add(CalculateNoOfTrees(input, 5, 1));
		resultArray.Add(CalculateNoOfTrees(input, 7, 1));
		resultArray.Add(CalculateNoOfTrees(input, 1, 2));

		return resultArray.Aggregate(1, (long treeProduct, long treeCount) => treeProduct * treeCount);
	}


	public const char TREE = '#';
	public const char SPACE = '.';

	public static int CalculateNoOfTrees(string[] map, int right, int down) {
		int currentColumn = 0;
		int mapWidth = map[0].Length;
		int noOfTreesHit = 0;

		for (int currentRow = 0; currentRow < map.Length; currentRow += down) {
			if (map[currentRow][currentColumn] == TREE) {
				noOfTreesHit++;
			}
			currentColumn = (currentColumn + right) % mapWidth;
		}

		return noOfTreesHit;
	}

}
