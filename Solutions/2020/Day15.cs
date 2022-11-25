namespace AdventOfCode.Solutions._2020;

/// <summary>
/// Day 15: Rambunctious Recitation
/// https://adventofcode.com/2020/day/15
/// </summary>
[Description("Rambunctious Recitation")]
public class Day15 {

	public static string Part1(string[] input, params object[]? args) {
		int noOfIterations = GetArgument(args, 1, 2020);
		return Solution(input, noOfIterations).ToString();
	}
	public static string Part2(string[] input, params object[]? args) {
		int noOfIterations = GetArgument(args, 1, 30000000);
		return Solution(input, noOfIterations).ToString();
	}

	record Spoken(int No, int TurnNo, bool FirstTime);

	private static int Solution(string[] input, int noOfIterations) {
		string[] inputs = input[0].Split(",");

		int newLastNo;
		Dictionary<int, Spoken> gamePlay = new();

		Spoken lastGo = new(0, 0, false);
		for (int turnNo = 1; turnNo <= inputs.Length; turnNo++) {
			lastGo = new Spoken(int.Parse(inputs[turnNo - 1]), turnNo, true);
			gamePlay.Add(lastGo.No, lastGo);
		}

		for (int turnNo = inputs.Length + 1; turnNo <= noOfIterations; turnNo++) {
			if (lastGo.FirstTime) {
				newLastNo = 0;
				gamePlay[lastGo.No] = lastGo with { FirstTime = false };
			} else {
				newLastNo = turnNo - 1 - gamePlay[lastGo.No].TurnNo;
				gamePlay[lastGo.No] = lastGo with { TurnNo = turnNo - 1 };
			}
			lastGo = new(newLastNo, turnNo, !gamePlay.ContainsKey(newLastNo));
		}

		return lastGo.No;
	}
}
