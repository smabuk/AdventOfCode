using BigLocation = (long X, long Y);

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

	public static long Part2(string[] _, params object[] args)
	{
		const long BIG_OFFSET = 10_000_000_000_000;

		long lowestCost = 0;

		foreach (ClawMachine clawMachine in _ClawMachines) {
			ClawMachine bigClawMachine = clawMachine with
				{ PrizeLocation = new(clawMachine.PrizeLocation.X + BIG_OFFSET, clawMachine.PrizeLocation.Y + BIG_OFFSET)};
			if (args.Testing()) { bigClawMachine = clawMachine with { }; }

			if (bigClawMachine.TryCheapestCostBig(out long cost)) {
				lowestCost += cost;
			}
		}

		return lowestCost;
	}

	internal static bool TryCheapestCostBig(this ClawMachine clawmachine, out long cost)
	{
		const int buttonACost = 3;

		long ax = clawmachine.A.DX;
		long bx = clawmachine.B.DX;
		long px = clawmachine.PrizeLocation.X;

		long ay = clawmachine.A.DY;
		long by = clawmachine.B.DY;
		long py = clawmachine.PrizeLocation.Y;

		long axby = ax * by;
		long pxby = px * by;

		long aybx = ay * bx;
		long pybx = py * bx;

		long X = axby - aybx;
		long P = pxby - pybx;


		if (P % X == 0) {
			long a = P / X;
			if ((px - (ax * a)) % bx == 0) {
				long b = (px - (ax * a)) / bx;
				cost = (a * buttonACost) + b;
				return true;
			}
		}

		cost = 0;
		return false;
	}

	public sealed record Button(string Name, int DX, int DY);

	public sealed record ClawMachine(Button A, Button B, BigLocation PrizeLocation)
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

	private static bool Testing(this object[]? args) => GetArgument(args, 1, false);

}
