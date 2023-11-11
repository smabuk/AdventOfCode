namespace AdventOfCode.Solutions._2018;

/// <summary>
/// Day 09: Marble Mania
/// https://adventofcode.com/2018/day/09
/// </summary>
[Description("Marble Mania")]
public sealed partial class Day09 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		string[] inputs = input[0].Split(" ");
		int noOfPlayers = int.Parse(inputs[0]);
		int lastMarble = int.Parse(inputs[6]);

		int[] elfScores = new int[noOfPlayers];
		List<int> circle = [0, 1];
		int currentMarblePosition = 1;
		int currentElf = 0;

		for (int marble = 2; marble <= lastMarble; marble++) {
			currentElf = (currentElf + 1) % noOfPlayers;
			if (marble % 23 == 0) {
				elfScores[currentElf] += marble;
				int nextPosition = (currentMarblePosition >= 7) switch {
					true  => currentMarblePosition - 7,
					false => currentMarblePosition + circle.Count - 7,
				};
				elfScores[currentElf] += circle[nextPosition];
				circle.RemoveAt(nextPosition);
				currentMarblePosition = nextPosition % circle.Count;
			} else {
				int nextPosition = (currentMarblePosition + 2) % circle.Count;
				if (nextPosition == 0) {
					circle.Add(marble);
					nextPosition = circle.Count - 1;
				} else {
					circle.Insert(nextPosition, marble);
				}
				currentMarblePosition = nextPosition;
			}
		}

		return elfScores.Max();
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}
}
