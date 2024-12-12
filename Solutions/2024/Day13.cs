namespace AdventOfCode.Solutions._2024;

/// <summary>
/// Day 13: Claw Contraption
/// https://adventofcode.com/2024/day/13
/// </summary>
[Description("Claw Contraption")]
public static partial class Day13 {

	private static IEnumerable<ClawMachine> _ClawMachines = [];

	[Init]
	public static void LoadClawMachines(string[] input)
		=> _ClawMachines = [.. input.Chunk(4).Select(i => ClawMachine.Parse(i))];

	public static int Part1(string[] _)
	{
		const int maxNoOfButtonPushes = 100;

		int lowestCost = 0;

		foreach (ClawMachine clawMachine in _ClawMachines) {
			if (clawMachine.TryCheapestCost(maxNoOfButtonPushes, out int cost)) {
				lowestCost += cost;
			}
		}

		return lowestCost;
	}

	internal static bool TryCheapestCost(this ClawMachine clawmachine, int noOfPresses, out int cost)
	{
		const int buttonACost = 3;
		const int buttonBCost = 1;

		int lowestCost = int.MaxValue;
		for (int b = 0; b < noOfPresses; b++) {
			for (int a = 0; a < noOfPresses; a++) {
				if ((a * clawmachine.A.DX) + (b * clawmachine.B.DX) == clawmachine.PrizeLocation.X
					&& (a * clawmachine.A.DY) + (b * clawmachine.B.DY) == clawmachine.PrizeLocation.Y) {
					lowestCost = int.Min(lowestCost, (a * buttonACost) + (b * buttonBCost));
				}
			}
		}

		if (lowestCost != int.MaxValue) {
			cost = lowestCost;
			return true;
		}

		cost = 0;
		return false;
	}

	public static string Part2(string[] input, params object[]? args) => NO_SOLUTION_WRITTEN_MESSAGE;



	public sealed record Button(string Name, int DX, int DY);

	public sealed record ClawMachine(Button A, Button B, Point PrizeLocation)
	{
		public static ClawMachine Parse(IEnumerable<string> s)
		{
			string[] lines = [.. s];

			int[] numbers = [..lines[0].Replace("Button A: X+", "").Replace("Y+", "").AsInts()];
			Button A = new("A", numbers[0], numbers[1]);

			numbers = [..lines[1].Replace("Button B: X+", "").Replace("Y+", "").AsInts()];
			Button B = new("B", numbers[0], numbers[1]);

			numbers = [..lines[2].Replace("Prize: X=", "").Replace("Y=", "").AsInts()];
			Point prizeLocation = new(numbers[0], numbers[1]);

			return new(A, B, prizeLocation);
		}
	}
}
