namespace AdventOfCode.Solutions._2020;

/// <summary>
/// Day 3: Toboggan Trajectory
/// https://adventofcode.com/2020/day/3
/// </summary>
[Description("Toboggan Trajectory")]
public static class Day03 {
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	public static long Solution1(string[] input) {
		return CalculateNoOfTrees(input, 3, 1);
	}

	public static long Solution2(string[] input) {
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
