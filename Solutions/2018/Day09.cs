namespace AdventOfCode.Solutions._2018;

/// <summary>
/// Day 09: Marble Mania
/// https://adventofcode.com/2018/day/09
/// </summary>
[Description("Marble Mania")]
public sealed partial class Day09 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static long Solution1(string[] input)
	{
		string[] inputs = input[0].Split(" ");
		int noOfPlayers = int.Parse(inputs[0]);
		int lastMarble = int.Parse(inputs[6]);

		return PlayGameAndReturnWinningScore(noOfPlayers, lastMarble);
	}

	private static long Solution2(string[] input) {
		string[] inputs = input[0].Split(" ");
		int noOfPlayers = int.Parse(inputs[0]);
		int lastMarble = int.Parse(inputs[6]);

		return PlayGameAndReturnWinningScore(noOfPlayers, lastMarble * 100);
	}

	private static long PlayGameAndReturnWinningScore(int noOfPlayers, int lastMarble)
	{
		long[] elfScores = new long[noOfPlayers];
		LinkedList<int> circle = [];
		LinkedListNode<int> currentMarble = circle.AddFirst(0);
		currentMarble = circle.AddAfter(currentMarble, 1);
		int currentElf = 0;

		for (int marbleValue = 2; marbleValue <= lastMarble; marbleValue++) {
			currentElf = (currentElf + 1) % noOfPlayers;
			if (marbleValue % 23 == 0) {
				elfScores[currentElf] += marbleValue;
				for (int i = 0; i < 6; i++) {
					currentMarble = currentMarble?.Previous ?? circle.Last!;
				}
				elfScores[currentElf] += (currentMarble?.Previous ?? circle.Last!).Value;
				circle.Remove(currentMarble?.Previous ?? circle.Last!);
			} else {
				currentMarble = circle.AddAfter(currentMarble?.Next ?? circle.First!, marbleValue);
			}
		}

		return elfScores.Max();
	}
}
