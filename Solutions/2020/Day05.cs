namespace AdventOfCode.Solutions._2020;

/// <summary>
/// Day 5: Binary Boarding
/// https://adventofcode.com/2020/day/5
/// </summary>
[Description("Binary Boarding")]
public class Day05 {
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static long Solution1(string[] input) {
		int highest = 0;
		foreach (string line in input) {
			int row = Convert.ToInt32(line[..7].Replace("B", "1").Replace("F", "0"), 2);
			int col = Convert.ToInt32(line[7..].Replace("R", "1").Replace("L", "0"), 2);
			int uId = (8 * row) + col;
			if (uId > highest) {
				highest = uId;
			}
		}
		return highest;
	}

	private static long Solution2(string[] input) {
		List<int> map = [];
		foreach (string line in input) {
			int row = Convert.ToInt32(line[..7].Replace("F", "0").Replace("B", "1"), 2);
			int col = Convert.ToInt32(line[7..].Replace("L", "0").Replace("R", "1"), 2);
			int uid = (8 * row) + col;
			map.Add(uid);
		}
		int myUid = 0;
		for (int seat = map.Min(); seat < map.Max(); seat++) {
			if (map.Contains(seat)) {
				continue;
			}
			if (map.Contains(seat + 1) && map.Contains(seat - 1)) {
				myUid = seat;
				break;
			}
		}

		return myUid;
	}
}
